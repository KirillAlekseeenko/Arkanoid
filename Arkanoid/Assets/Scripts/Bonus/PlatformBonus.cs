using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformBonus : Bonus {

	protected void addBonusToThePlatform<T> (Platform platform) where T : PlatformBonusComponent
	{
		PlatformBonusComponent currentBonusComponent = platform.gameObject.GetComponent<PlatformBonusComponent> ();
		if (currentBonusComponent == null) {
			platform.gameObject.AddComponent<T> ();
		} else {
			if (!(currentBonusComponent is T)) {
				currentBonusComponent.RemoveBonus ();
				platform.gameObject.AddComponent<T> ();
			}
		}
	}
}
