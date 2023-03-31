using UnityEngine;

namespace TopDownPlate
{
    public class ConditionHiden : PropertyAttribute
    {
        public string ConditionString = "";
        public bool Hidden = false;

        public ConditionHiden(string conditionString)
        {
            this.ConditionString = conditionString;
            this.Hidden = false;
        }

        public ConditionHiden(string conditionString, bool hideInInspector)
        {
            this.ConditionString = conditionString;
            this.Hidden = hideInInspector;
        }
    }
}
