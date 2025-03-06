using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UIManager
{
    public UI_Base CurrentMenuUI { get; private set; }
    public UI_Navigation NavagationUI { get; private set; }
    public UI_Popup PopupUI { get; private set; }

    private Transform Transform;
    

    public void Initialize()
    {
        Transform = new GameObject(nameof(UIManager), typeof(InputSystemUIInputModule)).transform;
        Transform.SetParent(Managers.Instance.transform);

        //NavagationUI = Open<UI_Navigation>();
        //PopupUI = Open<UI_Popup>();
    }

    public void Update()
    {
        
        

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    Open_MenuUI<UI_Inventory>();
        //}
        //else if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Open_MenuUI<UI_Building>();
        //}
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

    private void Open_MenuUI<T>() where T : UI_Base
    {
        if (CurrentMenuUI != null)
        {
            CurrentMenuUI.Close();
        }

        if (CurrentMenuUI is T)
        {
            CurrentMenuUI = null;
            return;
        }

        CurrentMenuUI = Open<T>();
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
        }
    }
}