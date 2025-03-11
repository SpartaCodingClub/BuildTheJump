using UnityEngine;

public class Building_Bonfire : BuildingObject
{
    private static readonly float INTERVAL = 2.0f;

    private bool interaction;
    private float timer;

    private void Update()
    {
        if (interaction == false)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer > INTERVAL)
        {
            timer -= INTERVAL;

            Managers.Game.SetHP(10);
            Managers.Game.SetSP(10);
        }
    }

    public override void InteractionEnter(int damage = 0)
    {
        base.InteractionEnter(damage);
        interaction = true;
    }

    public override void InteractionExit(bool isPlayer)
    {
        base.InteractionExit(isPlayer);

        interaction = false;
        timer = 0.0f;
    }
}