using System;
using MessagePack;

namespace UXK.Inventory
{
    [Serializable]
    [MessagePackObject()]
    public class BagConfig : Item, IBagConfig
    {
        
    }
}