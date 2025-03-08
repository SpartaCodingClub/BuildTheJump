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
        Open_NavigationItem().UpdateUI(dropItem);
    }

    public void Open_NavigationItem(string id, string description)
    {
        Open_NavigationItem().UpdateUI(id, description);
    }

    private UI_NavigationItem Open_NavigationItem()
    {
        RectTransform content = Get((int)Children.Content);
        if (content.childCount > 8)
        {
            Managers.Resource.Destroy(content.GetChild(0).gameObject);
        }

        UI_NavigationItem navigationItem = Managers.UI.Open<UI_NavigationItem>();
        navigationItem.transform.SetParent(content);

        return navigationItem;
    }
}