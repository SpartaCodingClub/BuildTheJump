public class Building_Trampoline : BuildingObject
{
    public override void InteractionEnter(bool isPlayer, int damage = 0)
    {
        base.InteractionEnter(isPlayer, damage);
        Managers.Game.Player.Jump(150.0f);
    }
}