using DG.Tweening;
using UnityEngine;

public class GameManager
{
    public P_Rigidbody Player { get; private set; }
    public P_Interaction Interaction { get; private set; }

    public void Initialize(P_Rigidbody player, P_Interaction interaction)
    {
        Player = player;
        Interaction = interaction;

        Application.targetFrameRate = 60;
        DOTween.SetTweensCapacity(200, 500);
    }
}