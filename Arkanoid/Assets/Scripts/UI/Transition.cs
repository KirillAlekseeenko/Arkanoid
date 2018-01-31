using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour {

	[SerializeField] private Image blackImage;
	[SerializeField] private int transitionSteps;
	[SerializeField] private float transitionTime;

	private float step;
	private float coroutineframeTime;

	private void Start()
	{
		step = 1.0f / transitionSteps;
		coroutineframeTime = transitionTime * step;
	}

	public IEnumerator ScreenFadeOut()
	{
		for (int i = 0; i <= transitionSteps; i++) {
			yield return new WaitForSeconds (coroutineframeTime);
			blackImage.color = new Color (blackImage.color.r, blackImage.color.g, blackImage.color.b, 1.0f - i * step);
		}
	}

	public IEnumerator ScreenFadeIn()
	{
		for (int i = 0; i <= transitionSteps; i++) {
			yield return new WaitForSeconds (coroutineframeTime);
			blackImage.color = new Color (blackImage.color.r, blackImage.color.g, blackImage.color.b, i * step);
		}
	}
}
