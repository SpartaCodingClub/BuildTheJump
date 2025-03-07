using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildingStatus : UI_Base
{
    #region Open
    private Sequence BuildingStatus_Open()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(1.0f, 0.3f).From(0.0f));
    }

    private Sequence Frame_Open()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(1.0f, 0.3f).From(0.0f).SetEase(Ease.OutBack));
    }

    private Sequence Icon_Open()
    {
        var child = Get((int)Children.Icon_Building);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(1.0f, 0.3f).From(0.0f).SetEase(Ease.OutBack));
    }

    private Sequence Text_Name_Open()
    {
        var child = Get((int)Children.Text_Name);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(1.0f, 0.3f).From(0.0f).SetEase(Ease.OutBack).SetDelay(0.1f));
    }
    #endregion
    #region Close
    private Sequence BuildingStatus_Close()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(0.0f, 0.3f));
    }

    private Sequence Frame_Close()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(0.0f, 0.3f));
    }
    #endregion

    private enum Children
    {
        Frame,
        Icon_Building,
        Text_Name,
        Icon_Rotate,
        Fill,
        Text_Value
    }

    private BuildingObject buildingObject;
    private Image fill;
    private TMP_Text textValue;

    private Sequence opened;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, BuildingStatus_Open, Frame_Open);
        BindSequences(UIState.Open, Icon_Open, Text_Name_Open);
        BindSequences(UIState.Close, BuildingStatus_Close, Frame_Close);

        buildingObject = transform.parent.GetComponent<BuildingObject>();
        fill = Get<Image>((int)Children.Fill);
        textValue = Get<TMP_Text>((int)Children.Text_Value);
        opened = DOTween.Sequence().SetLoops(-1)
            .Append(Get((int)Children.Icon_Rotate).DORotate(360.0f * Vector3.back, 1.0f).SetEase(Ease.Linear).SetRelative(true));

        gameObject.SetActive(false);
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
        opened.Kill();
    }

    public override void Open()
    {
        base.Open();

        int id = int.Parse(buildingObject.name[..buildingObject.name.IndexOf('_')]);
        BuildingData data = Managers.Data.GetData<BuildingData>(DataType.Building, id);
        Get<Image>((int)Children.Icon_Building).sprite = Managers.Resource.GetSprite(SpriteType.Building, data.ID.ToString());
        Get<TMP_Text>((int)Children.Text_Name).text = data.Name;

        gameObject.SetActive(true);
        StartCoroutine(Updating(data.Timer));
    }

    private IEnumerator Updating(float time)
    {
        float timer = 0.0f;
        while (timer < time)
        {
            yield return null;

            timer += Time.deltaTime;
            fill.fillAmount = timer / time;
            textValue.text = $"{fill.fillAmount * 100.0f:F1}%";
        }

        buildingObject.Complete();
        Close();
    }
}