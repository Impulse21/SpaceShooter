using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeBehavior : MonoBehaviour 
{
    public int dodge;
    public float smoothing;
    public Vector2 startWait;
    public Vector2 evadeTime;
    public Vector2 evadeWait;

    private Vector2 m_currentSpeed;
    private float m_evadeTarget;
    private Rigidbody2D m_rigidBody;

	// Use this for initialization
	void Start () 
    {
        Random.InitState((int) System.DateTime.Now.Ticks);
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_currentSpeed = m_rigidBody.velocity;
        StartCoroutine(Evade());
	}

    public IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            m_evadeTarget = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x);
            yield return new WaitForSeconds (Random.Range (evadeTime.x, evadeTime.y));
            m_evadeTarget = 0;
            yield return new WaitForSeconds (Random.Range (evadeWait.x, evadeWait.y));
        }
    }

    public void FixedUpdate()
    {
        float newManeuver = Mathf.MoveTowards (m_rigidBody.velocity.x, m_evadeTarget, Time.deltaTime * smoothing);
        m_rigidBody.velocity = new Vector3 (m_currentSpeed.x, newManeuver, 0.0f);
    }
}
