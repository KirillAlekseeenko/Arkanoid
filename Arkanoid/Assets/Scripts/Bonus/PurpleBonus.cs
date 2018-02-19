using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBonus : Bonus {

	public static event BonusAcquired PurpleBonusAcquired;

	public override void GetBonus (Platform platform)
	{
		PurpleBonusAcquired ();
	}
}
