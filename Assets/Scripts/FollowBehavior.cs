using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : MonoBehaviour 
{
    public GameObject objectToFollow;

	
	// Update is called once per frame
	void Update () 
    {
        if (objectToFollow != null)
        {
            gameObject.transform.position = objectToFollow.transform.position;
        }
	}
}
