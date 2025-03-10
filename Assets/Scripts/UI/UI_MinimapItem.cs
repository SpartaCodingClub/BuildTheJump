using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MinimapItem : UI_Base
{
    #region Open
    private Sequence Open_MinimapItem()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(1.0f, 0.3f).From(0.0f));
    }
    #endregion
    #region Close
    private Sequence Close_MinimapItem()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(0.0f, 0.3f));
    }
    #endregion

    private enum Children
    {
        Icon,
        Text_Distance
    }

    public Transform Target { get; private set; }

    private RectTransform rectTransform;
    private TMP_Text textDistance;

    private bool closed;
    private Sequence close;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, Open_MinimapItem);

        rectTransform = GetComponent<RectTransform>();
        textDistance = Get<TMP_Text>((int)Children.Text_Distance);
        close = Close_MinimapItem();
    }

    public void Initialize(Transform target, Transform parent, SpriteType type, string id)
    {
        Target = target;

        rectTransform.SetParent(parent);
        rectTransform.anchoredPosition = 50.0f * Vector2.up;

        gameObject.name = $"{nameof(UI_MinimapItem)} ({target.name})";
        Get<Image>((int)Children.Icon).sprite = Managers.Resource.GetSprite(type, id);
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
        close.Kill();
    }

    public void UpdateUI(float x, float distance)
    {
        if (closed == false && distance < 10.0f)
        {
            closed = true;
            close.Restart();
        }
        else if (closed && distance > 10.0f)
        {
            closed = false;
            Open();
        }

        rectTransform.anchoredPosition = new(x, rectTransform.anchoredPosition.y);
        textDistance.text = $"{distance:F1}m";

        //float t = Mathf.InverseLerp(MIN_DISTANCE, MAX_DISTANCE, Mathf.Clamp(distance, MIN_DISTANCE, MAX_DISTANCE));
        //canvasGroup.alpha = Mathf.SmoothStep(0.0f, 1.0f, t);
    }
}