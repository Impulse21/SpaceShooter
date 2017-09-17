using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagBehavior : EvadeBehavior
{
    public override IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        m_evadeTarget = Random.Range(1, dodge) * -Mathf.Sign(transform.position.y);

        while (true)
        {  
            yield return new WaitForSeconds(Random.Range(evadeTime.x, evadeTime.y));
            m_evadeTarget = -m_evadeTarget;
        }
    }
}
