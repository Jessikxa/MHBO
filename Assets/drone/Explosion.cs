using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 50f;
    public float radius = 5f;
    public float lifetime = 0.5f; // time before the explosion prefab is destroyed

    void Start()
    {
        // Deal damage to all Damageable objects in the radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hitColliders)
        {
            Damageable damageable = hit.GetComponentInParent<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }

        // Destroy the explosion visual after a short delay
        Destroy(gameObject, lifetime);
    }

    // Optional: for visualizing explosion radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
