using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour 
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Boundary")
        {
            if (ObjectPool.SharedInstance.containsTag(gameObject.tag))
            {
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
