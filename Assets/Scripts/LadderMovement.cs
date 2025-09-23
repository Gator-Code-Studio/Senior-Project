using UnityEngine;

public class LadderMovement : MonoBehaviour
{
   /* [SerializeField] private float speed = 2f;
    private bool isLadder;

    public bool IsClimbing { get; private set; }
    public float VerticalInput { get; private set; }

    void Update()
    {
        VerticalInput = Input.GetAxisRaw("Vertical");
        IsClimbing = isLadder && Mathf.Abs(VerticalInput) > 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            IsClimbing = false;
            VerticalInput = 0f;
        }
    }*/
}