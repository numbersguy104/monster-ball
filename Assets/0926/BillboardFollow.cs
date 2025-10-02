using UnityEngine;

public class BillboardFollow : MonoBehaviour
{
    public Transform target;  // Target monster to follow
    public Vector3 offset = new Vector3(0, 2f, 0);

    void LateUpdate()
    {
        if (target == null) return;

        // Follow position
        //transform.position = target.position + offset;

        // Always face the camera
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
