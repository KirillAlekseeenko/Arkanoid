using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {

	void OnEnable()
	{
		UIHandler.MainMenuButtonClicked += showAds;
		UIHandler.RetryButtonClicked += showAds;
	}

	void OnDisable()
	{
		UIHandler.MainMenuButtonClicked -= showAds;
		UIHandler.RetryButtonClicked -= showAds;
	}

	private void showAds()
	{
		#if UNITY_IOS || UNITY_ANDROID

		Advertisement.Show ("video");

		#endif
	}
}
