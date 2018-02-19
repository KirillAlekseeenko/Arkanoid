using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyBonus : Bonus {

	public static event BonusAcquired GreyBonusAcquired;

	public override void GetBonus (Platform platform)
	{
		GreyBonusAcquired ();
	}
}
