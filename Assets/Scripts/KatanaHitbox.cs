using System.Collections.Generic;
using UnityEngine;

public class KatanaHitbox : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 2;

    [Header("Ownership")]
    public string ownerTag = "Player";   

    private Collider2D col;
    private readonly HashSet<Collider2D> hitThisSwing = new HashSet<Collider2D>();

    void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false; 
    }


    public void EnableHitbox()
    {
        hitThisSwing.Clear();
        if (col != null) col.enabled = true;
    }


    public void DisableHitbox()
    {
        if (col != null) col.enabled = false;
        hitThisSwing.Clear();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled || col == null) return;

        if (other.CompareTag(ownerTag)) return;

        if (hitThisSwing.Contains(other)) return;
        hitThisSwing.Add(other);

        var dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeHit(damage);

        }
    }
}
