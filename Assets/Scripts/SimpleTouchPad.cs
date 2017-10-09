using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float smoothing;

    private Vector2 m_origin;
    private Vector2 m_direction;
    private Vector2 m_smotthDirection;
    private bool m_touched;
    private int m_pointerID;

    void Start()
    {
        m_direction = new Vector2();
        m_smotthDirection = new Vector2();
        m_touched = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == m_pointerID)
        {
            // Get difference between start point and our current point position
            Vector2 currentPosition = eventData.position;
            Vector2 directionRaw = currentPosition - m_origin;
            m_direction = directionRaw.normalized;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!m_touched)
        {
            Debug.Log("Touch Event -> Touched");
            m_touched = true;
            m_origin = eventData.position;
            m_pointerID = eventData.pointerId;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == m_pointerID)
        {
            m_direction = Vector3.zero;
            m_touched = false;
        }
    }

    public Vector2 GetDirection()
    {
        m_smotthDirection = Vector2.MoveTowards(m_smotthDirection, m_direction, smoothing);
        return m_direction;
    }
}
