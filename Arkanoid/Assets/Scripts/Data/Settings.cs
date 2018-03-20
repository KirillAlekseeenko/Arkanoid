using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
{
	[SerializeField] private float audioVolume;
	[SerializeField] private float musicVolume;
	[SerializeField] private float effectsVolume;

	[SerializeField] private bool analogInputOn;

	public Settings()
	{
		audioVolume = 1.0f;
		musicVolume = 1.0f;
		effectsVolume = 1.0f;
		analogInputOn = false;
	}

	public Settings(float audioVolume, float musicVolume, float effectsVolume, bool analogInputOn)
	{
		this.audioVolume = audioVolume;
		this.musicVolume = musicVolume;
		this.effectsVolume = effectsVolume;
		this.analogInputOn = analogInputOn;
	}

	public float AudioVolume { get { return audioVolume; } }
	public float MusicVolume { get { return musicVolume; } }
	public float EffectsVolume { get { return effectsVolume; } }

	public bool isAnalogInputOn { get { return analogInputOn; } }
}
