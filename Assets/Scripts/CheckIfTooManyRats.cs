using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfTooManyRats : MonoBehaviour
{
    public bool Full;
    [SerializeField] Ownership ownership;
    Unit unit;
  
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Unit"))
        {
            unit = other.gameObject.GetComponent<Unit>();
            if(unit.ownership.Equals(ownership))
            {
                Full = true;
                unit.checkIfTooManyRats = this;
            }
        }
    }
    
    public void RemoveUnit()
    {
        if(unit.ownership.Equals(ownership))
        {
            Full = false;
            unit.checkIfTooManyRats = null;
            unit = null;
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals("Unit"))
        {  if(unit != null)
            {RemoveUnit();}
        }
    }
}
