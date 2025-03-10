using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CameraManager
{
    // SHAKE
    private readonly float SHAKE_AMOUNT = 0.1f;
    private readonly float SHAKE_DURATION = 0.3f;

    // OFFSET
    private readonly float X = 10.0f;
    private readonly float Y = 15.0f;
    private readonly float Z = -1.0f;

    public Camera Main { get; private set; }
    public Transform Transform { get; private set; }

    private Transform target;

    private bool shaking;
    private Vector3 originalPosition;

    public void Initialize()
    {
        Main = Camera.main;
        Transform = Main.transform;
        target = Managers.Game.Player.transform;

        Vector3 endValue = new(55.0f, Transform.eulerAngles.y, Transform.eulerAngles.z);
        Transform.DORotate(endValue, 1.0f);
    }

    public void FixedUpdate()
    {
        if (shaking)
        {
            return;
        }

        Vector3 targetPosition = new(target.position.x + X, target.position.y + Y, target.position.z + Z);
        Transform.position = Vector3.Lerp(Transform.position, targetPosition, 2.0f * Time.deltaTime);
    }

    public void Shake()
    {
        if (shaking)
        {
            return;
        }

        shaking = true;
        originalPosition = Transform.position;

        Managers.Instance.StartCoroutine(Shaking());
    }

    private IEnumerator Shaking()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < SHAKE_DURATION * 0.5f)
        {
            Transform.localPosition = originalPosition + Random.insideUnitSphere * SHAKE_AMOUNT;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Transform.localPosition = originalPosition;
        shaking = false;
    }
}