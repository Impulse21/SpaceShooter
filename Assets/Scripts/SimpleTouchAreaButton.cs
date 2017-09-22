using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SimpleTouchAreaButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool m_touched;
    private bool m_canFire;
    private int m_pointerID;

    void Awake()
    {
        m_canFire = false;
        m_touched = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!m_touched)
        {
            m_touched = true;
            m_canFire = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == m_pointerID)
        {
            m_canFire = false;
            m_touched = false;
        }
    }

    public bool CanFire()
    {
        return m_canFire;
    }
}
