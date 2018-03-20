using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogInput : CommonInput {

	public delegate void AnalogInputEvent(Vector3 pos);
	public static event AnalogInputEvent Move;

	#region implemented abstract members of Input
	protected override void platformControlInput ()
	{
		var analogInput = input as IAnalogInputType;
		Vector3 pos;
		if (analogInput.Movement (out pos) && Move != null)
			Move (pos);
	}
	#endregion
}
