using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager
{
    private readonly string MESSAGE_CONFIRM = "건설이 시작되었습니다.";
    private readonly string MESSAGE_COMPLETE = "건설이 완료되었습니다.";

    #region InputSystem
    private void OnBuild(InputAction.CallbackContext callbackContext)
    {
        Vector2 readValue = callbackContext.ReadValue<Vector2>();
        Ray ray = Managers.Camera.Main.ScreenPointToRay(readValue);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
        {
            Vector3 targetPosition = hitInfo.point;
            targetPosition.x = (int)targetPosition.x;
            targetPosition.z = (int)targetPosition.z;
            buildingObject.transform.position = targetPosition;
        }
    }

    private void OnConfirm(InputAction.CallbackContext callbackContext)
    {
        if (buildingObject.CanBuild == false)
        {
            return;
        }

        buildingObject.Confirm();

        Managers.Input.System.UI.Enable();
        Managers.Input.System.UI_Building.Disable();
        Managers.UI.NavagationUI.Open_NavigationItem(id, MESSAGE_CONFIRM);
    }
    #endregion

    private LayerMask layerMask;

    private int id;
    private BuildingObject buildingObject;

    public void Initialize()
    {
        layerMask = LayerMask.GetMask(Define.LAYER_GROUND);

        Managers.Input.System.UI_Building.Build.performed += OnBuild;
        Managers.Input.System.UI_Building.Confirm.canceled += OnConfirm;
    }

    public void Build(BuildingData data)
    {
        id = data.ID;
        buildingObject = Managers.Resource.Instantiate($"{data.ID}_{data.name}", Vector3.zero, Define.PATH_BUILDING).GetComponent<BuildingObject>();

        Managers.Input.System.UI.Disable();
        Managers.Input.System.UI_Building.Enable();
    }

    public void Complete()
    {
        Managers.UI.NavagationUI.Open_NavigationItem(id, MESSAGE_COMPLETE);
    }
}