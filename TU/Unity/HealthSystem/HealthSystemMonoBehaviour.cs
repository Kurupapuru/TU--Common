using System;
using System.Threading.Tasks;
using TU.Unity.HealthSystem.Interfaces;
using UniRx;
using UnityEngine;

namespace TU.Unity.HealthSystem
{
    public class HealthSystemMonoBehaviour : MonoBehaviour, IHealthSystem
    {
        public int identificator = 0;

        [SerializeField] protected FloatReactiveProperty _maxHealth = new FloatReactiveProperty(100);
        [SerializeField] protected FloatReactiveProperty _health    = new FloatReactiveProperty(100);
        protected  BoolReactiveProperty _alive                = new BoolReactiveProperty(true);
        protected  IntReactiveProperty  _immortalityBuffsCount = new IntReactiveProperty(0);

        public IReadOnlyReactiveProperty<int> ImmortalityBuffsCount => _immortalityBuffsCount;
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth;
        public IReadOnlyReactiveProperty<float> Health    => _health;
        public IReadOnlyReactiveProperty<bool>  Alive     => _alive;
        
        public virtual void ReceiveDamage(DamagePack damagePack) => AddHealth(-damagePack.value);
        public virtual void ReceiveHeal(HealPack healPack)       => AddHealth(healPack.value);

        public void Awake()
        {
            MessageBroker.Default
                .Receive<HealthInfluenceMessage>()
                .Where(m => m.identificator == identificator)
                .Subscribe(HealthInfluence)
                .AddTo(this);

            MessageBroker.Default
                .Receive<ImmortalityMessage>()
                .Where(m => m.identificator == identificator)
                .Subscribe(m => Immortality(m.timeLength))
                .AddTo(this);
        }

        public void HealthInfluence(HealthInfluenceMessage message)
        {
            if (Alive.Value == false && !message.canRevive) return;

            float newHealth;

            if (message.procents)
                newHealth = Health.Value + MaxHealth.Value * message.influence;
            else
                newHealth = Health.Value + message.influence;
            
            if (newHealth > 0)
                _alive.Value = true;
            
            SetHealth(newHealth);
        }

        public async void Immortality(float timeLength)
        {
            _immortalityBuffsCount.Value++;

            await Task.Delay(TimeSpan.FromSeconds(timeLength));

            if (this == null) return;
            _immortalityBuffsCount.Value--;
        }

        public virtual void InitializeHealth(float health, float maxHealth)
        {
            _alive.Value = true;
            _maxHealth.Value = maxHealth;
            _health.Value = health;
        }

        protected virtual void AddHealth(float addHealth)
        {
            if (addHealth < 0 && ImmortalityBuffsCount.Value > 0) return;
            SetHealth(Health.Value + addHealth);
        }
        
        protected virtual void SetHealth(float newHealth)
        {
            if (!Alive.Value) return;

            if (newHealth >= MaxHealth.Value)
            {
                _health.Value = MaxHealth.Value;
            }
            else if (newHealth <= 0)
            {
                _alive.Value = false;
                _health.Value = 0;
            }
            else
            {
                _health.Value = newHealth;
            }
        }
    }
}