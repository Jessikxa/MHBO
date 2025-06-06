using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // You can replace this with explosion effects, death animations, etc.
        Destroy(gameObject);
    }
}
