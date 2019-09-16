using System.Reflection;
using Skanderbro.Models;

namespace Skanderbro.Validators
{
    public static class LeaderValidator
    {
        public static bool ArePipModifiersValid(LeaderPipModifiers leaderPipRequest, out string message)
        {
            foreach (PropertyInfo propertyInfo in typeof(LeaderPipModifiers).GetProperties())
            {
                int value = (int)propertyInfo.GetValue(leaderPipRequest);
                if (value < 0 || value > 6)
                {
                    message = $"Category {propertyInfo.Name} is out of range: {value}";
                    return false;
                }
            }

            message = null;
            return true;
        }

        public static bool IsTraditionValid(double tradition, out string message)
        {
            if (tradition < 0 || tradition > 100)
            {
                message = $"Tradition must be between 0 and 100, but the value given was {tradition}";
                return false;
            }
            message = null;
            return true;
        }

        public static bool IsRulerMilitarySkillValid(int militarySkill, out string message)
        {
            if (militarySkill < 0 || militarySkill > 6)
            {
                message = $"Ruler military skill must be between 0 and 6, but the value given was {militarySkill}";
                return false;
            }
            message = null;
            return true;
        }
    }
}
