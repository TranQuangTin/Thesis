using UnityEngine;
using System;
using System.Collections;

namespace Alta.Plugin
{
    public static class RectTransformExtensions
    {

        public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
        {
            trans.anchoredPosition = new Vector2(newPos.x, newPos.y);
        }

        public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.anchorMin = Vector2.zero;
            trans.anchorMax = Vector2.zero;
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        }
        public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.anchorMin = Vector2.up;
            trans.anchorMax = Vector2.up;
            trans.anchoredPosition = new Vector2(newPos.x + (trans.pivot.x * trans.sizeDelta.x), newPos.y - ((1f - trans.pivot.y) * trans.sizeDelta.y));
        }
        public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.anchorMin = new Vector2(1, 0);
            trans.anchorMax = new Vector2(1, 0);
            trans.anchoredPosition = new Vector2(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height));
        }
        public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.anchorMin = Vector2.one;
            trans.anchorMax = Vector2.one;
            trans.anchoredPosition = new Vector2(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height));
        }

        public static void SetSize(this RectTransform trans, Vector2 newSize)
        {
            Vector2 oldSize = trans.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
            trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
        }
        public static void SetWidth(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(newSize, trans.rect.size.y));
        }
        public static void SetHeight(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(trans.rect.size.x, newSize));
        }

    }
}