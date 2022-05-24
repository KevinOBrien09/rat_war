using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public enum RatClass{Basic,Tank,Kamikaze}

[System.Serializable]
public struct RatData
{
    public string Name_;
    [Multiline]public string ttdesc;
    public Sprite[] sprite_;
    public float MaxHealth;
    public float CurrentHealth;
    public float DamageLower;
    public float DamageUpper;
    public float Crit;
    public float Dodge;
    public int Cost;
    public float buildTime;
    public float unitRange;
    public float baseRange;
    public float HPBarHeight;
    public Vector3 ColliderSize;
    public Vector3 ColliderCenter;
    public AudioClip[] HitSolid;
    public AudioClip[] Squeal;
    public AudioClip[] Die;
    public AudioClip[] hitSFX;
    public Sprite UIIcon;
    public RatClass class_;
}

public class Unit : MonoBehaviour
{   
    
    public RatData data;
    public Ownership ownership;
    public  CheckIfTooManyRats checkIfTooManyRats;
    [SerializeField] GameObject TextEffectPrefab;    
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip DodgeSFX;
    [SerializeField] AudioClip CriticalSFX;
    [SerializeField] float Speed;
    [SerializeField] SpriteRenderer[] Graphic;
    [SerializeField] Transform RayObj;
    [SerializeField] Animator animator;
    [SerializeField] GameObject HPBarParent;
    [SerializeField] TextMeshProUGUI[] HPCount;
    [SerializeField] Image HPBar;
    [SerializeField] BoxCollider collider_;
    [SerializeField] LayerMask unitsLayers;
    [SerializeField] LayerMask baseLayer;
    [SerializeField] AudioSource hit;
    public Unit AllyInFront;
    Color32 thisTeam;
    Base OwnBase;
    Base RivalBase;
    bool OverLap;
    Transform transform_;
    bool canMove;
    Vector3 dir;
    Unit Rival;
    bool atBase;
    enum WhatToHit {Base,Unit};
    WhatToHit whatToHit;
    bool inCoro;
    Coroutine HPTD;
    bool leap;
    
   
    
    public void SetInfo(Transform trans, RatData incomingData,Base ownBase,Base rivalBase, int layer_,Color32 unitColour)
    {      
        foreach (var item in HPCount)
        {item.DOFade(0,0f);}
        HPBar.DOFade(0,0f);
        ownBase.CurrentUnits.Add(this);
        OwnBase = ownBase;
        data = incomingData;
        transform_ = trans;
        this.gameObject.layer = layer_;
        RivalBase = rivalBase;
        canMove = true;
        thisTeam = unitColour;
        collider_.size = data.ColliderSize;
        HPBarParent.GetComponent<RectTransform>().DOAnchorPosY(data.HPBarHeight,0);
       // collider_.center = data.ColliderCenter;

        if(ownership.Equals(Ownership.Enemy))
        {
            
            foreach (var item in Graphic)
            {item.flipX = true;}

            Graphic[0].sprite = data.sprite_[0];
            Graphic[1].sprite = data.sprite_[1];
            Graphic[2].sprite = data.sprite_[1];

            dir = Vector3.left;
            HPBar.fillOrigin = 1;
        }
        else if(ownership.Equals(Ownership.Player))
        {
            foreach (var item in Graphic)
            {
               
                item.flipX = false;
            }
            Graphic[0].sprite = data.sprite_[0];
            Graphic[1].sprite = data.sprite_[1];
            Graphic[2].sprite = data.sprite_[1];
            
            dir = Vector3.right;
            SFXManager.instance.Play(data.Squeal[Random.Range(0,data.Squeal.Length)],pitch: Random.Range(.9f,1.1f)); 
            HPBar.fillOrigin = 0;
        }
        HPBar.color =  thisTeam;
        HPCount[0].color =  thisTeam;
        Graphic[1].color = thisTeam;
        HPBar.DOFillAmount(data.CurrentHealth/data.MaxHealth,.1f);
        HPCount[0].text = data.CurrentHealth.ToString();
        HPCount[1].text = data.CurrentHealth.ToString();
    }

    void OnMouseOver()
	{ 
        if(inCoro)
        {return;}
        else
        {
            foreach (var item in HPCount)
            {   
                HPCount[0].color =  thisTeam;
                item.DOFade(1,.1f);
            }
            HPBar.DOFade(1,.1f);
            HPBar.color = thisTeam;
        }
    }
	
	void OnMouseExit()
	{
        if(inCoro){
            return;
        }
      else
        {
            foreach (var item in HPCount)
            {   HPCount[0].color =  thisTeam;
                item.DOFade(0,.1f);
            }
            HPBar.DOFade(0,.1f);
            HPBar.color = thisTeam;
        }
    }

    
    void Update()
    {
        if(data.class_ == RatClass.Kamikaze && leap){
          return;
        }
          canIWalk(FriendlyInFront(),EnemyInFront(),BaseInFront());
      
    }
    
