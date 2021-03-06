using MGame.Code.Skills.Base.Interfaces;
using UnityEngine;

namespace MGame.Code.Characters
{
    [CreateAssetMenu(fileName = "Unnamed Character", menuName = "MGame/Settings/Character")]
    public class CharacterSettings : ScriptableObject
    {
        [field: SerializeField] public string CharacterName { get; }
        [field: SerializeField] public Texture2D Icon { get; }
        [field: SerializeField] public float CooldownMax { get; }
        [field: SerializeField] private ISkill[] skillsSettings { get; } //TODO: ISkill serialization

        public ISkill[] CreateRuntimeSkills()
        {
            ISkill[] result = new ISkill[skillsSettings.Length];

            for (var i = 0; i < skillsSettings.Length; i++)
                if (skillsSettings[i] != null)
                    result[i] = skillsSettings[i].GetInstance();
                else
                    result[i] = null;

            return result;
        }
    }
}