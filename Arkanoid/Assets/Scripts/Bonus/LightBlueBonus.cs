using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlueBonus : Bonus {

	public static event BonusAcquired LightBlueBonusAcquired;
	
	public override void GetBonus (Platform platform)
	{
		LightBlueBonusAcquired ();
	}
}
