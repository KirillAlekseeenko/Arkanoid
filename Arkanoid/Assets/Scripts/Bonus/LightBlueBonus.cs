using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlueBonus : Bonus {
	
	public override void GetBonus (Platform platform)
	{
		GameManager.Instance.SpawnSpheres ();
	}
}
