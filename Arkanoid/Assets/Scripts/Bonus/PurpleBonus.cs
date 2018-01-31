using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBonus : Bonus {

	public override void GetBonus (Platform platform)
	{
		GameManager.Instance.CreatePortal ();
	}
}
