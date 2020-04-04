// using System;
// using System.Collections.Generic;
// using DefaultNamespace;
//
// namespace GameSystems.Inventory
// {
//     public class RootBag : IBag
//     {
//         // private IReadOnlyDictionary<IItem, uint> _items;
//         // private IBag _parentBag;
//         // private bool _subWeightMultipliers;
//         // private float _maxWeight;
//         // private float _weightMultiplier = 1;
//         //
//         // public IReadOnlyDictionary<IItem, uint> Items => _items;
//         //
//         // public AddItemResultMessage AddItem(IItem item, uint count)
//         // {
//         //     IBag addedBag = item as IBag;
//         //     if (addedBag == null) return new AddItemResultMessage(0);
//         //     
//         //     throw new NotImplementedException();
//         // }
//         // public IBag ParentBag => _parentBag;
//         // public bool SubWeightMultipliersAllowed => _subWeightMultipliers;
//         // public float MaxWeight => _maxWeight;
//         // public float SelfWeightMultiplier => _weightMultiplier;
//         // public float CanAddWeight() { throw new System.NotImplementedException(); }
//     }
// }