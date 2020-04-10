using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IHasId
{
    int Id { get; }
}

public interface IViewOfHasId<T> where T : IHasId
{
    RectTransform RectTransform { get; }
    T Value { get; }

    void SetupFor(T value);
}

public class InfiniteScroller<TItem, TView> : MonoBehaviour 
    where TItem : IHasId 
    where TView : Component, IViewOfHasId<TItem>
{
    public ScrollRect scrollRect;
    public TView itemPrefab;
    public float itemSize;

    private RectTransform container;
    private RectTransform scrollRectT;

    private List<TItem> itemsList;
    public Dictionary<TItem, TView> activeViews;
    private List<TView> despawnedViews;
    public Action<TView> onViewSpawn;
    
    private int NeededViewsCount => Mathf.CeilToInt(scrollRectT.rect.height / itemSize) + 2; 

    public void Initialize(List<TItem> itemsList, Action<TView> onViewSpawn = null)
    {
        this.itemsList = itemsList;
        this.onViewSpawn = onViewSpawn;

        scrollRectT = (RectTransform)scrollRect.transform;
        container = scrollRect.content;
        scrollRect.onValueChanged.RemoveAllListeners();
        scrollRect.onValueChanged.AddListener(UpdateViews);

        int predictedItemsCount = NeededViewsCount; 
        if (activeViews == null)
            activeViews = new Dictionary<TItem, TView>(predictedItemsCount);
        if (despawnedViews == null)
            despawnedViews = new List<TView>(predictedItemsCount);
        
        UpdateViews();
    }

    private void Update()
    {
        if (wasListChanged)
            OnAfterListChange();
    }

    private void UpdateViews(Vector2 normalizedScrollPosition) => UpdateViews();

    private void UpdateViews()
    {
        var verticalNormalizedPosition = scrollRect.verticalNormalizedPosition;
        
        var scrollHeight    = scrollRectT.rect.height;
        var containerHeight = container.rect.height;
        if (containerHeight < scrollHeight)
            containerHeight = scrollHeight;

        var verticalPixelPosition = verticalNormalizedPosition * (containerHeight - scrollHeight);
        var centerIndex           = Mathf.RoundToInt((verticalPixelPosition + scrollHeight / 2) / itemSize);

        DespawnAllViews();

        var viewsCount = NeededViewsCount;
        var startIndex = Mathf.Clamp(centerIndex - viewsCount / 2, 0, itemsList.Count);
        var endIndex   = Mathf.Clamp(centerIndex + viewsCount / 2, 0, itemsList.Count);
        for (int i = startIndex; i < endIndex; i++)
        {
            var item = itemsList[i];
            var view = GetView();
            view.SetupFor(item);
            view.RectTransform.anchoredPosition = new Vector2(0, i * itemSize);
            activeViews[item]                   = view;
        }
    }

    private TView GetView()
    {
        TView result;
        
        if (despawnedViews.Count > 0)
        {
            result = despawnedViews[despawnedViews.Count-1];
            despawnedViews.RemoveAt(despawnedViews.Count-1);
            result.gameObject.SetActive(true);
        }
        else
        {
            result = Instantiate(itemPrefab.gameObject, container).GetComponent<TView>();
        }
        
        onViewSpawn?.Invoke(result);
        
        return result;
    }
    
    private void DespawnAllViews()
    {
        foreach (var activeView in activeViews)
        {
            activeView.Value.gameObject.SetActive(false);
            despawnedViews.Add(activeView.Value);
        }
        activeViews.Clear();
    }

    #region Fixing Scroll On List Change

    private int? wasOnId    = null;
    private bool wasOnBegin = false;
    private bool wasListChanged = false;
    
    /// <summary>
    /// Invoke this before changing list
    /// </summary>
    public void OnBeforeListChange()
    {
        if (wasListChanged)
            return;

        wasListChanged = true;
        
        if (scrollRect.verticalNormalizedPosition < 0.05f)
        {
            wasOnBegin = true;
            return;
        }
        
        var scrollHeight    = scrollRectT.rect.height;
        var containerHeight = container.rect.height;
        if (containerHeight < scrollHeight)
            containerHeight = scrollHeight;

        var verticalPixelPosition = scrollRect.verticalNormalizedPosition * (containerHeight - scrollHeight);
        var wasOnIndex            = Mathf.RoundToInt((verticalPixelPosition + (scrollHeight / 2)) / itemSize);
        if (itemsList.Count < wasOnIndex)
        {
            if (itemsList.Count > 0)
                wasOnIndex = 0;
            else
                return;
        }
        wasOnId = itemsList[wasOnIndex].Id;
    }

    private void OnAfterListChange()
    {
        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemSize * itemsList.Count);

        if (wasOnBegin)
        {
            scrollRect.verticalNormalizedPosition = 0;

            wasOnBegin = false;
        }

        if (wasOnId.HasValue)
        {
            var scrollHeight    = scrollRectT.rect.height;
            var containerHeight = container.rect.height;
            if (containerHeight < scrollHeight)
                containerHeight = scrollHeight;

            var wasOnIndex                 = itemsList.FindIndex(x => x.Id == wasOnId.Value);
            if (wasOnIndex == -1)
            {
                // item with this id was deleted, snapping to top position (nearest to deleted)
                scrollRect.verticalNormalizedPosition = 1f;
            }
            else
            {
                var verticalPixelPosition      = wasOnIndex * itemSize - (scrollHeight / 2);
                var verticalNormalizedPosition = verticalPixelPosition / (containerHeight - scrollHeight);
                verticalNormalizedPosition            = Mathf.Clamp01(verticalNormalizedPosition);
                scrollRect.verticalNormalizedPosition = verticalNormalizedPosition;
            }

            wasOnId = null;
        }

        wasListChanged = false;
        
        UpdateViews();
    }
    #endregion
}
