using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	void Start () {
		adaptCameraToTheScreenResolution ();
	}

	private void adaptCameraToTheScreenResolution()
	{
		var rectWidth = GameField.Width / (Camera.main.aspect * Camera.main.orthographicSize * 2);
		var rectX = (1.0f - rectWidth) / 2;
		Camera.main.rect = new Rect (rectX, 0, rectWidth, 1);
	}
}
