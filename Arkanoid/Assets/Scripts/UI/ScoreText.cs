using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : Text {

	private const float maxScale = 1.75f;
	private const int scaleFrameCount = 20;

	private float _scale = 1;

	private float scale
	{
		get {
			return _scale;
		}
		set {
			_scale = value;
			rectTransform.localScale = Vector3.one * Mathf.Clamp (_scale, 1, maxScale);
		}
	}
	
	public override string text {
		get {
			return base.text;
		}
		set {
			base.text = value;
			StartCoroutine (scoreScaleUpAndDown ());
		}
	}

	private IEnumerator scoreScaleUpAndDown()
	{
		const float scaleStep = (maxScale - 1) / scaleFrameCount;
		for (int i = -scaleFrameCount; i < scaleFrameCount; i++) {
			scale -= scaleStep * Mathf.Sign (i);
			yield return null;
		}
	}
}
