using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float _time = 0;
    private float _speed = 0.2f;
    
    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime * _speed;
        gameObject.transform.rotation = Quaternion.Euler(0, (float)Math.Sin(_time) * 20, 0);
    }
}
