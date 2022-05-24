using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ShowTooltip : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public string name_;
    public string desc_;
    Coroutine coroutine;
    public void OnPointerEnter(PointerEventData eventData)
    {
       coroutine = StartCoroutine(inputDelay());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
       
        ToolTipSystem.Hide();
    }

    IEnumerator inputDelay()
    {
        yield return new WaitForSeconds(.5f);
        ToolTipSystem.Show(name: name_,desc: desc_);

    }
}
