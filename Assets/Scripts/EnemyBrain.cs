using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public EnemyAttackHitbox attackHitbox;  

    public void EnableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.EnableHitbox();
    }

    public void DisableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.DisableHitbox();
    }
}
