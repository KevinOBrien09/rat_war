using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DrawRay : MonoBehaviour
{
    public float d;
    public Transform loc;
   void LateUpdate()
    {
        Debug.DrawRay(transform.position, loc.TransformDirection(Vector3.left) * d, Color.yellow);
    }
}