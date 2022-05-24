using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenuAttribute(fileName = "Era")]
public class Era : ScriptableObject
{
    public List<Rat> Rats = new List<Rat>();
    public List<TurretInfo> Turrets = new List<TurretInfo>();

}
