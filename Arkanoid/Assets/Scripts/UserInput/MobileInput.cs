using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileInput : IDescreteInputType, IAnalogInputType {

	private int? frameCount;

	private float? timeSinceLastHandle;

	Dictionary<int, TouchInfo> touchesDictionary;
	Dictionary<int, int> stationaryFingerIDs;

	private bool left;
	private bool right;
	private int swipeCounter;

	public MobileInput()
	{
		touchesDictionary = new Dictionary<int, TouchInfo> ();
		stationaryFingerIDs = new Dictionary<int, int> (2);
	}

	private void handleDiscreteInput()
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
						if ((!stationaryFingerIDs.ContainsKey(touch.fingerId) || stationaryFingerIDs[touch.fingerId] <= 2)) {
							if(touchesDictionary[touch.fingerId].isSwipe(touch))
								swipeCounter++;
						} else {
							handleStaticTouch (touch);
						}
					}
					break;
				}
			case TouchPhase.Stationary:
				{
					if(!stationaryFingerIDs.ContainsKey(touch.fingerId))
					{
						stationaryFingerIDs.Add(touch.fingerId, 1);
					}
					else
					{
						stationaryFingerIDs[touch.fingerId]++;
					}

					handleStaticTouch (touch);

					break;
				}
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				{
					stationaryFingerIDs.Remove (touch.fingerId);
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

	public bool SpecialAction {
		get {
			handleDiscreteInput ();
			return swipeCounter == 1;
		}
	}

	public bool Pause {
		get {
			return false;
		}
	}

	public bool Start {
		get {
			handleDiscreteInput ();
			return swipeCounter == 1;
		}
	}

	#endregion

	#region IDiscreteInputType

	public bool Left {
		get {
			handleDiscreteInput ();
			return left;
		}
	}

	public bool Right {
		get {
			handleDiscreteInput ();
			return right;
		}
	}

	#endregion

	public bool Movement (out Vector3 pos)
	{
		if (Input.touchCount > 0) {
			foreach (var touch in Input.touches) {
				if (touch.position.y < Screen.height / 4) {
					pos = Camera.main.ScreenToWorldPoint (touch.position);
					return true;
				}
			}
		}
		pos = Vector3.zero;
		return false;
	}

	private class TouchInfo
	{
		private const float swipeDistanceInInches = 0.5f;
		private const float swipeTime = 0.5f;

		private Vector2 firstPosition;
		private float time;

		public bool isSwipe(Touch updatedTouch)
		{
			time += updatedTouch.deltaTime;
			var result = (Vector2.Distance (updatedTouch.position, firstPosition) >= swipeDistanceInInches * Screen.dpi) && (time <= swipeTime);
			if (result) {
				firstPosition = updatedTouch.position;
				time = swipeTime * 0.9f;
			}
			return result;
		}

		public TouchInfo(Touch touch)
		{
			firstPosition = touch.position;
			time = 0;
		}
	}
}
