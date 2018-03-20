using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LearningManager : MonoBehaviour {

	private bool isAnalogInputOn;

	[SerializeField] private Image discreteControls;
	[SerializeField] private Image analogControls;

	[SerializeField] private Image stickyControls;

	void OnEnable()
	{
		Platform.SphereSticked += showStickyControls;
		GameManager.GameStartedEvent += showControls;
	}

	void OnDisable()
	{
		Platform.SphereSticked -= showStickyControls;
		GameManager.GameStartedEvent -= showControls;

		CommonInput.SpecialAction -= onStickyInput;
		DiscreteInput.MoveLeft -= onDiscreteInput;
		DiscreteInput.MoveRight -= onDiscreteInput;
		AnalogInput.Move -= onAnalogInput;
	}

	void Start () 
	{
		isAnalogInputOn = SaveUtils.SettingsSaveUtility.LoadSettings ().isAnalogInputOn;
	}

	private void showControls()
	{
		GameManager.GameStartedEvent -= showControls;

		if (isAnalogInputOn) {
			showControls (analogControls);
		} else {
			showControls(discreteControls);
		}

		StartCoroutine(executeAfterDelay (0.3f, () =>
			{
				DiscreteInput.MoveLeft += onDiscreteInput;
				DiscreteInput.MoveRight += onDiscreteInput;
				AnalogInput.Move += onAnalogInput;
			}));
	}

	private void showStickyControls()
	{
		showControls (stickyControls);
		Platform.SphereSticked -= showStickyControls;
		CommonInput.SpecialAction += onStickyInput;
	}

	private void showControls(Image controls)
	{
		AnimationUtils.MakeAnimatorVisible (controls.GetComponent<Animator> ());
		Time.timeScale = 0;
	}

	private void hideControls(Image controls)
	{
		AnimationUtils.MakeAnimatorInvisible (controls.GetComponent<Animator> ());
		Time.timeScale = 1;
	}

	private void onDiscreteInput()
	{
		hideControls (discreteControls);
		DiscreteInput.MoveLeft -= onDiscreteInput;
		DiscreteInput.MoveRight -= onDiscreteInput;
	}

	private void onAnalogInput(Vector3 pos)
	{
		hideControls (analogControls);
		AnalogInput.Move -= onAnalogInput;
	}

	private void onStickyInput()
	{
		hideControls (stickyControls);
		CommonInput.SpecialAction -= onStickyInput;
	}

	private IEnumerator executeAfterDelay(float delay, Action task)
	{
		yield return new WaitForSecondsRealtime (delay);
		task ();
	}
}
