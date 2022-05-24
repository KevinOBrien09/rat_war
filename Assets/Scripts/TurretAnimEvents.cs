using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAnimEvents : MonoBehaviour
{
    [SerializeField] Turret turret;

    public void Shoot()
    {
        turret.Shoot();
    }
}
