using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    [Header("Ball Damage Settings")]
    public int baseDamage = 1; // Basicdamage基础伤害，可在 Inspector 调整 adjust
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var light = GameObject.Find("Spot Light");
        if (light != null)
        {
            light.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 0.31f,
                transform.position.z - 0.1f
            );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        MonsterController monster = collision.gameObject.GetComponent<MonsterController>();
        if (monster != null)
        {
            // 使用碰撞相对速度大小作为伤害加成系数
            float accelFactor = collision.relativeVelocity.magnitude;

            monster.TakeDamage(baseDamage, accelFactor);
        }
    }
    //Set the ball's velocity to a given 3D vector
    public void SetVelocity(Vector3 v)
    {
        rb.linearVelocity = v;
    }

    //Set the ball's X, Y, and Z velocity individually
    //Accepts null for any of the 3 arguments, representing no change to velocity
    public void SetVelocity(float? x, float? y, float? z)
    {
        Vector3 vel = rb.linearVelocity;
        if (x.HasValue)
        {
            vel.x = x.Value;
        }
        if (y.HasValue)
        {
            vel.y = y.Value;
        }
        if (z.HasValue)
        {
            vel.z = z.Value;
        }
        rb.linearVelocity = vel;
    }

    //Add a velocity vector to the ball's existing velocity
    public void AddVelocity(Vector3 v)
    {
        rb.linearVelocity += v;
    }

    //Set the ball's speed (magnitude of its velocity) by scaling its velocity to match
    public void SetSpeed(float speed)
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }

    //Get the ball's velocity vector
    public Vector3 GetVelocity()
    {
        return rb.linearVelocity;
    }
}
