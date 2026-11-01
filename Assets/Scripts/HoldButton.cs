using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onHold;

    public float interval = 10f;

    private bool isHolding;
    private float timer;

    private void Update()
    {
        if (!isHolding)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            onHold.Invoke();
            timer = interval;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        timer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }
}