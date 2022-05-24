using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    public static Stats instance;
    int DeadBlueRats;
    int DeadRedRats;
    float BlueDamageDealt;
    float RedDamageDealt;
    [SerializeField]  TextMeshProUGUI DR;
    [SerializeField]  TextMeshProUGUI DB;
    [SerializeField]  TextMeshProUGUI RDD;
    [SerializeField]  TextMeshProUGUI BDD;
   


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
     

    }

    public void UpdateStats(bool DeadReds = false,bool DeadBlues = false, float redDamageDealt = 0, float blueDamageDealt = 0)
    {
        if(DeadReds)
        {
            DeadRedRats++;
            DR.text = DeadRedRats.ToString() + " DEAD RATS";
        }

        if(DeadBlues)
        {
            DeadBlueRats++;
            DB.text =   DeadBlueRats.ToString() + " DEAD RATS";
        }

        if(redDamageDealt > 0){
            RedDamageDealt = RedDamageDealt+redDamageDealt;
            RDD.text =    RedDamageDealt.ToString() + " DAMAGE DEALT";
        }

           if(blueDamageDealt > 0){
            BlueDamageDealt = BlueDamageDealt+blueDamageDealt;
            BDD.text =   BlueDamageDealt.ToString() + " DAMAGE DEALT";
        }
    }

    

}
