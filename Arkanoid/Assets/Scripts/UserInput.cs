using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {

	[SerializeField] private Platform platform;

	private void Update()
	{
		if (GameManager.Instance.IsStarted && !GameManager.Instance.IsEnded) {
			getControlInput ();
		} else {
			if (!GameManager.Instance.IsStarted && !GameManager.Instance.Transition) {
				if (Input.GetKeyDown (KeyCode.Space)) {
					GameManager.Instance.IsStarted = true;
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			GameManager.Instance.PauseHandle ();
		}
	}

	private void getControlInput()
	{
		if (Input.GetKey (KeyCode.A)) {
			platform.MoveLeft ();
		}
		if (Input.GetKey (KeyCode.D)) {
			platform.MoveRight ();
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			platform.SpecialAction ();
		}
	}

}
	