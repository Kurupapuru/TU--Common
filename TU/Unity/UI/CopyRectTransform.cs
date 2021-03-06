﻿using DefaultNamespace;
using NaughtyAttributes;
using UnityEngine;

namespace TU.Unity.UI
{
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
}
