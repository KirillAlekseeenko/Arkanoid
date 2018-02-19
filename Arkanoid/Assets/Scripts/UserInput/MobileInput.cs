using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : IInputType {

	private int? frameCount;

	private float? timeSinceLastHandle;

	Dictionary<int, TouchInfo> touchesDictionary;

	private bool left;
	private bool right;
	private int swipeCounter;

	private void handleTouchInput()
	{
		if (frameCount.HasValue && frameCount == Time.frameCount)  // this function must be called only once per frame
			return;
		
		left = false;
		right = false;
		swipeCounter = 0;

		foreach (var touch in Input.touches) {
			switch (touch.phase) {
			case TouchPhase.Began:
				{
					touchesDictionary.Add (touch.fingerId, new TouchInfo(touch));
					break;
				}
			case TouchPhase.Moved:
				{
					if (touchesDictionary.ContainsKey (touch.fingerId)) {
						if (touchesDictionary [touch.fingerId].isSwipe(touch)) {
							swipeCounter++;
						} else {
							handleStaticTouch (touch);
						}
					}
					break;
				}
			case TouchPhase.Stationary:
				{
					handleStaticTouch (touch);
					break;
				}
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				{
					touchesDictionary.Remove (touch.fingerId);
					break;
				}
			}
		}
		frameCount = Time.frameCount;
	}

	private void handleStaticTouch(Touch touch)
	{
		if (Camera.main.ScreenToWorldPoint (touch.position).x > 0)
			right = true;
		else
			left = true;
	}


	#region IInputType implementation

	public bool Left {
		get {
			handleTouchInput ();
			return left;
		}
	}

	public bool Right {
		get {
			handleTouchInput ();
			return right;
		}
	}

	public bool SpecialAction {
		get {
			handleTouchInput ();
			return swipeCounter == 1;
		}
	}

	public bool Pause {
		get {
			handleTouchInput ();
			return swipeCounter == 2;
		}
	}

	public bool Start {
		get {
			handleTouchInput ();
			return swipeCounter == 1;
		}
	}

	#endregion

	private struct TouchInfo
	{
		private const float swipeDistanceInInches = 0.7f;
		private const float swipeTime = 0.5f;

		private Vector2 firstPosition;
		private float time;

		public bool isSwipe(Touch updatedTouch)
		{
			time += updatedTouch.deltaTime;
			return (Vector2.Distance (updatedTouch.position, firstPosition) >= swipeDistanceInInches * Screen.dpi) && (time <= swipeTime);
		}

		public TouchInfo(Touch touch)
		{
			firstPosition = touch.position;
			time = 0;
		}
	}
}
