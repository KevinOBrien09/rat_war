using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI[] nameField;
    public TextMeshProUGUI[] descField;
    public Image bg;
    [SerializeField] LayoutElement layoutElement;
    [SerializeField] int wrapLimit;
    [SerializeField] RectTransform rectTransform;

    public void SetText(string desc ="", string name ="")
    {
        if(name.Equals(string.Empty))
        {
            foreach (var item in nameField)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var item in nameField)
            {
                item.gameObject.SetActive(true);
                item.text = name;
            }

        }

        foreach (var item in descField)
        {
            
            item.text = desc;
        }

        int nameLength = nameField[0].text.Length;
        int descLength = descField[0].text.Length;

        layoutElement.enabled = (nameLength > wrapLimit || descLength > wrapLimit) ? true:false;

    }

    void Update(){
        Vector2 pos = Input.mousePosition;

        float pX = pos.x/Screen.width;
        float pY = pos.y/Screen.height;
        rectTransform.pivot = new Vector2(pX,pY);
        transform.position = pos;
    }

    

}
