using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour 
{
    public GameObject explosion;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boundary" || other.gameObject.tag == gameObject.gameObject.tag)
        {
            return;
        }

        if (other.gameObject.tag == "Player")
        {
            GameController.sharedInstance.gameOver();
        }

        ObjectUtils.cleanUpObject(other.gameObject);
        ObjectUtils.cleanUpObject(gameObject);
     
        ObjectUtils.createObject(explosion, gameObject.transform.position, gameObject.transform.rotation);
       
        Debug.Log("Destorying " + gameObject.name + " with tag \"" + gameObject.tag + "\" by object with a tag \"" + other.gameObject.tag + "\""); 
    }
}