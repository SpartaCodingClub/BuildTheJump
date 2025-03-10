using DG.Tweening;
using UnityEngine;

public enum MaterialType
{
    Opaque,
    Transparent
}

public abstract class BuildingObject : InteractableObject
{
    public bool CanBuild { get; private set; }

    protected UI_BuildingStatus buildingStatusUI;

    private Vector3 originalScale;
    private AudioSource audioSource;
    private Material originalMaterial;
    private MeshCollider meshCollider;
    private Transform effect;

    private bool confirm;
    private int layer;

    private void Start()
    {
        buildingStatusUI = gameObject.FindComponent<UI_BuildingStatus>();
        audioSource = GetComponent<AudioSource>();
        originalMaterial = meshRenderer.material;
        meshCollider = GetComponentInChildren<MeshCollider>();
        effect = gameObject.FindComponent<Transform>(Define.EFFECT);
        originalScale = effect.localScale;

        meshRenderer.material = Resources.Load<Material>($"{Define.PATH_MATERIAL}/{MaterialType.Transparent}");
        meshCollider.isTrigger = true;
        layer = LayerMask.NameToLayer(Define.LAYER_GROUND);
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

        meshRenderer.material.SetColor(Define.EMISSION_COLOR, Define.BLUE);
        CanBuild = true;
    }

    public void Confirm()
    {
        meshCollider.isTrigger = false;
        confirm = true;

        buildingStatusUI.UpdateUI_Build(data as BuildingData);
        Managers.Resource.Instantiate(Define.EFFECT_BUILD, transform.position, Define.PATH_EFFECT);
    }

    public void Complete()
    {
        meshRenderer.material = Resources.Load<Material>($"{Define.PATH_MATERIAL}/{MaterialType.Opaque}");

        DOTween.Sequence()
            .Append(meshRenderer.material.DOColor(Color.white * 5.0f, Define.EMISSION_COLOR, 1.0f))
            .Append(meshRenderer.material.DOColor(Color.black, Define.EMISSION_COLOR, 1.0f).SetEase(Ease.InSine))
            .AppendCallback(() =>
            {
                meshRenderer.material = originalMaterial;
                meshRenderer.gameObject.layer = LayerMask.NameToLayer(Define.LAYER_OBJECT);
            });

        audioSource.enabled = true;
        audioSource.DOFade(audioSource.volume, 2.0f).From(0.0f);

        effect.gameObject.SetActive(true);
        effect.DOScale(originalScale, 2.0f).From(0.0f);
    }
}