using UnityEngine;

public class BillboardFollow : MonoBehaviour
{
    public Transform target;  // 要跟随的怪物
    public Vector3 offset = new Vector3(0, 2f, 0);

    void LateUpdate()
    {
        if (target == null) return;
        
        // 跟随位置
        //transform.position = target.position + offset;

        // 始终面向相机
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
