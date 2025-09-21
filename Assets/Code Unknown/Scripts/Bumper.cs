using UnityEngine;
using UnityEngine.InputSystem;

public class Bumper: MonoBehaviour
{
    //The velocity the bumper sets balls to on contact
    const float BUMPER_POWER = 30.0f;

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
            ContactPoint point = collision.GetContact(0); //Assume only 1 collision point
            Vector3 normal = point.normal;
            rb.linearVelocity = Vector3.Normalize(normal) * -BUMPER_POWER;
            print(rb.linearVelocity);
        }
    }
}
