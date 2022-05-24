using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] Color32 red;
    [SerializeField] EventSystem eventSystem;
    
    public void OnSelect(BaseEventData eventData)
    {textMeshProUGUI.color = red;}

    public void OnDeselect(BaseEventData eventData)
    {textMeshProUGUI.color = Color.black;}

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
       eventSystem.SetSelectedGameObject(this.gameObject);
    }
}
