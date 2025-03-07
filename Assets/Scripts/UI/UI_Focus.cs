using DG.Tweening;
using UnityEngine;

public class UI_Focus : UI_Base
{
    private RectTransform rectTransform;

    private Sequence opened;

    protected override void Initialize()
    {
        base.Initialize();

        rectTransform = GetComponent<RectTransform>();
        opened = DOTween.Sequence().SetLoops(-1)
            .Append(rectTransform.DOScale(1.1f, 0.3f))
            .Append(rectTransform.DOScale(1.0f, 0.3f));

        gameObject.SetActive(false);
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
        opened.Kill();
    }

    public void UpdateUI(RectTransform rectTransform)
    {
        if (rectTransform == null)
        {
            gameObject.SetActive(false);
            return;
        }

        this.rectTransform.SetParent(rectTransform);
        this.rectTransform.localPosition = Vector3.zero;

        gameObject.SetActive(true);
    }
}