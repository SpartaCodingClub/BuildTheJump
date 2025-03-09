using TMPro;
using UnityEngine;

public class UI_Compass : UI_Base
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
        sliderWidth = Get((int)Children.Slider).rect.width;
        textSouth = Get<TMP_Text>((int)Children.Text_South);
        textWest = Get<TMP_Text>((int)Children.Text_West);
        textNorth = Get<TMP_Text>((int)Children.Text_North);
        textEast = Get<TMP_Text>((int)Children.Text_East);
        target = Managers.Game.Player.transform;
    }


    private void Update()
    {
        UpdateCompass();
    }

    private void UpdateCompass()
    {
        float heading = target.eulerAngles.y - offsetY;
        SetPositions(textSouth, heading, 0.0f);
        SetPositions(textWest, heading, 90.0f);
        SetPositions(textNorth, heading, 180.0f);
        SetPositions(textEast, heading, 270.0f);
    }

    private void SetPositions(TMP_Text text, float heading, float offset)
    {
        // 기준 각도에 따라 각 텍스트가 중앙으로 이동하도록 계산
        float relativeAngle = (heading - offset + 360.0f) % 360.0f; // 각도 보정
        float normalizedAngle = relativeAngle / 360.0f; // 0~1 사이의 값으로 정규화
        float xPosition = Mathf.Lerp(-sliderWidth, sliderWidth, normalizedAngle);
        text.rectTransform.anchoredPosition = new(xPosition, text.rectTransform.anchoredPosition.y);

        // 중심에서 거리 계산 (0이 중앙, 1이 최대 거리)
        float distanceFromCenter = Mathf.Abs(xPosition / sliderWidth);
    }
}