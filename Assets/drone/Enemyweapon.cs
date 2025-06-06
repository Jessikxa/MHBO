using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public float fireRate = 1f;
    public float bulletSpread = 5f;
    public float damage = 10f;
    public float fireDelay = 0.5f;
    public float aimLag = 0.3f;
    public LineRenderer bulletLine;

    private EnemySight enemySight;
    private float fireCooldown = 0f;
    private AudioSource audioSource;

    void Start()
    {
        enemySight = GetComponent<EnemySight>();
        audioSource = GetComponent<AudioSource>();

        if (bulletLine != null)
        {
            bulletLine.enabled = false;
        }
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (enemySight.HasTarget && fireCooldown <= 0f)
        {
            fireCooldown = 1f / fireRate;
            Invoke(nameof(FireWeapon), fireDelay);
        }
    }

    void FireWeapon()
    {
        if (!enemySight.HasTarget) return;

        Transform target = enemySight.CurrentTarget;
        if (target == null) return;

        Vector3 predictedPosition = target.position - target.forward * aimLag;
        Vector3 direction = (predictedPosition - transform.position).normalized;

        direction += Random.insideUnitSphere * Mathf.Tan(bulletSpread * Mathf.Deg2Rad);
        direction.Normalize();

        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        if (bulletLine != null)
        {
            bulletLine.SetPosition(0, transform.position);
            bulletLine.SetPosition(1, transform.position + direction * 100f);
            StartCoroutine(ShowBulletLine());
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Apply damage or effects to the hit object here
            Debug.Log($"{gameObject.name} hit {hit.collider.name}");
        }
    }

    System.Collections.IEnumerator ShowBulletLine()
    {
        bulletLine.enabled = true;
        yield return new WaitForSeconds(0.05f);
        bulletLine.enabled = false;
    }
}
