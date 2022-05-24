using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WhyTheFuckDoesMyScrollBarValueReset : MonoBehaviour
{
   [SerializeField] Scrollbar scrollbar;
   void Start(){
scrollbar.value = 1;
   }
}
