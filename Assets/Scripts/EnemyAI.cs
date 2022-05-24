using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Base base_;
    [SerializeField] CheckIfTooManyRats checkIfTooManyRats;

    void Start()
   {
      
        StartCoroutine(DecideWhenToSpawn());
        base_.Spawn(base_.currentRats.Rats[2]);
    }


    public IEnumerator DecideWhenToSpawn()
    {
       yield return new WaitForSeconds(base_.SpawnRate);
       int rand = Random.Range(0,2);
       if(rand == 1)
       {
            int whatRat = Random.Range(1,10); // 30 mm 20 sr 30 ps 20 jm
            Rat rat = null;
            switch (whatRat)
            {
                case 1:
                {rat = base_.currentRats.Rats[0] ;}
                break;
                case 2:
                {rat = base_.currentRats.Rats[0] ;}
                break;
                case 3:
                {rat = base_.currentRats.Rats[3] ;}
                break;
                case 4:
                { rat = base_.currentRats.Rats[2] ;}
                break;
                case 5:
                {rat = base_.currentRats.Rats[2] ;}
                break;
                case 6:
                {rat = base_.currentRats.Rats[3] ;}
                break;
                case 7:
                {rat = base_.currentRats.Rats[1] ;}
                break;
                case 8:
                {rat = base_.currentRats.Rats[1] ;}
                break;
                case 9:
                {rat = base_.currentRats.Rats[2] ;}
                break;
                case 10:
                {rat = base_.currentRats.Rats[3] ;}
                break;
            }
            base_.Spawn(rat);
        }
       StartCoroutine(DecideWhenToSpawn());
       
   }
}
