using UnityEngine;

public class UI_WorldSpaceHandler : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Managers.Camera.Main.transform.rotation;
    }
}