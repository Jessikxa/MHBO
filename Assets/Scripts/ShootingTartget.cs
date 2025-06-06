using UnityEngine;

public class ShootingTartget : MonoBehaviour
{
    [SerializeField] private int HP = 1;

    void Start()
    {

    }

    
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
