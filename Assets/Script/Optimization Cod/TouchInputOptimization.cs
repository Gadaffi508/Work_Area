using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputOptimization : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    // Dokunma başladı
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    // Dokunma hareket etti
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    // Dokunma bitti
                }
            }
        }
    }
    
    //<-------------------------------------Optimize---------------------------------------------------------------------------->
    
    public void OnPointerDown(PointerEventData eventData)
    {
        // Touching started
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Touching moved
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Touching stoped
    }
}
