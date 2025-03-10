public class Building_Trampoline : BuildingObject
{
    public override void InteractionEnter(int damage = 0)
    {
        base.InteractionEnter(damage);
        Managers.Game.Player.Jump(150.0f);
    }
}