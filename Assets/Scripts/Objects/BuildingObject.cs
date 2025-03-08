using DG.Tweening;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    private readonly string TRANSPARENT = "Transparent";

    public bool CanBuild { get; private set; }

    private Vector3 originalScale;
    private Material originalMaterial;
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;
    private Transform effect;
    private UI_BuildingStatus buildingStatusUI;

    private bool confirm;
    private int layer;

    private void Awake()
    {
        meshCollider = GetComponentInChildren<MeshCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        originalMaterial = meshRenderer.material;
        effect = gameObject.FindComponent<Transform>(Define.EFFECT);
        originalScale = effect.localScale;
        buildingStatusUI = gameObject.FindComponent<UI_BuildingStatus>();

        meshCollider.isTrigger = true;
        layer = LayerMask.NameToLayer(Define.LAYER_GROUND);

        meshRenderer.material = Resources.Load<Material>($"{Define.PATH_MATERIAL}/{TRANSPARENT}");
    }

    private void OnTriggerStay(Collider other)
    {
        if (confirm)
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
        if (confirm)
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
        confirm = true;

        Managers.Resource.Instantiate(Define.EFFECT_BUILD, transform.position, Define.PATH_EFFECT);
    }

    public void Complete()
    {
        meshRenderer.material = originalMaterial;

        DOTween.Sequence()
            .Append(meshRenderer.material.DOColor(Color.white * 5.0f, Define.EMISSION_COLOR, 1.0f))
            .Append(meshRenderer.material.DOColor(Color.black, Define.EMISSION_COLOR, 1.0f));

        effect.gameObject.SetActive(true);
        effect.DOScale(originalScale, 2.0f).From(0.0f);
    }
}