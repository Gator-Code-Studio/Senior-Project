using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 3f;
    public float yOffset = 1f;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, transform.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position,newPos,Time.deltaTime * FollowSpeed);
    }
}
