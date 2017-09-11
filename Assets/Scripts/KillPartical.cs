using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPartical : MonoBehaviour 
{
    
	void Update () 
    {
        ParticleSystem particleSystem =  GetComponent<ParticleSystem>();

        if (particleSystem != null && particleSystem.IsAlive())
        {
            ObjectUtils.cleanUpObject(gameObject);
        }
	}
}
