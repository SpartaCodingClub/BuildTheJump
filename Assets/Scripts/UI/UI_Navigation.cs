using UnityEngine;

public class UI_Navigation : UI_Base
{
    private enum Children
    {
        Content
    }

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));
    }

    public void Open_NavigationItem(Item dropItem)
    {
        UI_NavigationItem navigationItem = Managers.UI.Open<UI_NavigationItem>();
        navigationItem.transform.SetParent(Get((int)Children.Content));
        navigationItem.UpdateUI(dropItem);
    }
}