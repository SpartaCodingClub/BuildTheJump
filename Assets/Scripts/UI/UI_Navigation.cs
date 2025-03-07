using DG.Tweening;
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
        UI_NavigationItem navigationItem = Managers.UI.Open<UI_NavigationItem>();
        navigationItem.transform.SetParent(content);

        if (content.childCount > 9)
        {
            Transform child = content.GetChild(0);
            DOTween.Complete(child.gameObject, true);
            //Managers.Resource.Destroy(child.gameObject);
        }

        return navigationItem;
    }
}