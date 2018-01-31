using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBonus : PlatformBonus {
	
	public override void GetBonus (Platform platform)
	{
		addBonusToThePlatform<StickyPlatform> (platform);
	}
}
