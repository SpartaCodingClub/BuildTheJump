using DG.Tweening;
using UnityEngine;

public class GameManager
{
    public P_Movement Player { get; private set; }
    public P_Interaction Interaction { get; private set; }

    public void Initialize(P_Movement player, P_Interaction interaction)
    {
        Player = player;
        Interaction = interaction;

        Application.targetFrameRate = 60;
        DOTween.SetTweensCapacity(200, 500);
    }
}