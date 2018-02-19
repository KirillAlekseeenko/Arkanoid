using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsUIHandler : MonoBehaviour {

	[SerializeField] private Slider audioSlider;
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider effectsSlider;

	private void Start()
	{
		fillSettings ();
	}

	public void OnBackButton()
	{
		SaveUtils.SettingsSaveUtility.SaveSettings (new Settings (audioSlider.value, musicSlider.value, effectsSlider.value));
		var refs = GetComponent<ReferencesUIHandler> ();
		AnimationUtils.MakePanelTransition (refs.SettingsPanel, refs.MainPanel);
	}

	public void OnAudioValueChanged(float value)
	{
		AudioManager.Instance.SetAudioVolume (value);
	}

	public void OnMusicValueChanged(float value)
	{
		AudioManager.Instance.SetMusicVolume (value);
	}

	public void OnEffectsValueChanged(float value)
	{
		AudioManager.Instance.SetEffectsVolume (value);
	}

	private void fillSettings()
	{
		var settings = SaveUtils.SettingsSaveUtility.LoadSettings ();

		audioSlider.value = settings.AudioVolume;
		musicSlider.value = settings.MusicVolume;
		effectsSlider.value = settings.EffectsVolume;

		AudioManager.Instance.SetMusicVolume (settings.MusicVolume);
		AudioManager.Instance.SetEffectsVolume (settings.EffectsVolume);
		AudioManager.Instance.SetAudioVolume (settings.AudioVolume);

		audioSlider.onValueChanged.AddListener (OnAudioValueChanged);
		musicSlider.onValueChanged.AddListener (OnMusicValueChanged);
		effectsSlider.onValueChanged.AddListener (OnEffectsValueChanged);
	}
}
