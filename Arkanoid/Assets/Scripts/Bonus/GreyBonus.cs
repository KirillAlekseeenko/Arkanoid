using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyBonus : Bonus {

	public override void GetBonus (Platform platform)
	{
		GameManager.Instance.AdditionalLife ();
	}
}
