using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToolTipSystem : MonoBehaviour
{
    public static ToolTipSystem instance;
    [SerializeField] Tooltip tooltip;

    void Awake()
    {
        if(instance == null)
        {instance = this;}
        else
        {Destroy(gameObject);}
    }

    public static void Show(string name ="", string desc = "")
    {
        instance.tooltip.SetText(name,desc);
        instance.tooltip.gameObject.SetActive(true);
        instance.tooltip.bg.DOFade(1,.1f);
        for (int i = 0; i < 2; i++)
        {
            instance.tooltip.nameField[i].DOFade(1,.1f);
            instance.tooltip.descField[i].DOFade(1,.1f);
        }
      
    }

     public static void Hide(string name ="", string desc = "")
     {
        instance.tooltip.SetText(name,desc);
        instance.tooltip.SetText(name,desc);
       
       
        instance.tooltip.gameObject.SetActive(false);
    }
}
