using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBonus : PlatformBonus {

	public override void GetBonus (Platform platform)
	{
		addBonusToThePlatform<Gun> (platform);
	}
}