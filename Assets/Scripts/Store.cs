using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.CSharp;

public class Store : MonoBehaviour
{
	[SerializeField] GameObject buttonPrefab;
	[SerializeField] RectTransform UnitButtonHolder;
	[SerializeField] RectTransform TurretButtonHolder;
	[SerializeField] Base base_;
	List<ButtonSlot> CurrentButtons = new List<ButtonSlot>();
	
    public void AssignItemsForSale(Era era)
	{
		foreach (var item in CurrentButtons)
		{
			item.button.onClick.RemoveAllListeners();
			Destroy(item.gameObject);
		}

		for (int i = 0; i < era.Rats.Count; i++)
		{
			GameObject currentButton = Instantiate(buttonPrefab, UnitButtonHolder);
			ButtonSlot BS = currentButton.GetComponent<ButtonSlot>();
			BS.Cost.text =  "G:" + era.Rats[i].data.Cost.ToString();
			BS.image.sprite = era.Rats[i].data.UIIcon;
			BS.tooltip.desc_ = era.Rats[i].data.ttdesc;
			BS.tooltip.name_ = era.Rats[i].data.Name_;
			BS.rat = era.Rats[i];
			BS.base_ = base_;
			BS.button.onClick.AddListener(() => {BS.SpawnRat();});
			CurrentButtons.Add(BS);
		}
		
		// for (int i = 0; i < era.Turrets.Count; i++)
		// {
		// 	GameObject currentButton = Instantiate(buttonPrefab, TurretButtonHolder);
		// 	ButtonSlot BS = currentButton.GetComponent<ButtonSlot>();
		// 	BS.Cost.text =  "G:" + era.Turrets[i].data.Cost.ToString();
		// 	BS.image.sprite = era.Turrets[i].data.Sprite_[0];
		// 	BS.tooltip.desc_ = era.Turrets[i].data.ttDesc;
		// 	BS.tooltip.name_ = era.Turrets[i].data.Name;
		// 	BS.turret = era.Turrets[i];
		// 	BS.base_ = base_;
		// 	BS.button.onClick.AddListener(() => {BS.SpawnTurret();});
		// 	CurrentButtons.Add(BS);
		// }
	}
}
