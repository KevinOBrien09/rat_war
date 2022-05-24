using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextEffect : MonoBehaviour
{
    public Color32[] TeamColors;
    public TextMeshPro[] Texts;
   
   public  void Throw(Ownership ownership, string Text_)
    {
        foreach (var item in Texts)
        { item.text = Text_;}
        if(ownership == Ownership.Player)
        {Texts[1].color = TeamColors[0];}
        else if(ownership == Ownership.Enemy)
        {Texts[1].color = TeamColors[1];}
        transform.DOMoveY(transform.position.y + 2f, .5f);
        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(.55f);
        foreach (var item in Texts)
        {
            item.DOFade(0,.2f);
        }
        yield return new WaitForSeconds(.25f);
        Destroy(gameObject);
    }

}
