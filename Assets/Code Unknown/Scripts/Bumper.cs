using UnityEngine;
using UnityEngine.InputSystem;

public class Bumper: MonoBehaviour
{
    //The velocity the bumper sets balls to on contact
    [SerializeField] float Power = 30.0f;

    Collider col;


    void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //Get the direction of the collision
            ContactPoint point = collision.GetContact(0); //Assume only 1 collision point
            Vector3 normal = point.normal;

            //Set the ball's velocity in that direction
            rb.linearVelocity = Vector3.Normalize(normal) * -Power;
        }
    }
}
