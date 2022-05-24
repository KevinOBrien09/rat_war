using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum Ownership {Player,Enemy}
public class Base : MonoBehaviour
{
	public Ownership baseOwnership;
	public Era currentRats;
	public List<Unit> CurrentUnits = new List<Unit>();
	public int Gold;
	public float BaseHealth;
	public EXPManager expManager;
	[SerializeField] Transform SpawnLoc;
	[SerializeField] Base PlayerBase;
	[SerializeField] Base EnemyBase;
	[SerializeField] GameObject unitPrefab;
	[SerializeField] GameObject turretPrefab;
	[SerializeField] List<Transform> turretSlots = new List<Transform>();
	[SerializeField] List<bool> turretSlotsStatus = new List<bool>();
	[SerializeField] Button[] BuyRatButton;
	[SerializeField] TextMeshProUGUI[] UnitCosts;
	[SerializeField] Transform RatParent;
	[SerializeField] TextMeshProUGUI[] GoldText;
	[SerializeField] AudioSource source;
	[SerializeField] AudioClip Coins;
	[SerializeField]Transform EnemyTarget;
	[SerializeField] CheckIfTooManyRats checkIfTooManyRats;
	[SerializeField] TextMeshPro baseHealth;
	[SerializeField] List<bool> QueueSlot = new List<bool>();
	[SerializeField] Image BuildFill;
	[SerializeField] GameObject TraningIconPrefab;
	[SerializeField] Transform TraningIconHolder;
	[SerializeField] TextMeshProUGUI[] TrainingText;
	[SerializeField] Store store;
	[SerializeField] int selfUnitLayer;
	[SerializeField] int otherUnitLayer;
	[SerializeField] LayerMask otherUnitLayerMask;
	[SerializeField] SpriteRenderer[] renderer1;
	[SerializeField] AudioClip[] HitSounds;
	[SerializeField] Color32 teamColour;
	[SerializeField] Base rivalBase;
	[SerializeField] TextMeshProUGUI[] EndGameText;
	[SerializeField] TextMeshProUGUI YouWinLose;
	[SerializeField] GameObject EndGameButton;
	[SerializeField] Image GameOverBlackFade;
	[SerializeField] AudioClip GameOverBoom;
	[SerializeField] AudioClip[] GameOverCheerBoo;
	[SerializeField]  AudioSource Music;
	[SerializeField] AudioClip EndGameWinMice;
	[SerializeField] Color32 otherTeamColour;
	[HideInInspector] public int SpawnRate;
	
	bool inCoro;
	int ratCount = 0;
	Queue<IEnumerator>  SpawnQueue = new Queue<IEnumerator>();
	Queue<Rat> RatQueue = new Queue<Rat>();
	Coroutine mouseHover;
	Coroutine DecreaseSpawnRate;
	bool baseHit;
	
	void Start()
	{
		// if(baseOwnership == Ownership.Player)
		// {AssignRats(currentRats);}
		if(baseOwnership.Equals(Ownership.Player))
		{store.AssignItemsForSale(currentRats);}
		UpdateGold(350);
		BaseHealth = 500;
		
		baseHealth.DOFade(0,0);
		BuildFill.DOFillAmount(0,0);
		foreach (var item in TrainingText)
		{item.text = "";}
		//StartCoroutine(PassiveGold());
		SpawnRate = 5;
		
	}
	
	void OnMouseOver()
	{baseHealth.DOFade(1,.1f);}
	
	void OnMouseExit()
	{baseHealth.DOFade(0,.1f);}

	IEnumerator PassiveGold(){
		yield return new WaitForSeconds(1f);
		UpdateGold(5);
		StartCoroutine(PassiveGold());
	}

	public void UpdateGold(int moreGold)
	{
		Gold = Gold + moreGold;
		if(baseOwnership.Equals(Ownership.Player))
		{
			foreach (var item in GoldText)
			{item.text = "G:" + Gold.ToString();}
		}
	}

