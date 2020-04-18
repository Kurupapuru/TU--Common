using System;

namespace UXK.KurupapuruDialog
{
    [Serializable]
    public class DialogCondition : DialogContent
    {
        public DialogMethod ConditionMethod;
        public DialogNode   OnTrueNode;
        public DialogNode   OnFalseNode;
    }
}