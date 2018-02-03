using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	private float xRightWallPosition;
	private float xLeftWallPosition;

	void Start () {
		xRightWallPosition = GameManager.Instance.CommonReferences.RightWallPosition.x;
		xLeftWallPosition = GameManager.Instance.CommonReferences.LeftWallPosition.x;
		adaptCameraToTheScreenResolution ();
	}

	private void adaptCameraToTheScreenResolution()
	{
		var width = xRightWallPosition / (Camera.main.aspect * Camera.main.orthographicSize);
		var x = (1.0f - width) / 2;
		Camera.main.rect = new Rect (x, 0, width, 1);
	}
}
