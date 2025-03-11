using DG.Tweening;
using System.Collections.Generic;

public class UI_Building : UI_Base
{
    #region Open
    private Sequence Frame_Open()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOAnchorPosY(-378.0f, 0.3f).From());
    }
    #endregion
    #region Close
    private Sequence Frame_Close()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOAnchorPosY(-378.0f, 0.3f));
    }
    #endregion

    private enum Children
    {
        Frame,
        Content
    }

    public UI_Focus FocusUI { get; private set; }

    private readonly List<UI_BuildingItem> content = new();

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, Frame_Open);
        BindSequences(UIState.Close, Frame_Close);

        FocusUI = gameObject.FindComponent<UI_Focus>();

        foreach (var buildingData in Managers.Data.GetDatas(DataType.Building))
        {
            var buildingItem = Managers.UI.Open<UI_BuildingItem>();
            buildingItem.transform.SetParent(Get((int)Children.Content));
            buildingItem.UpdateUI(buildingData);
            content.Add(buildingItem);
        }
    }

    public override void Open()
    {
        base.Open();

        foreach (var buildingItem in content)
        {
            buildingItem.Open();
        }
    }

    public override void Close()
    {
        base.Close();

        foreach (var buildingItem in content)
        {
            buildingItem.Close();
        }

        FocusUI.UpdateUI(null);
        Managers.UI.PopupUI.Close();
    }

    public void UpdateUI()
    {
        if (UI_BuildingItem.BuildingBoardUI == null)
        {
            return;
        }

        UI_BuildingItem.BuildingBoardUI.UpdateUI();
    }
}