	IEnumerator DecreaseSR()
	{
		baseHit = true;
		yield return new WaitForSeconds(5);
		SpawnRate = 5;
		baseHit = false;
	}

	public void DamageBase(float Damage)
	{  
		SFXManager.instance.Play(HitSounds[Random.Range(0,HitSounds.Length)],volume: .33f ,pitch: Random.Range(.85f,1.1f));    
		float dmg = Damage/3;
		BaseHealth = BaseHealth - dmg;
		BaseHealth = Mathf.Ceil(BaseHealth);
		baseHealth.text = BaseHealth.ToString();
		if(!baseHit)
		{
			if(SpawnRate != 2)
			{

				SpawnRate = 2;
				DecreaseSpawnRate = StartCoroutine(DecreaseSR());
			}
			
			
			else{
				SpawnRate = 5;
			}
		}
		else
		{
			StopCoroutine(DecreaseSpawnRate);
		}
		
		
		
		//renderer1[0].transform.DOShakePosition(.1f,.25f);
		this.transform.DOShakePosition(.1f,.25f);
	
		foreach (var item in renderer1)
		{
			StartCoroutine(Flash(item));
		}
		if(BaseHealth <= 0)
		{
			StartCoroutine(EndGame());
			Debug.Log("GameOver");
		}
		
	}



	public IEnumerator Flash(SpriteRenderer renderer)
    {
        renderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        renderer.color = Color.white;
    }

	// public void BuildTurret(TurretInfo turret)
	// {
	// 	if(Gold >= turret.data.Cost)
	// 	{
	// 		source.pitch = Random.Range(.9f,1.1f);
	// 		source.PlayOneShot(Coins);

	// 		for (int i = 0; i < turretSlots.Count; i++)
	// 		{
	// 			if(!turretSlotsStatus[i])
	// 			{
	// 				GameObject newTurret = Instantiate(turretPrefab,turretSlots[i]);
	// 				Turret turretLogic = newTurret.GetComponent<Turret>();
	// 				turretLogic.SetInfo(turret,baseOwnership,otherUnitLayerMask,this);
	// 				turretSlotsStatus[i] = true;
	// 				break;
	// 			}
	// 		}
		
	// 	}

	// }
	
	public void Spawn(Rat rat )
	{
		// if(!checkIfTooManyRats.Full)
		// {
		if(baseOwnership.Equals(Ownership.Player))
		{
			if(Gold >= rat.data.Cost)
			{
				for (int i = 0; i <  QueueSlot.Count; i++)
				{
					if(!QueueSlot[i])
					{
						GameObject TrainIcon = null;
						if(baseOwnership.Equals(Ownership.Player))
						{	
							TrainIcon = Instantiate(TraningIconPrefab,TraningIconHolder);
							TrainIcon.transform.GetChild(0).GetComponent<Image>().sprite = rat.data.UIIcon;
						}
						SpawnQueue.Enqueue(BuildUnit(i,TrainIcon));
						RatQueue.Enqueue(rat);
						QueueSlot[i] = true;
						UpdateGold(-rat.data.Cost);
						source.pitch = Random.Range(.9f,1.1f);
						source.PlayOneShot(Coins);
						break;
					}
				}
				
				if(!inCoro)
				{StartCoroutine(SpawnQueue.Dequeue());} 
			}
			else
			{Debug.Log("No Money?");}
		}
		else if(baseOwnership.Equals(Ownership.Enemy))
		{
			for (int i = 0; i <  QueueSlot.Count; i++)
			{
				if(!QueueSlot[i])
				{	SpawnQueue.Enqueue(BuildUnit(i));
					RatQueue.Enqueue(rat);
					QueueSlot[i] = true;
					break;
				}
			}

			if(!inCoro)
			{StartCoroutine(SpawnQueue.Dequeue());} 
		}
			
			
			
			// GameObject go =  Instantiate(unitPrefab);
			// go.transform.position = SpawnLoc.transform.position;
			// Unit u =  go.GetComponent<Unit>();
			// u.ownership = Ownership.Enemy;
			// u.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = rat.data.UIIcon;
			// u.SetInfo(EnemyTarget, rat.data , this,PlayerBase,selfUnitLayer,teamColour);
			// go.transform.name = "Enemy" +  rat.data.Name_ + ratCount.ToString();
			// go.transform.SetParent(RatParent);
			// ratCount++;
	//	}
	}

