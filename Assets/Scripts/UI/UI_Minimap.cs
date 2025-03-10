using TMPro;
using UnityEngine;

public class UI_Minimap : UI_Base
{
    private enum Children
    {
        Slider,
        Text_South,
        Text_West,
        Text_North,
        Text_East
    }

    private float offsetY;
    private float sliderWidth;
    private TMP_Text textSouth;
    private TMP_Text textWest;
    private TMP_Text textNorth;
    private TMP_Text textEast;
    private Transform target;

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
        target = Managers.Game.Player.transform;
    }


    private void Update()
    {
        float eulerAngles = (target.eulerAngles.y - offsetY + 360.0f) % 360.0f;
        SetPositionX(textSouth, eulerAngles, 0.0f);
        SetPositionX(textWest, eulerAngles, 90.0f);
        SetPositionX(textNorth, eulerAngles, 180.0f);
        SetPositionX(textEast, eulerAngles, 270.0f);
    }

    private void SetPositionX(TMP_Text text, float eulerAngles, float offsetY)
    {
        float normalizedAngle = (eulerAngles - offsetY + 360.0f) % 360.0f / 360.0f;
        float x = Mathf.Lerp(-sliderWidth, sliderWidth, normalizedAngle);
        text.rectTransform.anchoredPosition = new(x, text.rectTransform.anchoredPosition.y);

        float distance = Mathf.Abs(x / sliderWidth);
        text.color = new(1.0f, 1.0f, 1.0f, Mathf.Lerp(1.0f, 0.2f, distance));
        text.rectTransform.localScale = Mathf.Lerp(1.2f, 0.8f, distance) * Vector3.one;
    }
}