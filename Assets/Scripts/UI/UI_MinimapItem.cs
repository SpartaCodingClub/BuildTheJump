using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MinimapItem : UI_Base
{
    private static readonly float MIN_DISTANCE = 10.0f;

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

    private bool isClose;
    private Sequence open;
    private Sequence close;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        rectTransform = GetComponent<RectTransform>();
        textDistance = Get<TMP_Text>((int)Children.Text_Distance);
        open = Open_MinimapItem();
        close = Close_MinimapItem();
    }

    public void Initialize(Transform target, Transform parent, SpriteType type, string id)
    {
        Target = target;

        rectTransform.SetParent(parent);
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = 50.0f * Vector2.up;

        gameObject.name = $"{nameof(UI_MinimapItem)} ({target.name})";
        Get<Image>((int)Children.Icon).sprite = Managers.Resource.GetSprite(type, id);
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
        close.Kill();
    }

    public override void Open()
    {
        base.Open();
        open.Restart();
    }

    public void UpdateUI(float x, float distance)
    {
        bool isClose = distance < MIN_DISTANCE;
        if (isClose != this.isClose)
        {
            this.isClose = isClose;
            if (this.isClose)
            {
                open.Pause();
                close.Restart();
            }
            else
            {
                close.Pause();
                open.Restart();
            }
        }

        rectTransform.anchoredPosition = new(x, rectTransform.anchoredPosition.y);
        textDistance.text = $"{distance:F1}m";
    }
}