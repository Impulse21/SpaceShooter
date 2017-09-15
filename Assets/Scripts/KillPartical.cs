using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPartical : MonoBehaviour 
{
    
	void Update () 
    {
        ParticleSystem particleSystem =  GetComponent<ParticleSystem>();

        if (particleSystem != null && !particleSystem.IsAlive())
        {
            Debug.Log("Clearning Up partical system");
            ObjectUtils.cleanUpObject(gameObject);
        }
	}
}
