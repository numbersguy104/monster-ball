using UnityEngine;
//�������������Ԥ������������ Spawn ʱ������ȷ��ײ��
public class CollisionSelector : MonoBehaviour
{
    // ����԰�����Collider�ֱ�ҵ��������ֶ�
    public Collider withCollision;      // ������ײ�壨���� BoxCollider��
    public Collider withoutCollision;   // ����ײ�������� trigger ����ֱ�ӽ��ã�

    // ��� monster prefab ������� collider��������������չΪ���鲢��������/ͣ
    public void SetCollisionType(MonsterCollisionType type)
    {
        if (withCollision != null) withCollision.enabled = (type == MonsterCollisionType.WithCollision);
        if (withoutCollision != null) withoutCollision.enabled = (type == MonsterCollisionType.WithoutCollision);
    }
}
