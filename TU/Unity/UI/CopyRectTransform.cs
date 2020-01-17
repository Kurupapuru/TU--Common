using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TU.Sharp.Utils;
using UnityEngine;
using UnityEngine.UI;

public class CopyRectTransform : MonoBehaviourInvokable
{
    public RectTransform fromT;
    public RectTransform toT;

    [Button]
    public override void Invoke()
    {
        toT.anchoredPosition = fromT.anchoredPosition;
        toT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,   fromT.rect.height);
        toT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fromT.rect.width);
    }
}
