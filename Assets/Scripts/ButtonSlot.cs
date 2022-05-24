using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSlot : MonoBehaviour
{
    public TextMeshProUGUI Cost;
    public ShowTooltip tooltip;
    public Image image;
    [HideInInspector] public Rat rat;
    [HideInInspector] public TurretInfo turret;
    [HideInInspector] public Base base_;
    public Button button;

    public void SpawnRat()
    {
        base_.Spawn(rat);
    }

    // public void SpawnTurret()
    // {
    //     base_.BuildTurret(turret);
    // }
   
}
