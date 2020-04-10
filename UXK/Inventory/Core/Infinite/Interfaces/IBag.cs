using System.Collections.Generic;
using MessagePack;
using TU.Sharp.Extensions;
using UniRx;
using UnityEngine;

namespace UXK.Inventory
{
    [Union(0, typeof(Bag))]
    public interface IBag : IItem
    {
        IBag                         ParentBag { get; set; }
        IBagConfig                   Config    { get; }
        IReadOnlyReactiveDictionary<IItem, uint> Items     { get; }


        // Add
        bool AddItems(params IItemWithAmount[] items);
        bool AddItem(IItemWithAmount           item);

        bool CanAddItems(params IItemWithAmount[] items);
        bool CanAddItem(IItemWithAmount           item);

        // Remove
        bool RemoveItem(IItemWithAmount items);
        bool RemoveItems(params IItemWithAmount[] items);
        
        bool CanRemoveItem(IItemWithAmount item);
        bool CanRemoveItems(params IItemWithAmount[] item);
        


#if INV_WEIGHT
        /// <summary>
        /// Максимальный вместимый вес
        /// </summary>
        public float MaxWeight => Config.MaxWeight;

        /// <summary>
        /// Модификатор веса из конфига
        /// </summary>
        public float SelfWeightMultiplier => Config.WeightMultiplier;

        /// <summary>
        /// Разрешены ли модификаторы веса в дочерних мешках
        /// </summary>
        bool SubWeightMultipliersAllowed => Config.SubWeightMultipliersAllowed;
        
        /// <summary>
        /// Сколько весит мешок и предметы в нём (модификаторы учитываются)
        /// </summary>
        public float Weight
        {
            get
            {
                var weight = 0f;
                var parentBagWeightMult = GetWeightMult(ParentBag);
                weight += Config.Weight * parentBagWeightMult;
                var itemsWeightMult = GetWeightMult(this, parentBagWeightMult);
                weight += ItemsWeightRaw * itemsWeightMult;
                return weight;
            }
        }

        /// <summary>
        /// Общий вес предметов внутри мешка (без модификатора)
        /// </summary>
        float ItemsWeightRaw
        {
            get
            {
                var result = 0f;
                Items.ForEach(x => result += x.Item.Weight * x.Count);
                return result;
            }
        }

        /// <summary>
        /// Общий вес предметов внутри мешка (модификаторы учитываются)
        /// </summary>
        public float FilledWeight => ItemsWeightRaw * GetWeightMult(this);
        
        /// <summary>
        /// Сколько мешок ещё может вместить веса
        /// </summary>
        public float EmptyWeight => MaxWeight - FilledWeight;

        /// <summary>
        /// Сколько веса (не умноженного на модификатор) можно добавить
        /// </summary>
        public float CanAddWeight
        {
            get
            {
                Stack<IBag> fromParentToThis = new Stack<IBag>();
                IBag currentBag = this;
                while (currentBag != null)
                {
                    fromParentToThis.Push(currentBag);
                    currentBag = currentBag.ParentBag;
                }

                var weightMultiplierAllowed = true;
                var weightMult = 1;

                foreach (var bag in fromParentToThis)
                {
                    weightMultiplierAllowed = weightMultiplierAllowed && bag.SubWeightMultipliersAllowed;
                    weightMult *=
                }

                var weightMult = GetWeightMult(this);
                var filledWeight = ItemsWeightRaw * weightMult;
                var emptyWeight = MaxWeight - filledWeight;
                var canAddWeight = emptyWeight / weightMult;
                return canAddWeight;
            }
        }

        
        public float GetCanAddWeightRecursive()
        {
            IBag currentBag = this;

            var canAddWeight = CanAddWeight();

            while (currentBag.ParentBag != null)
            {
                canAddWeight *= currentBag.ParentBag.SelfWeightMultiplier;
                var parentBagEmptyWeight = currentBag.ParentBag.CanAddWeight();
                if (parentBagEmptyWeight < canAddWeight)
                    canAddWeight = parentBagEmptyWeight;
                currentBag = currentBag.ParentBag;
            }

            return canAddWeight;
        }

        
        /// <summary>
        /// Модификатор веса мешка учитывая настройки родительских мешков
        /// </summary>
        private float GetWeightMult(IBag bag)
        {
            var weightMult = 1f;
            var currentBag = bag;
            while (currentBag != null)
            {
                if (currentBag.SubWeightMultipliersAllowed)
                    weightMult *= currentBag.SelfWeightMultiplier;
                else
                    weightMult = currentBag.SelfWeightMultiplier;
                currentBag = currentBag.ParentBag;
            }

            return weightMult;
        }
        
        private float GetWeightMult(IBag bag, float parentBagWeightMult)
        {
            if (!ParentBag.SubWeightMultipliersAllowed)
                return parentBagWeightMult;
            else
                return bag.SelfWeightMultiplier * parentBagWeightMult;
        }

#endif
    }
}