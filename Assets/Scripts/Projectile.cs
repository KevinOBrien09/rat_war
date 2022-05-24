using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    bool hasData = false;
    Vector3 target;
    float speed;
    float Damage;
    Ownership ownership;
    [SerializeField] AudioClip clip;
     [SerializeField] AudioClip clip2;

    public void GetTarget(Vector3 Pos,float Speed, float dmg,Ownership ownership_)
    {
       target = Pos;
       speed = Speed;
       Damage = dmg;
       hasData = true;
       ownership = ownership_;
       Destroy(gameObject, 2);
    }

    void FixedUpdate()
    {
        if(hasData)
        {
            transform .position = Vector3.MoveTowards(transform.position,target,speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Unit")){
           
            Unit u = other.gameObject.GetComponent<Unit>();
            if(u.ownership != ownership)
            {
                SFXManager.instance.Play(clip,pitch: Random.Range(1.2f,1.5f)); 
                
                u.TakeDamage(Damage,-1);
                Destroy(gameObject);
            }
           
        }
    }
}
