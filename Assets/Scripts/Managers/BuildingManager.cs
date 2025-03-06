using UnityEngine;

public class BuildingManager
{
    private LayerMask layerMask;

    private bool onBuild;
    private BuildingObject buildingObject;

    public void Initialize()
    {
        layerMask = Define.LAYER_GROUND;
    }

    public void Update()
    {
        if (buildingObject == null)
        {
            return;
        }

        Ray ray = Managers.Camera.Main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
        {
            Vector3 targetPosition = hitInfo.point;
            targetPosition.x = (int)targetPosition.x;
            targetPosition.z = (int)targetPosition.z;

            onBuild = true;
            buildingObject.transform.position = targetPosition;
        }
    }

    public void Build(BuildingData data)
    {
        buildingObject = Managers.Resource.Instantiate(data.ID.ToString(), Vector3.zero, Define.PATH_BUILDING).GetComponent<BuildingObject>();
    }

    private void Confirm()
    {
        if (onBuild == false)
        {
            return;
        }

        if (buildingObject.CanBuild == false)
        {
            return;
        }

        buildingObject.Confirm();

        onBuild = false;
        buildingObject = null;
    }
}