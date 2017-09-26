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
    public int weaponLevel;
    public bool isEnabled;
}   

public class Player : MonoBehaviour 
{
    [Header("Fire Details")]
    public SimpleTouchAreaButton firePad;
    public float fireRate = 10;
    public List<Turret> StandardTurrets = new List<Turret>();

    [Header("Movement")]
    public float speed = 2.0f;
    public float rotateSpeed = 2.0f;
    public Boundary movementBounds;
    public SimpleTouchPad touchPad;

    [Header("Explosion Details")]
    public GameObject explosion;

    [Header("Other")]
    public ShieldController sheild;

    private Rigidbody2D m_rigBody;
 
    private float m_nextfile = 0.0f;
    private int m_weaponLevel = 1;
    private float m_weaponUpgradeDuration;

	// Use this for initialization
	void Start () 
	{
        m_rigBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () 
	{
        bool canFire = Input.GetButton("Fire1");

        if (canFire)
        {
            fireBullet();
        }

        if (m_weaponUpgradeDuration <= Time.time)
        {
            m_weaponLevel = 1;
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

        if (other.gameObject.tag.Contains("PowerUp"))
        {
            consumePowerUp(other.gameObject);
            ObjectUtils.cleanUpObject(other.gameObject);
        }
        else
        {
            GameController.sharedInstance.gameOver();
            ObjectUtils.cleanUpObject(other.gameObject);
            ObjectUtils.cleanUpObject(gameObject);

            ObjectUtils.createObject(explosion, gameObject.transform.position, gameObject.transform.rotation);
        }
        Debug.Log("Destorying " + gameObject.name + " with tag \"" + gameObject.tag + "\" by object with a tag \"" + other.gameObject.tag + "\"");
    }

    private void consumePowerUp(GameObject gameObject)
    {
        PowerUpEnum powerUp = gameObject.GetComponent<PowerUpEnum>();

        switch (powerUp.powerUpType)
        {
            case PowerUpEnum.PowerUpType.Weapon:
                upgradeWeapons(gameObject);
                break;
            case PowerUpEnum.PowerUpType.Shield:
                sheild.enableSheilds();
                break;
        }
    }

    private void upgradeWeapons(GameObject gameObject)
    {
        WeaponUpgrade powerUp = gameObject.GetComponent<WeaponUpgrade>();

        m_weaponLevel = powerUp.weaponLevel;

        m_weaponUpgradeDuration = Time.time + powerUp.weaponUpgradeDurationInSeconds;
    }

    private void fireBullet()
    {
        if (Time.time > m_nextfile)
        {
            m_nextfile = Time.time + fireRate;

            foreach (var turret in StandardTurrets)
            {
                if (turret.weaponLevel == m_weaponLevel)
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

        //Vector3 movement = touchPad.GetDirection();

        m_rigBody.velocity = movement.normalized * speed;

        m_rigBody.position = new Vector3(
            Mathf.Clamp(m_rigBody.position.x, movementBounds.xMin, movementBounds.xMax), 
            Mathf.Clamp(m_rigBody.position.y, movementBounds.yMin, movementBounds.yMax)
            );
    }
}
