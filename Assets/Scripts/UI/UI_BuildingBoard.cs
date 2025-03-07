using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildingBoard : UI_Base
{
    #region Open
    private Sequence Frame_Open()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOAnchorPosX(500.0f, 0.3f).From().SetEase(Ease.OutBack));
    }
    #endregion
    #region Close
    private Sequence Frame_Close()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOAnchorPosX(500.0f, 0.3f).SetEase(Ease.OutExpo));
    }
    #endregion
    #region Events
    private void Button_Build()
    {
        if (canBuild == false)
        {
            return;
        }

        // TODO: 추후 재료 삭제
        Managers.Building.Build(data);
        (Managers.UI.CurrentMenuUI as UI_Building).Close();
    }
    #endregion

    private enum Children
    {
        Frame,
        Content,
        Text_Timer,
        Button_Build
    }

    public int ID { get; private set; }

    private readonly List<UI_BuildingBoardItem> content = new();

    private bool canBuild;
    private BuildingData data;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, Frame_Open);
        BindSequences(UIState.Close, Frame_Close);

        RectTransform content = Get((int)Children.Content);
        for (int i = 0; i < content.childCount; i++)
        {
            UI_BuildingBoardItem buildingBoardItem = content.GetChild(i).GetComponent<UI_BuildingBoardItem>();
            buildingBoardItem.gameObject.SetActive(false);
            this.content.Add(buildingBoardItem);
        }

        Get<Button>((int)Children.Button_Build).onClick.AddListener(Button_Build);
    }

    public void UpdateUI(BaseData data)
    {
        ID = data.ID;
        this.data = data as BuildingData;

        Get<TMP_Text>((int)Children.Text_Timer).text = Utility.GetTimer(this.data.Duration);
        UpdateUI();
    }

    public void UpdateUI()
    {
        canBuild = true;
        for (int i = 0; i < data.Items.Count; i++)
        {
            if (content[i].UpdateUI(data.Items[i]) == false)
            {
                canBuild = false;
            }

            content[i].gameObject.SetActive(true);
        }
    }
}