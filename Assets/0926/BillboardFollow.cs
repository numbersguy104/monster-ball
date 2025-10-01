using UnityEngine;

public class BillboardFollow : MonoBehaviour
{
    public Transform target;  // Ҫ����Ĺ���
    public Vector3 offset = new Vector3(0, 2f, 0);

    void LateUpdate()
    {
        if (target == null) return;
        
        // ����λ��
        //transform.position = target.position + offset;

        // ʼ���������
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
