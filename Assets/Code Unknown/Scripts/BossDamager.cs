using UnityEngine;

public class BossDamager : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            //placeholder
            print("Boss Damaged!");
        }
    }
}
