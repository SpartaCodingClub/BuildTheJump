using DG.Tweening;
using UnityEngine;

public enum MaterialType
{
    Opaque,
    Transparent
}

public class BuildingObject : MonoBehaviour
{
    public bool CanBuild { get; private set; }

    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;
    private Transform effect;
    private UI_BuildingStatus buildingStatusUI;

    private bool completed;
    private int layer;

    private void Awake()
    {
        meshCollider = GetComponentInChildren<MeshCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        effect = gameObject.FindComponent<Transform>(Define.EFFECT);
        buildingStatusUI = gameObject.FindComponent<UI_BuildingStatus>();

        meshCollider.isTrigger = true;
        layer = LayerMask.NameToLayer(Define.LAYER_GROUND);

        SetMaterial(MaterialType.Transparent);
    }

    private void OnTriggerStay(Collider other)
    {
        if (completed)
        {
            return;
        }

        if (other.gameObject.layer == layer)
        {
            return;
        }

        CanBuild = false;
        meshRenderer.material.SetColor(Define.EMISSION_COLOR, Define.RED);
    }

    private void OnTriggerExit(Collider other)
    {
        if (completed)
        {
            return;
        }

        CanBuild = true;
        meshRenderer.material.SetColor(Define.EMISSION_COLOR, Define.BLUE);
    }

    public void Confirm()
    {
        meshCollider.isTrigger = false;
        buildingStatusUI.Open();

        Managers.Resource.Instantiate(Define.EFFECT_BUILD, transform.position, Define.PATH_EFFECT);
    }

    public void Complete()
    {
        SetMaterial(MaterialType.Opaque);

        DOTween.Sequence()
            .Append(meshRenderer.material.DOColor(Color.white * 5.0f, Define.EMISSION_COLOR, 1.0f))
            .Append(meshRenderer.material.DOColor(Color.black, Define.EMISSION_COLOR, 1.0f));

        effect.gameObject.SetActive(true);
        effect.DOScale(0.3f, 2.0f).From(0.0f);

        completed = true;
    }

    private void SetMaterial(MaterialType type)
    {
        meshRenderer.material = Resources.Load<Material>($"{Define.PATH_MATERIAL}/{type}");
    }
}