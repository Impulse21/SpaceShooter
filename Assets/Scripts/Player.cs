using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Boundary
{
    public float yMin;
    public float yMax;
    public float xMin;
    public float xMax;
}

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
    public Boundary movementBounds;

    [Header("Explosion Details")]
    public GameObject explosion;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boundary" || 
            other.gameObject.tag == gameObject.gameObject.tag ||
            other.gameObject.tag == "Bullet")
        {
            return;
        }

        GameController.sharedInstance.gameOver();
        ObjectUtils.cleanUpObject(other.gameObject);
        ObjectUtils.cleanUpObject(gameObject);

        ObjectUtils.createObject(explosion, gameObject.transform.position, gameObject.transform.rotation);

        Debug.Log("Destorying " + gameObject.name + " with tag \"" + gameObject.tag + "\" by object with a tag \"" + other.gameObject.tag + "\"");
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

                        GetComponent<AudioSource>().Play();
                    }
                }
            }  
        }
    }
       
    private void movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

     
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        m_rigBody.velocity = movement.normalized * speed;

        m_rigBody.position = new Vector3(
            Mathf.Clamp(m_rigBody.position.x, movementBounds.xMin, movementBounds.xMax), 
            Mathf.Clamp(m_rigBody.position.y, movementBounds.yMin, movementBounds.yMax)
            );
    }
}
