using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IDescreteInputType, IAnalogInputType {
	
	#region IInputType implementation

	public bool SpecialAction { get { return Input.GetKeyDown (KeyCode.Space); } }
	public bool Start { get { return Input.GetKeyDown (KeyCode.Space); } }

	#endregion

	public bool Left { get { return Input.GetKey (KeyCode.A); } }
	public bool Right {	get { return Input.GetKey (KeyCode.D); } }

	public bool Movement (out Vector3 pos)
	{
		pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		return true;
	}

	public bool Pause {
		get {
			return Input.GetKeyDown (KeyCode.Escape);
		}
	}
}
