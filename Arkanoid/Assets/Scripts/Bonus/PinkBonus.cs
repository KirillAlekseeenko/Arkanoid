using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkBonus : Bonus {

	public static event BonusAcquired PinkBonusAcquired;
	
	public override void GetBonus (Platform platform)
	{
		PinkBonusAcquired ();
	}
}
