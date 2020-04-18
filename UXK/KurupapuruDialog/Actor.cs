using System;

namespace UXK.KurupapuruDialog
{
    [Serializable]
    public class Actor
    {
        public string MainName
        {
            get
            {
                if (AllNames == null || AllNames.Length == 0)
                    return null;
                else
                    return AllNames[0];
            }
        }
        public string[] AllNames;
        public string TranslatedName;

        public Actor(string[] allNames, string translatedName)
        {
            AllNames = allNames;
            TranslatedName = translatedName;
        }
    }
}