using UnityEngine;

public class Building_Portal : BuildingObject
{
    public override void OnInteraction(int damage)
    {
        base.OnInteraction(damage);
        Managers.UI.Open_MenuUI<UI_Portal>().Portal = this;
    }

    public void Summon(UnitData data)
    {
        meshRenderer.gameObject.layer = LayerMask.NameToLayer(Define.LAYER_DEFAULT);
        buildingStatusUI.UpdateUI_Summon(data);
    }

    public void Summoned(UnitData data)
    {
        meshRenderer.gameObject.layer = LayerMask.NameToLayer(Define.LAYER_OBJECT);
        Managers.Resource.Instantiate($"{data.ID}_{data.name}", transform.position, Define.PATH_UNIT);
    }
}