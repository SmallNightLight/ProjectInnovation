using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonValue : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BoolReference ButtonInput;

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonInput.Value = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ButtonInput.Value = false;
    }
}