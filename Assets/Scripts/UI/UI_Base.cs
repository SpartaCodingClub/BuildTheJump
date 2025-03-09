using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    Open,
    Close
}

[RequireComponent(typeof(CanvasGroup))]
public abstract class UI_Base : MonoBehaviour
{
    protected CanvasGroup canvasGroup;

    private readonly SequenceHandler sequenceHandler = new();
    private readonly List<RectTransform> children = new();

    private void Awake() => Initialize();
    private void OnDestroy() => Deinitialize();

    protected void BindSequences(UIState type, params Func<Sequence>[] sequences) => sequenceHandler.Bind(type, sequences);
    protected RectTransform Get(int index) => children[index];
    protected T Get<T>(int index) where T : Component => Get(index).GetComponent<T>();

    protected virtual void Initialize()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        sequenceHandler.Initialize();
        sequenceHandler.Close.OnComplete(() =>
        {
            if (!TryGetComponent<Canvas>(out var canvas))
            {
                return;
            }

            if (canvas.worldCamera != null)
            {
                gameObject.SetActive(false);
                return;
            }

            Managers.Resource.Destroy(gameObject);
        });
    }

    protected virtual void Deinitialize()
    {
        sequenceHandler.Deinitialize();
    }

    public virtual void Open()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        sequenceHandler.Open.Restart();
    }

    public virtual void Close()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        sequenceHandler.Close.Restart();
    }

    protected void BindChildren(Type enumType)
    {
        var names = Enum.GetNames(enumType);
        foreach (var name in names)
        {
            RectTransform child = gameObject.FindComponent<RectTransform>(name);
            children.Add(child);
        }
    }
}