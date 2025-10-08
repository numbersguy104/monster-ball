using UnityEngine;

public class CollisionSelector : MonoBehaviour
{
    
    public Collider withCollision;      // BoxCollider
    public Collider withoutCollision;   // trigger

   
    //if monster prefab contains mutiple collider arrange and expand here
    public void SetCollisionType(MonsterCollisionType type)
    {
        if (withCollision != null) withCollision.enabled = (type == MonsterCollisionType.WithCollision);
        if (withoutCollision != null) withoutCollision.enabled = (type == MonsterCollisionType.WithoutCollision);
    }
}
