using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomExplosionSound : MonoBehaviour
{
    public AudioClip[] explosionClips;
    public AudioSource audioSource;

    void Start()
    {
        if (explosionClips.Length == 0 || audioSource == null)
        {
            Debug.LogWarning("Explosion sound clips or AudioSource not assigned!");
            return;
        }

        int randomIndex = Random.Range(0, explosionClips.Length);
        AudioClip selectedClip = explosionClips[randomIndex];

        audioSource.clip = selectedClip;
        audioSource.Play();
    }
}
