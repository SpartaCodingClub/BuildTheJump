using DG.Tweening;
using UnityEngine;

public class UI_Focus : UI_Base
{
    private Sequence opened;

    protected override void Initialize()
    {
        base.Initialize();

        opened = DOTween.Sequence().SetLoops(-1)
            .Append(transform.DOScale(1.1f, 0.3f))
            .Append(transform.DOScale(1.0f, 0.3f));

        gameObject.SetActive(false);
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
        opened.Kill();
    }

    public void UpdateUI(Transform transform)
    {
        if (transform == null)
        {
            gameObject.SetActive(false);
            return;
        }

        this.transform.SetParent(transform);
        this.transform.localPosition = Vector3.zero;

        gameObject.SetActive(true);
    }
}