    public bool FriendlyInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(RayObj.transform.position,RayObj.transform.TransformDirection(dir), out hit,data.unitRange)) 
        {
            if(hit.collider.gameObject.tag.Equals("Unit"))
            {
                Unit u =  hit.collider.GetComponent<Unit>();
                if(u.ownership.Equals(ownership))
                {
                    AllyInFront = u;
                    if(data.class_.Equals(RatClass.Kamikaze) & !leap)
                    {
                        if(OwnBase.CurrentUnits[0] == this |OwnBase.CurrentUnits[1])
                        {
                            DealWithLeap();
                        }
                    }
                    return true;
                }
            }
            else
            {return false;}
            return false;
        }
        else
        {return false;}
    }

    public (bool EF, Unit unit) EnemyInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(RayObj.transform.position,RayObj.transform.TransformDirection(dir), out hit,data.unitRange,unitsLayers)) 
        {
            if(hit.collider.gameObject.tag.Equals("Unit"))
            {
                Unit u = hit.collider.GetComponent<Unit>();
                if(!u.ownership.Equals(ownership))
                {return (true, u);}
            }
            else
            {return (false,null);}
            return (false,null);
        }
        else
        {return(false,null);}
    }

    bool BaseInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(RayObj.transform.position,RayObj.transform.TransformDirection(dir), out hit,data.baseRange,baseLayer)) 
        {
            if(hit.collider.gameObject.tag.Equals("Base"))
            {   
                Base b = hit.collider.GetComponent<Base>();
                if(!b.baseOwnership.Equals(ownership) & b.BaseHealth > 0)
                {return true;}
                else if(!b.baseOwnership.Equals(ownership) & b.BaseHealth <= 0)
                {   canMove = false;
                    animator.Play("Idle");
                    return true;
                }            
            }
            else
            {return false;}
            return false;
        }
        else
        {return false;}
    }

    bool canIWalk(bool isFriendlyInFront, (bool enemyInFront, Unit unit)sneed, bool baseInFront )
    {
        // if(ownership.Equals(Ownership.Enemy))
        // {
        //     Debug.Log(" Friendly In Front: " + isFriendlyInFront + ". Enemy in front: " + sneed.enemyInFront +". Base in Front: " + baseInFront);
        // }
      
        if(!sneed.enemyInFront & !baseInFront! & !isFriendlyInFront) //Nothing blocking
        {
            canMove = true;
            animator.Play("Walk");
            return true;
        }
        else if(sneed.enemyInFront& !baseInFront & !isFriendlyInFront) //only enemy
        {
            canMove = false;
            Rival = sneed.unit;
            whatToHit = WhatToHit.Unit;
            animator.Play("Attack");
            return false;
        }
        else if(sneed.enemyInFront & baseInFront & !isFriendlyInFront) //enemy and base
        {
          
            canMove = false;
            Rival = sneed.unit;
            whatToHit = WhatToHit.Unit;
            animator.Play("Attack");
            return false;
        }
        else if(!sneed.enemyInFront & baseInFront & !isFriendlyInFront) //only base
        {
            canMove = false;
            whatToHit = WhatToHit.Base;
            animator.Play("Attack");
            return false;
        }
        else if(!sneed.enemyInFront & !baseInFront & isFriendlyInFront) //only ally
        {
            canMove = false;
            animator.Play("Idle");
            return false;
        }
        else
        {return false;}
    }
    
    void FixedUpdate()
    {
        if(canMove)
        {
            Vector3 loc =  new Vector3(transform_.position.x,this.transform.position.y,this.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position,loc,Speed * Time.deltaTime);
        }
    }

    void DealWithLeap()
    {
        if(AllyInFront!= null)
        {
            if(AllyInFront.Rival != null)
            {   
                animator.SetTrigger("Leap1");
                if(ownership == Ownership.Enemy)
                {
                    foreach (var item in Graphic)
                    {item.flipX = false;}
                    transform.localScale = new Vector3(-1,1,1);
                }
                leap = true;
            }

            if(AllyInFront.AllyInFront != null)
            {
                if(AllyInFront.AllyInFront.Rival != null)
                {
                    animator.SetTrigger("Leap2");
                    if(ownership == Ownership.Enemy)
                    {
                        foreach (var item in Graphic)
                        {item.flipX = false;}
                        transform.localScale = new Vector3(-1,1,1);
                    }
                    leap = true;
                }
            }
        }

    }
    
    public void Attack()
    {
        // if(!SFXManager.instance.musicPlaying)
        // {
        //   SFXManager.instance.PlayMusic();
        
        // }
     
        if( whatToHit.Equals(WhatToHit.Unit))
        {
            SFXManager.instance.UnitFight();
            float d = Mathf.Ceil(Random.Range(data.DamageLower,data.DamageUpper));   
            Rival.TakeDamage(d, data.Crit);
        }
        else if( whatToHit.Equals(WhatToHit.Base))
        {
            source.volume =  .5f;
          
            float d = Mathf.Ceil(Random.Range(data.DamageLower,data.DamageUpper)); 
          
            RivalBase.DamageBase(d);
        }
    }
    
    public void TakeDamage(float Damage, float EnemyCrit)
    {
        if(!inCoro)
        {
            foreach (var item in HPCount)
            {
                item.DOFade(1,.1f);
            }
            HPBar.DOFade(1,.1f);
            HPTD = StartCoroutine(ShowHealthBarInCombat());
        }
        else
        {
            if(gameObject != null){
                StopCoroutine(HPTD);
                HPTD = StartCoroutine(ShowHealthBarInCombat());
            }
          
        }
        int Rand =  Random.Range(0,100);
        if(Rand >= data.Dodge)
        {
            int Random_ =  Random.Range(0,100);
            if(Random_ <= EnemyCrit)
            {
                GameObject TE = Instantiate(TextEffectPrefab, this.transform);
                TE.transform.SetParent(null);
                if(ownership.Equals(Ownership.Enemy))
                {
                    TE.GetComponent<TextEffect>().Throw(Ownership.Player,"CRITICAL!");
                   
                }
                else if(ownership.Equals(Ownership.Player))
                {
                    TE.GetComponent<TextEffect>().Throw(Ownership.Enemy,"CRITICAL!");
                   
                }
                SFXManager.instance.Play(CriticalSFX,volume:.5f, pitch:Random.Range(.75f,1.25f)); 
                data.CurrentHealth = data.CurrentHealth - data.CurrentHealth;
                HPBar.DOFillAmount(data.CurrentHealth/data.MaxHealth,.1f);
                HPCount[0].text = data.CurrentHealth.ToString();
                HPCount[1].text = data.CurrentHealth.ToString();

                 if(ownership.Equals(Ownership.Player))
                {
                    Stats.instance.UpdateStats(blueDamageDealt:data.CurrentHealth);
                }
                else{
                    Stats.instance.UpdateStats(redDamageDealt:data.CurrentHealth);
                }


                
                Die();
            }
            else
            {
            
                data.CurrentHealth = data.CurrentHealth - Damage;
                HPBar.DOFillAmount(data.CurrentHealth/data.MaxHealth,.1f);
                HPCount[0].text = data.CurrentHealth.ToString();
                HPCount[1].text = data.CurrentHealth.ToString();
            
                if(ownership.Equals(Ownership.Player))
                {
                    Stats.instance.UpdateStats(blueDamageDealt:Damage);
                }
                else{
                    Stats.instance.UpdateStats(redDamageDealt:Damage);
                }

                if(this != null)
                {
                    StartCoroutine(Flash(Graphic[0]));
                }
                if(data.CurrentHealth <= 0)
                {Die();}
            }
        }
        else
        {
            GameObject TE = Instantiate(TextEffectPrefab, this.transform);
            StartCoroutine(HPBarFlash());
            TE.transform.SetParent(null);
            TE.GetComponent<TextEffect>().Throw(ownership,"Dodge!");
            SFXManager.instance.Play(DodgeSFX,pitch: Random.Range(1.25f,1.33f),volume: .33f);    
        
        }
    }

    IEnumerator ShowHealthBarInCombat()
    {
        inCoro = true;
        yield return new WaitForSeconds(1f);
        HPCount[0].color =  thisTeam;
        HPBar.color = thisTeam;
        foreach (var item in HPCount)
        {
            item.DOFade(0,.5f);
        }
        HPBar.DOFade(0,.5f);
        inCoro = false;

    }

    public IEnumerator HPBarFlash()
    {
        HPCount[0].color = Color.white;  
        HPBar.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        HPCount[0].color = thisTeam;
        HPBar.color = thisTeam;
    }
    
    public IEnumerator Flash(SpriteRenderer renderer)
    {
        renderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        renderer.color = Color.white;
    }

    public void Die()
    {
        SFXManager.instance.UnFadeFight();
        SFXManager.instance.Play(data.Die[Random.Range(0,data.Die.Length)],pitch: Random.Range(.85f,1.1f));    
        if(checkIfTooManyRats)
        {checkIfTooManyRats.RemoveUnit();}
        RivalBase.expManager.AddXP((int)data.Cost/10);
        RivalBase.UpdateGold(data.Cost + data.Cost/10);
        OwnBase.CurrentUnits.Remove(this);

        if(ownership.Equals(Ownership.Player))
        {
            Stats.instance.UpdateStats(DeadReds: true);
        }
        else{
             Stats.instance.UpdateStats(DeadBlues: true);
        }
        Destroy(gameObject);
    }

    public void Leap1()
    {
        if(AllyInFront != null & AllyInFront.Rival != null) 
        {
           
           

            AllyInFront.Rival.TakeDamage(0,101);
            
      
        }
       
        Die();
    }

    public void Leap2()
    {
       

        if(AllyInFront != null & AllyInFront.AllyInFront != null & AllyInFront.AllyInFront.Rival != null)
        {
                
            
            AllyInFront.AllyInFront.Rival.TakeDamage(0,101);
                
        }
    
        
    


        Die();
    }
}
