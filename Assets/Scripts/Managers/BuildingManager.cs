using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager
{
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
    }

    private void OnCancel(InputAction.CallbackContext callbackContext)
    {
        Managers.Input.System.UI.Enable();
        Managers.Input.System.UI_Building.Disable();
        Managers.Resource.Destroy(buildingObject.gameObject);
        Managers.UI.Open_MenuUI<UI_Building>();
    }
    #endregion

    private LayerMask layerMask;

    private BuildingObject buildingObject;

    public void Initialize()
    {
        layerMask = LayerMask.GetMask(Define.LAYER_GROUND);

        Managers.Input.System.UI_Building.Build.performed += OnBuild;
        Managers.Input.System.UI_Building.Confirm.canceled += OnConfirm;
        Managers.Input.System.UI_Building.Cancel.started += OnCancel;
    }

    public void Build(BuildingData buildingData)
    {
        buildingObject = Managers.Resource.Instantiate(buildingData.name, Vector3.zero, Define.PATH_BUILDING).GetComponent<BuildingObject>();

        Managers.Input.System.UI.Disable();
        Managers.Input.System.UI_Building.Enable();
    }
}