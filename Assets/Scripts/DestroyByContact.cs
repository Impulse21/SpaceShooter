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
            other.gameObject.tag == "Enemy" ||
            other.gameObject.tag == "Enemy2" ||
            other.gameObject.tag.Contains("PowerUp") ||
            other.gameObject.tag == "EnemyBullet" ||
            other.gameObject.tag == "Meteor")
        {
            return;
        }

        ObjectUtils.cleanUpObject(gameObject);
        ObjectUtils.createObject(explosion, gameObject.transform.position, gameObject.transform.rotation);
    }
}