using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildingStatus : UI_Base
{
    private static readonly string MESSAGE_CONFIRM = "건설이 시작되었습니다.";
    private static readonly string MESSAGE_COMPLETE = "건설이 완료되었습니다.";

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

    private string id;
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

        buildingObject = GetComponentInParent<BuildingObject>();
        id = buildingObject.name[..buildingObject.name.IndexOf('_')];
        fill = Get<Image>((int)Children.Fill);
        textValue = Get<TMP_Text>((int)Children.Text_Value);
        opened = DOTween.Sequence().SetLoops(-1)
            .Append(Get((int)Children.Icon_Rotate).DORotate(360.0f * Vector3.back, 1.0f).SetEase(Ease.Linear).SetRelative(true));

        GetComponent<Canvas>().worldCamera = Managers.Camera.Main;

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

        BuildingData data = Managers.Data.GetData<BuildingData>(DataType.Building, int.Parse(id));
        Get<Image>((int)Children.Icon_Building).sprite = Managers.Resource.GetSprite(SpriteType.Building, id);
        Get<TMP_Text>((int)Children.Text_Name).text = data.Name;

        gameObject.SetActive(true);
        Managers.UI.NavagationUI.Open_NavigationItem(id, MESSAGE_CONFIRM);

        StartCoroutine(Updating(data.Duration));
    }

    private IEnumerator Updating(float duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fill.fillAmount = elapsedTime / duration;
            textValue.text = $"{fill.fillAmount * 100.0f:F1}%";
            yield return null;
        }

        buildingObject.Complete();
        Managers.UI.NavagationUI.Open_NavigationItem(id, MESSAGE_COMPLETE);

        Close();
    }
}