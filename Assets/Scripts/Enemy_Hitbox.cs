using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    public int damage = 1;
    public string ignoreTag = "Enemy";
    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col) col.enabled = false; 
    }

    public void EnableHitbox() { if (col) col.enabled = true; }
    public void DisableHitbox() { if (col) col.enabled = false; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ignoreTag)) return;
        var d = other.GetComponent<IDamageable>();
        if (d != null) d.TakeHit(damage);
    }
}