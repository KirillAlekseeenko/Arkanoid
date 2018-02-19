using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyBlock : DynamicBlock {

	[SerializeField] protected int initialCounter;

	protected int counter;

	protected void Awake()
	{
		counter = initialCounter;
	}

	public override void Hit ()
	{
		counter--;
		if (counter > 0) {
			AudioManager.Instance.PlayOnStaticBlockHitEffect ();
			illumination ();
		} else {
			base.Hit ();
		}
	}
}
