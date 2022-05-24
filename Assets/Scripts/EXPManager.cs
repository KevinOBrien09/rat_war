using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

 public enum Level{ModernEra,Rome,Future}
public class EXPManager : MonoBehaviour
{
   int level;
    public Ownership ownership;
    [SerializeField] float currentEXP,targetEXP;
    [SerializeField]  TextMeshProUGUI[] levelText,currentEXPtext,targetEXPtext;
    [SerializeField]  Image levelFill;
    [SerializeField] Store store;
    [SerializeField] Era[] eras;
    

    void Start()
    {
        level = 0;

        if(ownership.Equals(Ownership.Player))
        {
         
            foreach (var item in  targetEXPtext)
            {item.text = targetEXP.ToString();}

            foreach (var item in  currentEXPtext)
            {item.text = currentEXP.ToString();}
        }
     
    }
    
    public void AddXP(int XP)
    {
        currentEXP = currentEXP + XP;
        while(currentEXP >= targetEXP)
        {

            currentEXP = currentEXP - targetEXP;
            level++;
            targetEXP =  targetEXP +  targetEXP / 20;
            targetEXP = Mathf.Ceil(targetEXP);

            if(ownership.Equals(Ownership.Player))
            {
                foreach (var item in levelText)
                {
                   item.text = "Level: " + level.ToString();
                   
                }

                foreach (var item in  targetEXPtext)
                {
                    item.text = targetEXP.ToString();
                }
            }
        }
        
        if(ownership.Equals(Ownership.Player))
        {
            foreach (var item in  currentEXPtext)
            {
                item.text = currentEXP.ToString();
            }
            levelFill.DOFillAmount( currentEXP/targetEXP,.25f);
        }
    }
}
