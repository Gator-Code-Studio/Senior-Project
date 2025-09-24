using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float grappleLength;
    [SerializeField] private float grappleSpeed = 10f; // Speed at which player is reeled in
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;

    private Vector3 grapplePoint;
    private DistanceJoint2D joint;
    private bool isGrappling = false; // State tracker

    void Start()
    {
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        rope.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                origin: Camera.main.ScreenToWorldPoint(Input.mousePosition),
                direction: Vector2.zero,
                distance: Mathf.Infinity,
                layerMask: grappleLayer);

            if (hit.collider != null)
            {
                grapplePoint = hit.point;
                grapplePoint.z = 0;
                joint.connectedAnchor = grapplePoint;
                joint.enabled = true;

                // --- MODIFICATION START ---
                // Calculate the starting distance instead of using a fixed length
                float distance = Vector2.Distance(transform.position, grapplePoint);
                joint.distance = distance;
                // --- MODIFICATION END ---

                rope.SetPosition(0, grapplePoint);
                rope.SetPosition(1, transform.position);
                rope.enabled = true;
                isGrappling = true; // Set state to true
            }
        }

        // Stop Grapple
        if (Input.GetMouseButtonUp(0))
        {
            joint.enabled = false;
            rope.enabled = false;
            isGrappling = false; // Set state to false
        }

        // Update Rope and Joint while grappling
        if (isGrappling)
        {
            rope.SetPosition(1, transform.position);
            
            joint.distance = Mathf.MoveTowards(joint.distance, 1f, grappleSpeed * Time.deltaTime);

        }
    }
}