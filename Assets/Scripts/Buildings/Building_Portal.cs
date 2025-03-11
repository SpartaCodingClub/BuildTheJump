using UnityEngine;

public class Building_Portal : BuildingObject
{
    public override void InteractionEnter(int damage = 0)
    {
        base.InteractionEnter(damage);
        Managers.UI.Open_MenuUI<UI_Portal>().Portal = this;
    }

    public void Summon(UnitData unitData)
    {
        meshRenderer.gameObject.layer = LayerMask.NameToLayer(Define.LAYER_DEFAULT);
        buildingStatusUI.UpdateUI_Summon(unitData);
    }

    public P_Worker Complete(UnitData unitData)
    {
        meshRenderer.gameObject.layer = LayerMask.NameToLayer(Define.LAYER_OBJECT);

        P_Worker worker = Managers.Resource.Instantiate(unitData.name, transform.position + Vector3.left, Define.PATH_UNIT).GetComponent<P_Worker>();
        worker.SetDestination(transform.position + 4.0f * Vector3.right, () => worker.SetState(WorkerState.Idle));
        worker.SetStats(unitData);

        return worker;
    }
}