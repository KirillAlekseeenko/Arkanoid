using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBlueBonus : PlatformBonus {

	public override void GetBonus (Platform platform)
	{
		addBonusToThePlatform<DoubleLength> (platform);
	}
}
