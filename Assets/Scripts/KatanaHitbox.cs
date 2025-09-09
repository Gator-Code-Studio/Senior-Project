using UnityEngine;

public class KatanaHitbox : MonoBehaviour
{
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false; // disabled by default
    }

    public void EnableHitbox()
    {
        col.enabled = true;
    }

    public void DisableHitbox()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            // damage logic here
            var dmg = other.GetComponent<IDamageable>();
            if (dmg != null) dmg.TakeHit(2);
        }
    }
}
