using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public struct TurretData
{
    public string Name;
    [Multiline] public string ttDesc;
    public int Cost;
    public float Damage;
    public float Range;
    public Sprite[] Sprite_;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float animSpeed;
}

public class Turret : MonoBehaviour
{
    [SerializeField] TurretInfo info;
    [SerializeField] TurretData data;
    [SerializeField] Ownership ownership;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite_;
    public LayerMask layerMask;
    Vector3 dir;
    bool hasTarget;
    Unit Target;
    public Collider[] hitColliders; 
     [SerializeField] Base _base;
     [SerializeField]float MaxNutCount;
    [SerializeField] float CurrentNuts;
    bool SpeedBuff_;
    [SerializeField] float BuffDuration;
    [SerializeField] TMPro.TextMeshProUGUI AcornText;
     [SerializeField] TMPro.TextMeshProUGUI BuffText;
      [SerializeField] int NutCost;
      [SerializeField] Image BuffDur;
        [SerializeField] TMPro.TextMeshProUGUI EXPAT;

    void Start()
    {
        data = info.data;
        CurrentNuts = MaxNutCount;
        if(ownership.Equals(Ownership.Player))
        {
            AcornText.text = CurrentNuts.ToString() + "/" + MaxNutCount.ToString();
            EXPAT.text = CurrentNuts.ToString() + "/" + MaxNutCount.ToString();
        }
        
    }

    public void BuyNuts()
    {
        if(_base.Gold >= NutCost)
        {
            CurrentNuts = MaxNutCount;
            _base.UpdateGold(-NutCost);
            AcornText.text = CurrentNuts.ToString() + "/" + MaxNutCount.ToString();
              EXPAT.text = CurrentNuts.ToString() + "/" + MaxNutCount.ToString();
        }
       
    }

   public void ChangeSpeed()
    {
        if(!SpeedBuff_)
        {StartCoroutine(SpeedBuff());}
    }

    IEnumerator SpeedBuff()
    {
        if(_base.Gold >= NutCost)
        {
            SpeedBuff_ = true;
            _base.UpdateGold(-500);
            BuffDur.fillAmount = 1;
            BuffDur.DOFillAmount(0,BuffDuration);
            animator.SetFloat("speed_",3f);
            BuffText.text = "Buff APPLIED";
            yield return new WaitForSeconds(BuffDuration);
            animator.SetFloat("speed_",1f);
            BuffText.text = "BUFF SPEED 500G";
            SpeedBuff_ = false;

        }
   
    }
    void Update()
    {    
        hitColliders = Physics.OverlapSphere(transform.position,data.Range,layerMask);
        hitColliders = hitColliders.OrderBy((d) => (d.transform.position - transform.position).sqrMagnitude).ToArray();
        if(hitColliders.Length == 0)
        {
            Target = null;
            animator.Play("Idle");
        }
        else
        {
            if(ownership.Equals(Ownership.Enemy))
            {
                Unit u = hitColliders[0].gameObject.GetComponent<Unit>();
                if(u!= null)
                {
                    if(u.ownership != ownership)        
                    {
                        Target = u;
                        animator.speed = data.animSpeed;
                        animator.Play("Attack");
                        hasTarget = true;
                    }
                }
            }
            else
            {
                Unit u = hitColliders[0].gameObject.GetComponent<Unit>();
                if(u!= null)
                {
                    if(u.ownership != ownership)        
                    {
                        if(CurrentNuts > 0)
                        {
                            Target = u;
                            animator.speed = data.animSpeed;
                            animator.Play("Attack");
                            hasTarget = true;
                        }
                        else{
                             Target = null;
                            animator.Play("Idle");
                        }
                        
                    }
                }

            }
           
        }
    }

    public void Shoot()
    {
        if(Target != null)
        {
            if(ownership.Equals(Ownership.Player))
            {
                CurrentNuts--;
                AcornText.text = CurrentNuts.ToString() + "/" + MaxNutCount.ToString();
                EXPAT.text = CurrentNuts.ToString() + "/" + MaxNutCount.ToString();
            }
            GameObject p = Instantiate(data.projectilePrefab,transform);
            p.transform.SetParent(null);
            p.GetComponent<Projectile>().GetTarget(Target.transform.position,data.projectileSpeed,data.Damage,ownership);
        }
    }
    
    void OnDrawGizmos()
    {
        if(ownership.Equals(Ownership.Enemy))
        {Gizmos.color = Color.blue;}
        else
        {Gizmos.color = Color.red;}
        Gizmos.DrawWireSphere(transform.position, data.Range);
    }
}
