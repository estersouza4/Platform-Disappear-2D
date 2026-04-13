using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offsetX = 4f;

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.position.x + offsetX,
            transform.position.y,
            transform.position.z
        );
    }
}