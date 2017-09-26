using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour 
{
    public List<GameObject> Shields = new List<GameObject>();

    private int m_activeShieldLvl;
    private Collider2D m_collider2D;

	// Use this for initialization
	void Start () 
    {
        foreach (var shield in Shields)
        {
            shield.SetActive(false);
        } 

        m_activeShieldLvl = 0;
        m_collider2D = GetComponent<Collider2D>();
        m_collider2D.enabled = false;
	}
   
    public void enableSheilds()
    {
        foreach (var shield in Shields)
        {
            shield.SetActive(true);
        }  

        m_activeShieldLvl = Shields.Count;
        m_collider2D.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boundary" ||
            other.gameObject.tag == gameObject.gameObject.tag ||
            other.gameObject.tag == "Bullet" ||
            other.gameObject.tag == "PowerUp")
        {
            return;
        }

        if (m_activeShieldLvl > 0 && m_activeShieldLvl <= Shields.Count)
        {
            Shields[m_activeShieldLvl - 1].SetActive(false);
            m_activeShieldLvl--;

            if (m_activeShieldLvl == 0)
            {
                m_collider2D.enabled = false;
            }
        }
    }
}
