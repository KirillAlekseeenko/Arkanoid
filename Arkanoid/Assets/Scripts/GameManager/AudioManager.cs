using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	private static AudioManager instance;

	[Header("Random pitch")]

	[SerializeField] private float minPitch;
	[SerializeField] private float maxPitch;

	[Header("Audio sources")]

	[SerializeField] private AudioSource effectSource;
	[SerializeField] private AudioSource musicSource;

	[Header("Audio clips")]

	[SerializeField] private AudioClip onHoverButtonClip;
	[SerializeField] private AudioClip onClickButtonClip;

	[SerializeField] private AudioClip onLifeLostClip;
	[SerializeField] private AudioClip onLevelPassedClip;

	[SerializeField] private AudioClip onBlockDestroyedClip;
	[SerializeField] private AudioClip onBlockHitClip;
	[SerializeField] private AudioClip onWallHitClip;
	[SerializeField] private AudioClip onPlatformHitClip;

	private float audioVolume;
	private float musicVolume;
	private float effectsVolume;

	public static AudioManager Instance { get { return instance; } }

	#region MonoBehaviour

	void OnEnable()
	{
		GameManager.LevelPassedEvent += PlayOnLevelPassedEffect;
		GameManager.LifeLostEvent += PlayOnLifeLostEffect;
		MovingEnemy.EnemyDestroyed += PlayOnBlockHitEffect;
		Block.BlockDestroyed += PlayOnBlockHitEffect;
	}

	void OnDisable()
	{
		GameManager.LevelPassedEvent -= PlayOnLevelPassedEffect;
		GameManager.LifeLostEvent -= PlayOnLifeLostEffect;
		MovingEnemy.EnemyDestroyed -= PlayOnBlockHitEffect;
		Block.BlockDestroyed -= PlayOnBlockHitEffect;
	}
		
	void Awake()
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

	public void PlayOnBlockHitEffect(IHittable hittable)
	{
		randomizePitch ();
		effectSource.PlayOneShot (onBlockDestroyedClip);
	}

	public void PlayOnStaticBlockHitEffect()
	{
		randomizePitch ();
		effectSource.PlayOneShot (onBlockHitClip);
	}

	public void PlayOnWallHitEffect()
	{
		randomizePitch ();
		effectSource.PlayOneShot (onWallHitClip);
	}

	public void PlayOnPlatformHitEffect()
	{
		randomizePitch ();
		effectSource.PlayOneShot (onPlatformHitClip);
	}

	#endregion

	#region volume settings

	public void SetAudioVolume(float value)
	{
		audioVolume = value;
		SetMusicVolume (musicVolume);
		SetEffectsVolume (effectsVolume);
	}

	public void SetMusicVolume(float value)
	{
		musicVolume = value;
		musicSource.volume = value * audioVolume;
	}

	public void SetEffectsVolume(float value)
	{
		effectsVolume = value;
		effectSource.volume = value * audioVolume;
	}

	#endregion

	private void randomizePitch()
	{
		effectSource.pitch = Random.Range (minPitch, maxPitch);
	}

}
