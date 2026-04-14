using UnityEngine;

public class IF_Projectile : MonoBehaviour
{
    [Tooltip("This is how long the object will take to despawn")]
    [SerializeField] float despawnTime = 10f; // This is how long the object will take to despawn;

    [Tooltip("This is the damage that the projectile will deal to the enemy it hits")]
    [SerializeField] int damage; // This is the damage that the projectile will deal to the enemy it hits

    private void Start()
    {
        Destroy(this.gameObject, despawnTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IF_EnemyController ifEnemy = other.transform.GetComponent<IF_EnemyController>();

            ifEnemy.TakeDamage(damage);
        }
    }
}
