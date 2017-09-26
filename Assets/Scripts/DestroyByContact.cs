using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour 
{
    public GameObject explosion;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boundary" || 
            other.gameObject.tag == gameObject.gameObject.tag ||
            other.gameObject.tag == "Bullet" ||
            other.gameObject.tag == "Player" ||
            other.gameObject.tag.Contains("Enemy"))
        {
            return;
        }

        ObjectUtils.cleanUpObject(gameObject);
        ObjectUtils.createObject(explosion, gameObject.transform.position, gameObject.transform.rotation);
    }
}