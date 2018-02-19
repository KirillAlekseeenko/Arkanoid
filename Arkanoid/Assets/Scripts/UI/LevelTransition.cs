using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour {

	[SerializeField] private Image blackImage;
	[SerializeField] private int transitionSteps;

	private float step;
	private float coroutineframeTime;

	private void Start()
	{
		step = 1.0f / transitionSteps;
	}

	public IEnumerator ScreenFadeOut()
	{
		for (int i = 0; i <= transitionSteps; i++) {
			yield return null;
			blackImage.color = new Color (blackImage.color.r, blackImage.color.g, blackImage.color.b, 1.0f - i * step);
		}
	}

	public IEnumerator ScreenFadeIn()
	{
		for (int i = 0; i <= transitionSteps; i++) {
			yield return null;
			blackImage.color = new Color (blackImage.color.r, blackImage.color.g, blackImage.color.b, i * step);
		}
	}
}
