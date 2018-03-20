using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonInput : MonoBehaviour {

	public delegate void InputEvent();

	public static event InputEvent SpecialAction;
	public static event InputEvent StartGame;
	public static event InputEvent Pause;

	protected IInputType input;

	void Start()
	{
		chooseInputType ();
	}

	void Update()
	{
		platformControlInput ();
		commonInput ();
	}

	protected abstract void platformControlInput ();

	protected void chooseInputType ()
	{
		#if UNITY_STANDALONE || UNITY_EDITOR

		input = new KeyboardInput();

		#elif UNITY_IOS || UNITY_ANDROID

		input = new MobileInput();

		#endif
	}

	private void commonInput()
	{
		if (input.SpecialAction && SpecialAction != null) {
			SpecialAction ();
		}
		if (input.SpecialAction && StartGame != null) {
			StartGame ();
		}
		if (input.Pause && Pause != null) {
			Pause ();
		}
	}
}
