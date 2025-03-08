public class Building_Portal : BuildingObject
{
    public override void OnInteraction(int damage)
    {
        base.OnInteraction(damage);
        Managers.UI.Open<UI_Portal>();
    }
}