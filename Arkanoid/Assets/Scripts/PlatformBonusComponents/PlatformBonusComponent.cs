﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformBonusComponent : MonoBehaviour {

	public abstract void RemoveBonus ();

	public virtual void SpecialAction ()
	{
		Debug.LogWarning ("Platform hasn't any special action");
	}
}