using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IInputType {
	
	#region IInputType implementation

	public bool Left { get { return Input.GetKey (KeyCode.A); } }
	public bool Right {	get { return Input.GetKey (KeyCode.D); } }
	public bool SpecialAction { get { return Input.GetKeyDown (KeyCode.Space); } }
	public bool Pause { get { return Input.GetKeyDown (KeyCode.Escape); } }
	public bool Start { get { return Input.GetKeyDown (KeyCode.Space); } }

	#endregion
}
