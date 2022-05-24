using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float edgeSize;
    [SerializeField] Camera camera_;
    [SerializeField] float speed;
    [SerializeField] Transform target;
    [SerializeField] float smoothspeed = 0.125f;
    [SerializeField] Vector3 offset;
    [SerializeField] float[] EndPoints;
    bool Move;
    
    void Update()
    {target.position =  Camera.main.ScreenToWorldPoint(Input.mousePosition);}
    
    void FixedUpdate()
    {
        if(Input.mousePosition.x > Screen.width - edgeSize && transform.position.x <= EndPoints[1] )
        {
            MoveCam();
        }
        else if (Input.mousePosition.x < edgeSize && transform.position.x >= EndPoints[0] )
        {
           MoveCam(); 
        }
        
      
    }

    void MoveCam()
    {
        Vector3 desiredposition = target.position + offset;
        Vector3 smoothedposition = Vector3.Lerp(transform.position, desiredposition, smoothspeed*Time.deltaTime);
        Vector3 newPos = new Vector3(smoothedposition.x,offset.y,-10);
        transform.position = newPos;

    }

}
