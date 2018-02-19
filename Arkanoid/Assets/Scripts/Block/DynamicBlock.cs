using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBlock : Block {

	public override void Hit ()
	{
		this.destroy ();
	}
}
