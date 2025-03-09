using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UI_Portal : UI_Base
{
    #region Open
    private Sequence Background_Open()
    {
        var graphic = Get<Graphic>((int)Children.Background);

        return Utility.RecyclableSequence()
            .Append(graphic.DOFade(0.8f, 0.3f).From(0.0f));
    }

    private Sequence Frame_Open()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(1.0f, 0.3f).From(0.0f).SetEase(Ease.OutBack));
    }

    private Sequence Open_Content_Right()
    {
        var child = Get((int)Children.Content_Right);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(1.0f, 0.2f).From(0.0f).SetEase(Ease.OutBack));
    }
    #endregion
    #region Close
    private Sequence Background_Close()
    {
        var graphic = Get<Graphic>((int)Children.Background);

        return Utility.RecyclableSequence()
            .Append(graphic.DOFade(0.0f, 0.3f));
    }

    private Sequence Frame_Close()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(0.0f, 0.3f).SetEase(Ease.InBack));
    }
    #endregion
    #region Events
    private void Button_Summon()
    {
        if (canSummon == false)
        {
            return;
        }

        // TODO: 추후 재료 삭제
        Portal.Summon(data);
        Managers.UI.Close_MenuUI<UI_Portal>();
    }
    #endregion

    private enum Children
    {
        Background,
        Frame,
        Button_Close,
        Content_Left,
        Icon_Unit,
        Text_DescriptionUnit,
        Content_Right,
        Button_Summon
    }

    public int ID { get; private set; }
    public Building_Portal Portal { get; set; }

    private UI_PortalUnit[] contentLeft;
    private UI_Subitem[] contentRight;

    private bool canSummon;
    private UnitData data;

    private Sequence openContentRight;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, Background_Open, Frame_Open);
        BindSequences(UIState.Close, Background_Close, Frame_Close);

        contentLeft = Get((int)Children.Content_Left).GetComponentsInChildren<UI_PortalUnit>();
        contentRight = Get((int)Children.Content_Right).GetComponentsInChildren<UI_Subitem>();
        openContentRight = Open_Content_Right();

        Get<Button>((int)Children.Button_Close).onClick.AddListener(() => Managers.UI.Close_MenuUI<UI_Portal>());
        Get<Button>((int)Children.Button_Summon).onClick.AddListener(Button_Summon);

        UpdateUI(Managers.Data.GetData<UnitData>(DataType.Unit, 10001));
        UpdateUI();
    }

    private void Start()
    {
        int index = 0;
        foreach (var data in Managers.Data.GetDatas(DataType.Unit))
        {
            contentLeft[index++].UpdateUI(data);
        }
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
        openContentRight.Kill();
    }

    public override void Open()
    {
        base.Open();
        Managers.Input.System.Player.Disable();
    }

    public override void Close()
    {
        base.Close();
        Managers.Input.System.Player.Enable();
    }

    public void UpdateUI()
    {
        foreach (var sumItem in contentRight)
        {
            sumItem.gameObject.SetActive(false);
        }

        canSummon = true;
        for (int i = 0; i < data.Items.Count; i++)
        {
            UI_Subitem subitem = contentRight[i];
            if (subitem.UpdateUI(data.Items[i]) == false)
            {
                canSummon = false;
            }

            subitem.gameObject.SetActive(true);
        }
    }

    public void UpdateUI(UnitData data)
    {
        ID = data.ID;
        this.data = data;

        Get<Image>((int)Children.Icon_Unit).sprite = Managers.Resource.GetSprite(SpriteType.Unit, data.ID.ToString());
        Get<TMP_Text>((int)Children.Text_DescriptionUnit).text = data.DescriptionUnit;

        openContentRight.Restart();
    }
}