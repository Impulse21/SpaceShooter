﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Turret
{
    public GameObject gameObj;
    public bool isEnabled;
}   

public class Player : MonoBehaviour 
{
    [Header("Fire Details")]
    public float fireRate = 10;
    public List<Turret> StandardTurrets = new List<Turret>();

    [Header("Movement")]
    public float speed = 2.0f;
    public float rotateSpeed = 2.0f;

    private Rigidbody2D m_rigBody;
    private Vector2 m_touchStartPos;

    private float m_nextfile = 0.0f;

	// Use this for initialization
	void Start () 
	{
        m_rigBody = GetComponent<Rigidbody2D>();

        m_touchStartPos = new Vector2();

	}
	
	// Update is called once per frame
	void Update () 
	{
        // Temp for bug fixing
        if (Input.GetButton("Fire1"))
        {
            fireBullet();
        }
	}

    // Fixed update
    void FixedUpdate()
    {
        movement();
    }

    private void fireBullet()
    {
        if (Time.time > m_nextfile)
        {
            m_nextfile = Time.time + fireRate;

            foreach (var turret in StandardTurrets)
            {
                if (turret.isEnabled)
                {
                    GameObject bullet = ObjectPool.SharedInstance.GetPooledObject("Bullet");
                    if (bullet != null)
                    {
                        bullet.transform.position = turret.gameObj.transform.position;
                        bullet.transform.rotation = turret.gameObj.transform.rotation;

                        bullet.SetActive(true);
                    }
                }
            }  
        }
    }
       
    private void movement()
    {
        #if UNITY_IPHONE || UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
        m_touchStartPos = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
        // Get Movment of finer since last fram

        Vector2 touchDeltaPos = Input.GetTouch(0).deltaPosition;
        }
        #else
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        #endif
     
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        m_rigBody.velocity = movement.normalized * speed;
       
    }
}
