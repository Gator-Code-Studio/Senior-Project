using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float grappleLength = 20f;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;

    [Header("Swinging")]
    [SerializeField] private float grappleSpeed = 10f; // Speed of the initial pull
    [Tooltip("How much the rope shortens to start the swing. 1 = no pull, 0.5 = 50% pull.")]
    [SerializeField] private float swingPullFactor = 0.8f;
    [SerializeField] private float swingForce = 50f; // Force to influence the swing

    private Vector3 grapplePoint;
    private DistanceJoint2D joint;
    private Rigidbody2D rb;

    private bool isSwinging = false;
    private float targetJointDistance;

    void Start()
    {
        joint = gameObject.GetComponent<DistanceJoint2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        joint.enabled = false;
        rope.enabled = false;
    }

    void Update()
    {
        // Start Swing
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                origin: Camera.main.ScreenToWorldPoint(Input.mousePosition),
                direction: Vector2.zero,
                distance: Mathf.Infinity,
                layerMask: grappleLayer);

            if (hit.collider != null && Vector2.Distance(transform.position, hit.point) <= grappleLength)
            {
                grapplePoint = hit.point;
                grapplePoint.z = 0;
                joint.connectedAnchor = grapplePoint;
                joint.enabled = true;

                float initialDistance = Vector2.Distance(transform.position, grapplePoint);
                joint.distance = initialDistance;

                // Calculate the target distance for the initial pull
                targetJointDistance = initialDistance * swingPullFactor;

                rope.SetPosition(0, grapplePoint);
                rope.SetPosition(1, transform.position);
                rope.enabled = true;
                isSwinging = true;
            }
        }

        // Stop Swing
        if (Input.GetMouseButtonUp(0))
        {
            joint.enabled = false;
            rope.enabled = false;
            isSwinging = false;
        }

        // Update logic while swinging
        if (isSwinging)
        {
            rope.SetPosition(1, transform.position);

            // This performs the initial pull-in
            if (joint.distance > targetJointDistance)
            {
                joint.distance = Mathf.MoveTowards(joint.distance, targetJointDistance, grappleSpeed * Time.deltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        // Apply horizontal force to influence the swing
        if (isSwinging)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            rb.AddForce(new Vector2(horizontalInput * swingForce, 0));
        }
    }
}