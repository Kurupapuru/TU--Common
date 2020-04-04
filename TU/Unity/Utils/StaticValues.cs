using UnityEngine;

public static class StaticValues
{
    #region Layers
    public static LayerInfo Default = new LayerInfo("Default");
    public static LayerInfo Ground = new LayerInfo("Ground");
    public static LayerInfo Interactable = new LayerInfo("Interactable");
    public static LayerInfo ItemPlace = new LayerInfo("ItemPlace");
    public static LayerInfo Invisible = new LayerInfo("ItemPlace");
    #endregion Layers  
}


public class LayerInfo
{
    public LayerInfo (string name)
    {
        this.name = name;
    }

    public string name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            layer = LayerMask.NameToLayer(value);
            layerMask = LayerMask.GetMask(value);
        }
    }
    public int layer;
    public int layerMask;

    //Saved name
    private string _name;
}
