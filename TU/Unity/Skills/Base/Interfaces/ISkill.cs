namespace MGame.Code.Skills.Base.Interfaces
{
    public interface ISkill
    {
        float CooldownCurrent { get; }
        float CooldownMax { get; }

        bool IsInCooldown { get; }

        ISkill GetInstance();
        
        /// <returns>Is used succesfully?</returns>
        bool Use(ISkillsUser user);
    }
}