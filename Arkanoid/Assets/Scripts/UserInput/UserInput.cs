using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {

	public delegate void InputEvent();
	public static event InputEvent MoveLeft;
	public static event InputEvent MoveRight;
	public static event InputEvent SpecialAction;
	public static event InputEvent StartGame;
	public static event InputEvent Pause;

	[SerializeField] private Platform platform;

	private IInputType input;

	void Start()
	{
		chooseInputType ();
	}

	void Update()
	{
		platformControlInput ();
		startGameInput ();
		pauseInput ();
	}

	private void chooseInputType()
	{
		#if UNITY_STANDALONE || UNITY_EDITOR

		input = new KeyboardInput();

		#endif

		#if UNITY_IOS

		inputSource = new MobileInput();

		#endif

	}

	private void platformControlInput()
	{
		if (input.Left && MoveLeft != null) {
			MoveLeft ();
		}
		if (input.Right && MoveRight != null) {
			MoveRight ();
		}
		if (input.SpecialAction && SpecialAction != null) {
			SpecialAction ();
		}
	}

	private void startGameInput()
	{
		if (input.Start && StartGame != null) {
			StartGame ();
		}
	}

	private void pauseInput()
	{
		if (input.Pause && Pause != null) {
			Pause ();
		}
	}
}
	
	