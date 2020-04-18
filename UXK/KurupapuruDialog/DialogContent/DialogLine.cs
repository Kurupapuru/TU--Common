using System;

namespace UXK.KurupapuruDialog
{
    [Serializable]
    public class DialogLine : DialogContent
    {
        public string Actor;
        public string Phrase;

        public DialogLine(string actor, string phrase)
        {
            Actor  = actor;
            Phrase = phrase;
        }
    }
}