using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Sirenix.OdinInspector;
using TU.Sharp.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))] [ExecuteInEditMode]
public class SizeFromChilds : MonoBehaviourInvokable
{
    public RectTransform setSizeTo;
    public RectTransform readSizeFrom;
    public bool onUpdate   = false;
    public bool copyHeight = false;
    public bool copyWidth  = false;
    public bool сheckIgnoreLayout = false;

    private void Update()
    {
        if (onUpdate)
            Invoke();
    }

    [Button]
    public override void Invoke()
    {
        if (copyHeight)
        {
            float heightSum = 0;
            foreach (RectTransform child in readSizeFrom)
                if (!isIgnoreLayout(child)) heightSum += child.rect.height;
            setSizeTo.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heightSum);
        }
        
        if (copyWidth)
        {
            float widthSum = 0;
            foreach (RectTransform child in readSizeFrom)
                if (!isIgnoreLayout(child)) widthSum += child.rect.width;
            setSizeTo.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthSum);
        }
    }

    private bool isIgnoreLayout(RectTransform rectTransform)
    {
        if (сheckIgnoreLayout == false) return false;

        var layoutElementComponent = rectTransform.GetComponent<LayoutElement>();
        if (layoutElementComponent == null) return false;

        return layoutElementComponent.ignoreLayout;
    }
}
