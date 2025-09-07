using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    // where shuriken spawns from
    public Transform handAnchor;            
    public GameObject shurikenPrefab;       
    public GameObject katanaSlashPrefab;
    public PlayerMovement movement;

    [Header("Settings")]

    // offset from player’s hand
    public float shurikenSpawnOffset = 0.4f;  

    // Update is called once per frame
    void Update()
    {
        // Throw shuriken when pressing F
        if (Input.GetKeyDown(KeyCode.F))
        {
            ThrowShuriken();
        }

        // Katana slash when pressing J
        if (Input.GetKeyDown(KeyCode.J))
        {
            SlashKatana();
        }
    }
    private void ThrowShuriken()
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

    private void SlashKatana()
    {
        if (katanaSlashPrefab == null) return;

        bool right = (movement == null || movement.IsFacingRight());

        Vector3 spawnPos = handAnchor != null ? handAnchor.position : transform.position;
        GameObject go = Instantiate(katanaSlashPrefab, spawnPos, Quaternion.identity, transform);

        // If facing left, flip
        if (!right)
        {
            Vector3 scale = go.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -1f;
            go.transform.localScale = scale;
        }
    }
}
