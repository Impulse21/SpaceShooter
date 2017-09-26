using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUtils : MonoBehaviour 
{
    public static void createObject(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        GameObject obj = ObjectPool.SharedInstance.GetPooledObject(gameObject.tag);

        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
        }
        else
        {
            Instantiate(gameObject, position, rotation);
        }
    }

    public static void cleanUpObject(GameObject gameObj)
    {
        DropItem dropItem = gameObj.GetComponent<DropItem>();

        if (dropItem != null)
        {
            dropItem.dropItem();
        }

        if (ObjectPool.SharedInstance.containsTag(gameObj.tag))
        {
            gameObj.SetActive(false);
        }
        else
        {
            Destroy(gameObj);
        }
    }
}
