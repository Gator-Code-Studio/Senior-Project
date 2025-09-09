using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    // where shuriken spawns from
    public Transform handAnchor;            
    public GameObject shurikenPrefab;       
    public PlayerMovement movement;
    public KatanaHitbox katanaHitbox;

    [Header("Settings")]

    // offset from player’s hand
    public float shurikenSpawnOffset = 0.4f;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Throw shuriken when pressing F
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (anim != null)
            {
                anim.SetTrigger("Throw");
            }
        }

        // Reset speed back to normal when Throw is not playing
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            anim.speed = 1f;
        }

        // Katana slash when pressing J
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (anim != null) anim.SetTrigger("Slash");
        }
    }
    private void SpawnShuriken()
    {
        if (shurikenPrefab == null) return;

        // Get facing direction from PlayerMovement
        int dir = (movement == null || movement.IsFacingRight()) ? 1 : -1;

        Vector3 spawnPos = handAnchor != null ? handAnchor.position : transform.position;
        spawnPos += new Vector3(shurikenSpawnOffset * dir, 0f, 0f);

        GameObject go = Instantiate(shurikenPrefab, spawnPos, Quaternion.identity);

        Shuriken s = go.GetComponent<Shuriken>();
        if (s != null) s.Fire(dir);

        if (dir == -1)
        {
            Vector3 scale = go.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -1f;
            go.transform.localScale = scale;
        }
    }

    public void EnableHitbox()
    {
        if (katanaHitbox != null) katanaHitbox.EnableHitbox();
    }

    public void DisableHitbox()
    {
        if (katanaHitbox != null) katanaHitbox.DisableHitbox();
    }
}
