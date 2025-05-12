using UnityEngine;

public class ElectroArrowSkill : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
       // if (other.CompareTag("Enemy"))
        {
            //other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
