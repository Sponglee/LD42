using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            FunctionHandler.Instance.ClickHandler(gameObject.name,0);
        else if (eventData.button == PointerEventData.InputButton.Right)
            FunctionHandler.Instance.ClickHandler(gameObject.name,1);
    }
}