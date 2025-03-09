using DG.Tweening;
using TMPro;
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
        Managers.UI.Close_MenuUI<UI_Building>();
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

    private UI_Subitem[] content;

    private bool canBuild;
    private BuildingData data;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, Frame_Open);
        BindSequences(UIState.Close, Frame_Close);

        content = Get((int)Children.Content).GetComponentsInChildren<UI_Subitem>();
        foreach (var subItem in content)
        {
            subItem.gameObject.SetActive(false);
        }

        Get<Button>((int)Children.Button_Build).onClick.AddListener(Button_Build);
    }

    public void UpdateUI(BaseData data)
    {
        ID = data.ID;
        this.data = data as BuildingData;

        UpdateUI();
        Get<TMP_Text>((int)Children.Text_Timer).text = Utility.GetTimer(this.data.Duration);
    }

    public void UpdateUI()
    {
        foreach (var subItem in content)
        {
            subItem.gameObject.SetActive(false);
        }

        canBuild = true;
        for (int i = 0; i < data.Items.Count; i++)
        {
            UI_Subitem subitem = content[i];
            if (subitem.UpdateUI(data.Items[i]) == false)
            {
                canBuild = false;
            }

            subitem.gameObject.SetActive(true);
        }
    }
}