	IEnumerator BuildUnit(int I, GameObject icon = null)
	{
		inCoro = true;
		GameObject go = null;
		Rat rat = null;
		rat = RatQueue.Dequeue();
		go = Instantiate(unitPrefab);
		go.SetActive(false);
		if(baseOwnership.Equals(Ownership.Player))
		{	BuildFill.DOFillAmount(1,rat.data.buildTime);
		}
	
		if(baseOwnership.Equals(Ownership.Player))
		{
			foreach (var item in TrainingText)
			{item.text = "TRAINING " + rat.data.Name_.ToUpper() +"...";}
		}
		
		yield return new WaitForSeconds(rat.data.buildTime+.1f);
		while(checkIfTooManyRats.Full)
		{
			if(baseOwnership.Equals(Ownership.Player))
			{
				foreach (var item in TrainingText)
				{item.text = "WAITING FOR SPACE..";}
				yield return null;
			}
			else
			{
				yield return null;
			}
		}
		if(baseOwnership.Equals(Ownership.Player))
		{
			foreach (var item in TrainingText)
			{item.text = "";}
		}
			if(baseOwnership.Equals(Ownership.Player))
		{
		BuildFill.DOFillAmount(0,0);
		}
		go.SetActive(true);
		QueueSlot[I] = false;
		if(baseOwnership.Equals(Ownership.Player))
		{Destroy(icon);}
		go.transform.position = SpawnLoc.transform.position;
		Unit u =  go.GetComponent<Unit>();
		u.ownership = baseOwnership;
	
		u.SetInfo(EnemyTarget, rat.data, this,rivalBase,selfUnitLayer,teamColour);
		go.transform.name = rat.data.Name_ + ratCount.ToString();
		go.transform.SetParent(RatParent);
		ratCount++;
		inCoro = false;
		
		if(SpawnQueue.Count > 0)
		{
			StartCoroutine(SpawnQueue.Dequeue());
		}
	}

	IEnumerator EndGame(){
	BaseHealth = 0;
	Music.DOFade(0,.1f);
	Time.timeScale = .25f;
	Camera.main.orthographicSize = Camera.main.orthographicSize - .25f;
	baseHealth.text = BaseHealth.ToString();
	this.transform.DOShakeRotation(2f,.5f);
	this.transform.DOMoveX(this.transform.position.x + 10, .5f);
	this.transform.DOMoveY(this.transform.position.y + 10, .5f);
	foreach (var item in EndGameText)
	{
		item.DOFade(1,.5f);
		item.text = "WASTED.";
	}
	EndGameText[1].color = rivalBase.teamColour;
	SFXManager.instance.Play(GameOverBoom,pitch: Random.Range(.9f,1.1f), volume: .5f);    
	  
	yield return new WaitForSeconds(.5f);
	
	GameOverBlackFade.DOFade(1,.25f);
	
	YouWinLose.DOFade(1,.25f);

	if(baseOwnership.Equals(Ownership.Player))
	{
		SFXManager.instance.Play(GameOverCheerBoo[1],pitch: Random.Range(.9f,1.1f), volume:.45f);    
		YouWinLose.text = "You Lose.";
	}
	else
	{	SFXManager.instance.Play(EndGameWinMice,pitch: Random.Range(.9f,1.1f), volume:2);  
		SFXManager.instance.Play(GameOverCheerBoo[0],volume: .2f);    
		YouWinLose.text = "You Win.";
	}
	yield return new WaitForSeconds(1f);
	EndGameButton.SetActive(true);
	
	
	yield return null;

	}


}
