using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkBonus : Bonus {
	
	public override void GetBonus (Platform platform)
	{
		GameManager.Instance.SlowSphereDown ();
	}
}
