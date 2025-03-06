using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        var main = _particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnParticleSystemStopped()
    {
        Managers.Resource.Destroy(gameObject);
    }

    public void Play(MeshRenderer meshRenderer)
    {
        var shape = _particleSystem.shape;
        shape.meshRenderer = meshRenderer;

        _particleSystem.Play();
    }
}