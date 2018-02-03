using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	private static AudioManager instance;

	public static AudioManager Instance { get { return instance; } }

	[Header("Parameters")]

	[SerializeField] private float minPitch;
	[SerializeField] private float maxPitch;

	[Header("AudioSources")]

	[SerializeField] private AudioSource effectSource;
	[SerializeField] private AudioSource musicSource;

	[Header("Audio Clips")]

	[SerializeField] private AudioClip onHoverButtonClip;
	[SerializeField] private AudioClip onClickButtonClip;

	[SerializeField] private AudioClip onLifeLostClip;
	[SerializeField] private AudioClip onLevelPassedClip;

	[SerializeField] private AudioClip onBlockHitClip;
	[SerializeField] private AudioClip onWallHitClip;

	private float musicVolume;
	private float effectsVolume;

	#region MonoBehaviour

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	private void Start()
	{
		effectSource = transform.Find ("EffectsSource").GetComponent<AudioSource>();
		musicSource = transform.Find ("MusicSource").GetComponent<AudioSource>();
	}

	#endregion

	#region play audio clips

	public void PlayOnHoverButtonEffect()
	{
		effectSource.PlayOneShot (onHoverButtonClip);
	}

	public void PlayOnClickButtonEffect()
	{
		effectSource.PlayOneShot (onClickButtonClip);
	}

	public void PlayOnLifeLostEffect()
	{
		effectSource.PlayOneShot (onLifeLostClip);
	}

	public void PlayOnLevelPassedEffect()
	{
		effectSource.PlayOneShot (onLevelPassedClip);
	}

	public void PlayOnBlockHitEffect()
	{
		randomizePitch ();
		effectSource.PlayOneShot (onBlockHitClip);
	}

	public void PlayOnWallHitEffect()
	{
		randomizePitch ();
		effectSource.PlayOneShot (onWallHitClip);
	}

	#endregion

	#region volume settings

	public void SetAudioVolume(float value)
	{
		musicSource.volume = musicVolume * value;
		effectSource.volume = effectsVolume * value;
	}

	public void SetMusicVolume(float value)
	{
		musicVolume = value;
		musicSource.volume = value;
	}

	public void SetEffectsVolume(float value)
	{
		effectsVolume = value;
		effectSource.volume = value;
	}

	#endregion

	private void randomizePitch()
	{
		effectSource.pitch = Random.Range (minPitch, maxPitch);
	}

}
