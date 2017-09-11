using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectPoolItem
{
    public GameObject objectToPool;
    public int numObjectInPool;
    public bool canGrow;

}

public class ObjectPool : MonoBehaviour 
{
    public static ObjectPool SharedInstance;

    public List<ObjectPoolItem> ObjectsToPool;

    private List<GameObject> m_objectPool;

    void Awake()
    {
        SharedInstance = this;
    }

	// Use this for initialization
	void Start () 
    {
        m_objectPool = new List<GameObject>();

        foreach (var item in ObjectsToPool)
        {
            for (int i = 0; i < item.numObjectInPool; i++)
            {
                addObjectToPool(item.objectToPool);
            }
        }
	}

    public bool containsTag(string tag)
    {
        bool containsTag = false;

        foreach(var pooledObj in m_objectPool)
        {
            if(pooledObj.tag == tag)
            {
                containsTag = true;
            }
        }

        return containsTag;
    }

    public GameObject GetPooledObject(string tag)
    {
        foreach(var pooledObj in m_objectPool)
        {
            if(!pooledObj.activeInHierarchy && pooledObj.tag == tag)
            {
                return pooledObj;
            }
        }
            
        foreach (var objectItem in ObjectsToPool)
        {
            if (objectItem.objectToPool.tag == tag && objectItem.canGrow)
            {
                return addObjectToPool(objectItem.objectToPool);
            }    
        }

        return null;
    }

    private GameObject addObjectToPool(GameObject objectToPool)
    {
        GameObject newObj = (GameObject)Instantiate(objectToPool);

        newObj.SetActive(false);

        m_objectPool.Add(newObj);

        return newObj;
    }
}
