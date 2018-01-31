using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformBonus : Bonus {

	protected void addBonusToThePlatform<T> (Platform platform) where T : PlatformBonusComponent
	{
		PlatformBonusComponent bonusComponent = platform.gameObject.GetComponent<PlatformBonusComponent > ();
		if (bonusComponent == null) {
			platform.gameObject.AddComponent<T> ();
		} else {
			if (!(bonusComponent is T)) {
				bonusComponent.RemoveBonus ();
				platform.gameObject.AddComponent<T> ();
			}
		}
	}
}
