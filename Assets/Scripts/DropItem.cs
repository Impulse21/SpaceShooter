using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour 
{
    public List<GameObject> itemsToDrop;

    public int dropChance;

    private bool canDrop = false;

    void Start()
    {
        Random.InitState( (int) System.DateTime.Now.Ticks);
    }

    public void dropItem()
    {
        int rand = Random.Range(0, 10000);

        if (rand <= dropChance * 100)
        {
            GameObject item = itemsToDrop[Random.Range(0, itemsToDrop.Count - 1)];
            ObjectUtils.createObject(item, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
