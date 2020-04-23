using System;
using System.Collections.Generic;

namespace UXK.KurupapuruDialog
{
    [Serializable]
    public class DialogDecision : DialogContent
    {
        public List<DialogDecisionOption> Options = new List<DialogDecisionOption>();
    }

    [Serializable]
    public class DialogDecisionOption : DialogNode
    {
        public string OptionName;

        public DialogDecisionOption(string optionName, string currentNodeName) : base(string.Concat(currentNodeName, ".", optionName))
        {
            this.OptionName = optionName;
        }
    }
}