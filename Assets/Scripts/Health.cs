using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHP = 5;
    public bool isPlayer = false;           
    public float hurtInvulnTime = 0.3f;     
    private int hp;
    private bool invuln;

    [Header("Death")]
    public GameObject deathVFX;             

    private Animator anim;
    private Collider2D[] cols;

    void Awake()
    {
        hp = maxHP;
        anim = GetComponent<Animator>();
        cols = GetComponentsInChildren<Collider2D>(true);
    }

    public void TakeHit(int amount) 
    {
        if (invuln || hp <= 0) return;

        hp -= Mathf.Max(1, amount);

        if (hp > 0)
        {
            if (anim) anim.SetTrigger("Hurt");
            if (isPlayer) StartInvuln();
        }
        else
        {
            hp = 0;
            Die();
        }
    }

    private void StartInvuln()
    {
        if (!gameObject.activeInHierarchy) return;
        invuln = true;
        Invoke(nameof(EndInvuln), hurtInvulnTime);
    }

    private void EndInvuln() => invuln = false;

    private void Die()
    {
        if (anim) anim.SetBool("Dead", true);

        foreach (var c in cols) c.enabled = false;

        if (deathVFX) Instantiate(deathVFX, transform.position, Quaternion.identity);

        if (!isPlayer) Destroy(gameObject, 0.75f);

    }
}
