using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteInput : CommonInput {

	public delegate void DiscreteInputEvent();

	public static event DiscreteInputEvent MoveLeft;
	public static event DiscreteInputEvent MoveRight;

	protected override void platformControlInput()
	{
		var discreteInput = input as IDescreteInputType;
		
		if (discreteInput.Left && MoveLeft != null) {
			MoveLeft ();
		}
		if (discreteInput.Right && MoveRight != null) {
			MoveRight ();
		}
	}
}
	
	