using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByPlayerBullet : MonoBehaviour 
{
    public int scoreValue;
    public GameObject explosion;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            ObjectUtils.cleanUpObject(other.gameObject);
            ObjectUtils.cleanUpObject(gameObject);

            ObjectUtils.createObject(explosion, gameObject.transform.position, gameObject.transform.rotation);

            GameController.sharedInstance.addScore(scoreValue);

            Debug.Log("Destorying " + gameObject.name + " with tag \"" + gameObject.tag + "\" by object with a tag \"" + other.gameObject.tag + "\""); 
        }
    }
}
