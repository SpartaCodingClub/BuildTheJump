using UnityEngine;

public class AudioSourceHandler : MonoBehaviour
{
    private float originalVolume;
    private AudioSource audioSource;
    private Transform player;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        originalVolume = audioSource.volume;
        player = Managers.Game.Player.transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        audioSource.volume = Mathf.Clamp01(1 - (distance / 20.0f)) * originalVolume;
        audioSource.panStereo = Mathf.Clamp((player.position.x - transform.position.x) * 0.1f, -1f, 1f);
    }
}