using UnityEngine;

public enum WeaponType
{
    RocketLauncher,
}

[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public WeaponType weaponType;
    public GameObject projectilePrefab;
    public Transform[] firingPoints;
    public float fireRate = 1f;
    public float projectileSpeed = 20f;
    public KeyCode fireKey = KeyCode.Space;

    [Header("Audio")]
    public AudioSource fireAudioSource;     // Dedicated audio source for this weapon
    public AudioClip[] fireClips;           // Firing sound options

    [HideInInspector]
    public float fireCooldown = 0f;

    [HideInInspector]
    public int currentFireIndex = 0;
}

public class PrimaryWeaponSystem : MonoBehaviour
{
    public WeaponData[] weapons;

    void Update()
    {
        foreach (var weapon in weapons)
        {
            weapon.fireCooldown -= Time.deltaTime;

            if (Input.GetKey(weapon.fireKey) && weapon.fireCooldown <= 0f)
            {
                FireSingleRocket(weapon);
                weapon.fireCooldown = 1f / weapon.fireRate;
            }
        }
    }

    void FireSingleRocket(WeaponData weapon)
    {
        if (weapon.firingPoints.Length == 0) return;

        Transform firePoint = weapon.firingPoints[weapon.currentFireIndex];
        GameObject rocket = Instantiate(weapon.projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = rocket.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * weapon.projectileSpeed;
        }
        else
        {
            Debug.LogWarning("Rocket prefab missing Rigidbody.");
        }

        PlayFiringSound(weapon);

        weapon.currentFireIndex = (weapon.currentFireIndex + 1) % weapon.firingPoints.Length;
    }

    void PlayFiringSound(WeaponData weapon)
    {
        if (weapon.fireAudioSource != null && weapon.fireClips.Length > 0)
        {
            int index = Random.Range(0, weapon.fireClips.Length);
            weapon.fireAudioSource.PlayOneShot(weapon.fireClips[index]);
        }
    }
}
