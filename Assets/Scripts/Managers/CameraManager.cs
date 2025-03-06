using DG.Tweening;
using UnityEngine;

public class CameraManager
{
    private readonly float X = 10.0f;
    private readonly float Y = 15.0f;
    private readonly float Z = -1.0f;

    public Camera Main { get; private set; }
    public Transform Transform { get; private set; }

    private Transform target;

    public void Initialize()
    {
        Main = Camera.main;
        Transform = Main.transform;
        target = Managers.Game.Player.transform;

        Vector3 endValue = new(55.0f, Transform.eulerAngles.y, Transform.eulerAngles.z);
        Transform.DORotate(endValue, 1.0f);
    }

    public void LateUpdate()
    {
        Vector3 targetPosition = new(target.position.x + X, target.position.y + Y, target.position.z + Z);
        Transform.position = Vector3.Lerp(Transform.position, targetPosition, 2.0f * Time.deltaTime);
    }
}