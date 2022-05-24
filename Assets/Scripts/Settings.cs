using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    bool mouseLock;
    void Awake()
    {Application.targetFrameRate = 60;
     Cursor.lockState = CursorLockMode.Confined;
   }

  
}
