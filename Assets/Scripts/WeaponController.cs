using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour 
{
    [Header("Weapon Details")]
    public List<Turret> Turrets;
    public GameObject Bullet;

    public float delay;
    public float fireRate;

	// Use this for initialization
	void Start () 
    {
        InvokeRepeating("fireProjectile", delay, fireRate);
	}

    void fireProjectile()
    {
        foreach ( Turret turret in Turrets)
        {
            if(turret.isEnabled)
            {
                GameObject projectile = ObjectPool.SharedInstance.GetPooledObject(Bullet.tag);

                if (projectile != null)
                {
                    projectile.transform.position = turret.gameObj.transform.position;
                    projectile.transform.rotation = gameObject.transform.rotation;

                    projectile.SetActive(true);
                }
            }
        }
    }

    void update()
    {
        if (!gameObject.activeInHierarchy)
        {
            CancelInvoke();
        }
    }
}
