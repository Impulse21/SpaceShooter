using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour 
{
    public float minRotationSpeed;
    public float maxRotationSpeed;

    private float rotationRate;

	// Use this for initialization
	void Start () 
    {
        rotationRate = Random.Range(minRotationSpeed, maxRotationSpeed);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Rigidbody2D rigBody = GetComponent<Rigidbody2D>();

        rigBody.rotation += rotationRate;
    }
}
