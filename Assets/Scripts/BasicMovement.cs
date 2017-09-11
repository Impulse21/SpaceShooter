using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour 
{
    public float speed = 10;

    void OnEnable()
    {
        Rigidbody2D rigBody = GetComponent<Rigidbody2D>();

        if (rigBody != null)
        {
            rigBody.velocity = transform.up * speed;
        }
    }
    
}
