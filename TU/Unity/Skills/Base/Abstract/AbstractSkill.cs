using System;
using MGame.Code.Skills.Base.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MGame.Code.Skills.Base.Abstract
{
    public abstract class AbstractSkill : SerializedScriptableObject, ISkill
    {
        [SerializeField] protected float cooldownMax;

        private float _lastUseTime = float.MinValue;
        public virtual float CooldownCurrent
        {
            get
            {
                var endTime = _lastUseTime + CooldownMax;
                var result = endTime - Time.time;
                if (result < 0) result = 0;
                return result;
            }
        }

        public virtual float CooldownMax => cooldownMax;

        public virtual bool IsInCooldown => CooldownCurrent > 0;

        public virtual ISkill GetInstance()
        {
            return Instantiate(this);
        }

        public virtual bool Use(ISkillsUser user)
        {
            if (IsInCooldown)
            {
                Debug.LogError($"Skill \"{name}\" is in cooldown (Current Colldown: {CooldownCurrent}s)");
                return false;
            }


            UseBehaviour(user);


            _lastUseTime = Time.time;
            return true;
        }

        public abstract void UseBehaviour(ISkillsUser user);
    }
}