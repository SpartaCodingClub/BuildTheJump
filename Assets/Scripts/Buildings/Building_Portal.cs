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
        P_Worker worker = Managers.Resource.Instantiate(data.name, transform.position + Vector3.left, Define.PATH_UNIT).GetComponent<P_Worker>();
        worker.SetDestination(transform.position + 4.0f * Vector3.right, () => worker.SetState(WorkerState.Idle));
    }
}