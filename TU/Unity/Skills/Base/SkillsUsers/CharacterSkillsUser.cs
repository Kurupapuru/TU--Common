using System;
using System.ComponentModel;
using MGame.Code.Characters;
using MGame.Code.Skills.Base.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MGame.Code.Skills.Base.SkillsUsers
{
    public class CharacterSkillsUser : MonoBehaviour, ISkillsUser
    {
        [SerializeField] private LayerMask _invisiblePlaneLayerMask;
        public LayerMask invisiblePlaneLayerMask => _invisiblePlaneLayerMask;
        [SerializeField] private LayerMask _attackMask;
        public LayerMask attackMask => _attackMask;
        public Transform UserTransform => transform;
        public Rigidbody UserRigidbody => userRigidbody;

        [SerializeField] [InlineEditor()] private CharacterSettings _characterSettings;
        [SerializeField] private KeyCode[] skillKeys;
        [SerializeField] private Rigidbody userRigidbody;

        public CharacterSettings CharacterSettings => _characterSettings;

        private ISkill[] _skills;

        private void Awake()
        {
            _skills = CharacterSettings.CreateRuntimeSkills();
        }

        private void Update()
        {
            for (var i = 0; i < skillKeys.Length; i++)
            {
                if (_skills[i] != null && Input.GetKeyDown(skillKeys[i]))
                    _skills[i].Use(this);
            }
        }
    }
}