using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Minimap : UI_Base
{
    private enum Children
    {
        Slider,
        Content,
        Text_South,
        Text_West,
        Text_North,
        Text_East
    }

    private readonly Dictionary<Transform, UI_MinimapItem> Items = new();

    private float offsetY;
    private float sliderWidth;
    private TMP_Text textSouth;
    private TMP_Text textWest;
    private TMP_Text textNorth;
    private TMP_Text textEast;
    private Transform player;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        offsetY = Managers.Camera.Main.transform.eulerAngles.y;
        sliderWidth = Get((int)Children.Slider).rect.width - 50.0f;
        textSouth = Get<TMP_Text>((int)Children.Text_South);
        textWest = Get<TMP_Text>((int)Children.Text_West);
        textNorth = Get<TMP_Text>((int)Children.Text_North);
        textEast = Get<TMP_Text>((int)Children.Text_East);
        player = Managers.Game.Player.transform;
    }


    private void Update()
    {
        UpdateContent();
        UpdateItems();
    }

    private void SetPositionX(TMP_Text text, float eulerAngle, float offsetY)
    {
        float normalizedAngle = (eulerAngle - offsetY + 360.0f) % 360.0f / 360.0f;
        float x = Mathf.Lerp(-sliderWidth, sliderWidth, normalizedAngle);
        text.rectTransform.anchoredPosition = new(x, text.rectTransform.anchoredPosition.y);

        float distance = Mathf.Abs(x / sliderWidth);
        text.color = new(1.0f, 1.0f, 1.0f, Mathf.Lerp(1.0f, 0.2f, distance));
        text.transform.localScale = Mathf.Lerp(1.2f, 0.8f, distance) * Vector3.one;
    }

    public void AddItem(Transform target, SpriteType type, string id)
    {
        UI_MinimapItem minimapItem = Managers.UI.Open<UI_MinimapItem>();
        minimapItem.Initialize(target, Get((int)Children.Content), type, id);
        Items.Add(target, minimapItem);
    }

    private void UpdateContent()
    {
        float eulerAngle = (player.eulerAngles.y - offsetY + 360.0f) % 360.0f;
        SetPositionX(textSouth, eulerAngle, 0.0f);
        SetPositionX(textWest, eulerAngle, 90.0f);
        SetPositionX(textNorth, eulerAngle, 180.0f);
        SetPositionX(textEast, eulerAngle, 270.0f);
    }

    private void UpdateItems()
    {
        foreach (var item in Items)
        {
            UI_MinimapItem minimapItem = item.Value;
            if (minimapItem.Target == null)
            {
                Managers.Resource.Destroy(minimapItem.gameObject);
                Items.Remove(item.Key);
                continue;
            }

            Vector3 direction = minimapItem.Target.position - player.position;
            float targetAngle = Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg;
            float normalizedAngle = (player.eulerAngles.y - targetAngle + 360.0f) % 360.0f / 360.0f;
            float x = Mathf.Lerp(-sliderWidth, sliderWidth, normalizedAngle);
            minimapItem.UpdateUI(x, direction.magnitude);
        }
    }
}