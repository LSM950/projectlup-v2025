using LUP.RL;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private BulletData bulletData;
    private GameObject owner;
    private int damage;
    private Transform target;

    public void Init(BulletData data, GameObject Owner, int Damage, Transform Target)
    {
        bulletData = data;
        owner = Owner;
        damage = Damage;
        target = Target;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (target == null)
        {
            transform.position += transform.forward * bulletData.Speed * Time.deltaTime;
            return;
        }
      
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * bulletData.Speed * Time.deltaTime;
    }
}
