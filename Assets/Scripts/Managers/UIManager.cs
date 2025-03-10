using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIManager
{
    #region InputSystem
    private void Open_MenuUI<T>(InputAction.CallbackContext callbackContext) where T : UI_Base
    {
        if (Close_MenuUI<T>())
        {
            return;
        }

        CurrentMenuUI = Open<T>();
    }

    private void Close_MenuUI(InputAction.CallbackContext callbackContext)
    {
        if (CurrentMenuUI == null)
        {
            return;
        }

        CurrentMenuUI.Close();
        CurrentMenuUI = null;
    }
    #endregion

    public UI_Base CurrentMenuUI { get; private set; }
    public UI_Minimap MinimapUI { get; private set; }
    public UI_Navigation NavagationUI { get; private set; }
    public UI_Popup PopupUI { get; private set; }

    private Transform Transform;

    public void Initialize()
    {
        Transform = new GameObject(nameof(UIManager), typeof(InputSystemUIInputModule)).transform;
        Transform.SetParent(Managers.Instance.transform);

        MinimapUI = Open<UI_Minimap>();
        NavagationUI = Open<UI_Navigation>();
        PopupUI = Open<UI_Popup>();

        Managers.Input.System.UI.UI_Inventory.started += Open_MenuUI<UI_Inventory>;
        Managers.Input.System.UI.UI_Building.started += Open_MenuUI<UI_Building>;
        Managers.Input.System.UI.Close_MenuUI.started += Close_MenuUI;
    }

    public T Open<T>() where T : UI_Base
    {
        string key = typeof(T).Name;
        GameObject gameObject = Managers.Resource.Instantiate(key, Vector3.zero, Define.PATH_UI);
        gameObject.transform.SetParent(Transform);

        T @base = gameObject.GetComponent<T>();
        @base.Open();
        return @base;
    }

    public T Open_MenuUI<T>() where T : UI_Base
    {
        Open_MenuUI<T>(new());
        return CurrentMenuUI as T;
    }

    public bool Close_MenuUI<T>() where T : UI_Base
    {
        if (CurrentMenuUI == null)
        {
            return false;
        }

        UI_Base menuUI = CurrentMenuUI;
        menuUI.Close();

        CurrentMenuUI = null;
        return menuUI is T;
    }

    public void UpdateMenuUI()
    {
        switch (CurrentMenuUI)
        {
            case var @base when CurrentMenuUI is UI_Inventory:
                (@base as UI_Inventory).UpdateUI();
                break;
            case var @base when CurrentMenuUI is UI_Building:
                (@base as UI_Building).UpdateUI();
                break;
            case var @base when CurrentMenuUI is UI_Portal:
                (@base as UI_Portal).UpdateUI();
                break;
        }
    }
}