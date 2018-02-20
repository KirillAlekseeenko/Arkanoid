using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public delegate void CommonGameEvents();
	public static event CommonGameEvents GameStartedEvent;
	public static event CommonGameEvents GamePausedEvent;
	public static event CommonGameEvents GameResumedEvent;
	public static event CommonGameEvents LifeLostEvent;
	public static event CommonGameEvents LevelPassedEvent;
	public static event CommonGameEvents LevelCleanedEvent;

	public delegate void LevelEvents (int levelNumber);
	public static event LevelEvents LevelBuiltEvent;

	public delegate void SessionResult(GameResult result, Session session);
	public static event SessionResult SessionEndedEvent;

	[Header("UI")]

	[SerializeField] private UIHandler UI;

	[Header("Balance")]

	[SerializeField] private int initialLifeCount;

	private int levelNumber;

	private int lifeCount;
	private string playerName;
	private int score;

	private bool isPaused = false;

	#region MonoBehaviour

	void Awake()
	{
		lifeCount = initialLifeCount;
		levelNumber = 1;
		score = 0;
	}

	void OnEnable()
	{
		UserInput.Pause += onPauseGameInput;
		UserInput.StartGame += onStartGameInput;
		MovingEnemy.EnemyDestroyed += HittableDestroyed;
		Block.BlockDestroyed += HittableDestroyed;
		Enemy.PlatformDestroyed += onLifeLost;
		UIHandler.ResumeButtonClicked += onPauseGameInput;
		UIHandler.MainMenuButtonClicked += OnMainMenuButtonClick;
		UIHandler.RetryButtonClicked += onRetryButtonClick;
		GreyBonus.GreyBonusAcquired += AdditionalLife;
		SpheresManager.AllSpheresLost += onLifeLost;
		LevelManager.LevelIsClear += onLevelPassed;
	}

	void OnDisable()
	{
		UserInput.Pause -= onPauseGameInput;
		UserInput.StartGame -= onStartGameInput;
		MovingEnemy.EnemyDestroyed -= HittableDestroyed;
		Block.BlockDestroyed -= HittableDestroyed;
		Enemy.PlatformDestroyed -= onLifeLost;
		UIHandler.ResumeButtonClicked -= onPauseGameInput;
		UIHandler.MainMenuButtonClicked -= OnMainMenuButtonClick;
		UIHandler.RetryButtonClicked -= onRetryButtonClick;
		GreyBonus.GreyBonusAcquired -= AdditionalLife;
		SpheresManager.AllSpheresLost -= onLifeLost;
		LevelManager.LevelIsClear -= onLevelPassed;
	}

	void Start()
	{
		playerName = SaveUtils.PlayerNameSaveUtility.LoadName ();
		LevelBuiltEvent (levelNumber);
	}

	#endregion

	#region reset game

	private void resetLevel(GameResult result) 
	{
		StartCoroutine (resetLevelCoroutine (result));
	}

	private IEnumerator resetLevelCoroutine(GameResult result)
	{
		UserInput.Pause -= onPauseGameInput;
		Time.timeScale = 0;

		yield return new WaitForSecondsRealtime (1.0f);

		if (result == GameResult.WIN || result == GameResult.LOST) {
			SessionEndedEvent (result, new Session(playerName, score));
			yield break;
		}

		yield return UI.ScreenFadeIn ();

		LevelCleanedEvent ();
		if(result == GameResult.NEXTLEVEL)
			levelNumber++;
		LevelBuiltEvent (levelNumber);

		yield return UI.ScreenFadeOut ();

		Time.timeScale = 1;
		UserInput.StartGame += onStartGameInput;
		UserInput.Pause += onPauseGameInput;
	}

	#endregion

	#region end game

	private void onLifeLost()
	{
		LifeLostEvent ();
		lifeCount--;
		if (lifeCount == 0)
			resetLevel (GameResult.LOST);
		else
			resetLevel (GameResult.RESETLEVEL);
	}

	private void onLevelPassed(bool isLast)
	{
		LevelPassedEvent ();
		if (isLast)
			resetLevel (GameResult.WIN);
		else
		    resetLevel (GameResult.NEXTLEVEL);
	}

	private void saveResults()
	{
		var records = SaveUtils.RecordsSaveUtility.LoadRecords ();
		records.AddSession (new Session(playerName, score));
		SaveUtils.RecordsSaveUtility.SaveRecords (records);
	}

	#endregion

	#region common game events

	private void onStartGameInput()
	{
		UserInput.StartGame -= onStartGameInput;
		GameStartedEvent ();
	}

	private void onPauseGameInput()
	{
		if (isPaused)
			resumeGame ();
		else
			pauseGame();
	}

	private void pauseGame()
	{
		isPaused = true;
		Time.timeScale = 0;
		GamePausedEvent ();
	}

	private void resumeGame ()
	{
		isPaused = false;
		Time.timeScale = 1;
		GameResumedEvent ();
	}

	private void onRetryButtonClick()
	{
		saveResults ();
		Time.timeScale = 1;
		SceneManager.LoadScene ("GameScene");
	}

	private void OnMainMenuButtonClick ()
	{
		saveResults ();
		Time.timeScale = 1;
		SceneManager.LoadScene ("MainMenu");
	}

	#endregion

	#region score and life

	private void HittableDestroyed(IHittable hittable)
	{
		score += hittable.RewardPoints;
		UI.UpdateScore (score);
	}

	private void AdditionalLife()
	{
		lifeCount++;
	}

	#endregion
}

public enum GameResult { WIN, LOST, RESETLEVEL, NEXTLEVEL };
