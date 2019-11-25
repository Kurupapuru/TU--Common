using System.Collections;
using System.Collections.Generic;
using MGame.Code.Skills.Base.Abstract;
using MGame.Code.Skills.Base.Interfaces;
using UnityEngine;

[CreateAssetMenu(menuName = "MGame/Settings/Skill/Simple Jump Skill")]
public class JumpSkill : AbstractSkill
{
    public float jumpForce;
    
    public override void UseBehaviour(ISkillsUser user)
    {
        user.UserRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
