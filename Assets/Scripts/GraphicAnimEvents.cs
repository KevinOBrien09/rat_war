using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicAnimEvents : MonoBehaviour
{
	[SerializeField] Unit unit;

	public void Attack()
	{
		unit.Attack();
	}

	public void l1()
	{
		unit.Leap1();
	}

	public void l2()
	{
		unit.Leap2();
	}

  
  
}
