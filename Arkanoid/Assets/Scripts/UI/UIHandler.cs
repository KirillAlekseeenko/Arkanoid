using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour {

	public delegate void ButtonClicked();
	public static event ButtonClicked ResumeButtonClicked;
	public static event ButtonClicked RetryButtonClicked;
	public static event ButtonClicked MainMenuButtonClicked;
	public static event ButtonClicked PauseButtonClicked;

	[SerializeField] private ScoreText scoreLabel;
	[SerializeField] private Text pressBarLabel;
	[SerializeField] private Text levelNumberLabel;
	[SerializeField] private Text resultLabel;
	[SerializeField] private Text endGameScore;

	[SerializeField] private Button pauseButton;

	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject endGamePanel;
	[SerializeField] private GameObject lifePanel;

	[SerializeField] private Image lifeImagePrefab;

	private LevelTransition transition;

	void OnEnable()
	{
		GameManager.GameStartedEvent += OnStartGame;
		GameManager.SessionEndedEvent += OnEndGame;
		GameManager.GamePausedEvent += OnPauseGame;
		GameManager.GameResumedEvent += OnResumeGame;
		GameManager.LifeLostEvent += onLifeLost;
		GameManager.LevelBuiltEvent += OnResetGame;
		GameManager.AddLife += onLifeAdded;
		GameManager.LifeLostEvent += OnLevelEnd;
		GameManager.LevelPassedEvent += OnLevelEnd;
		GreyBonus.GreyBonusAcquired += onLifeAdded;
	}

	void OnDisable()
	{
		GameManager.GameStartedEvent -= OnStartGame;
		GameManager.SessionEndedEvent -= OnEndGame;
		GameManager.GamePausedEvent -= OnPauseGame;
		GameManager.GameResumedEvent -= OnResumeGame;
		GameManager.LifeLostEvent -= onLifeLost;
		GameManager.LevelBuiltEvent -= OnResetGame;
		GameManager.AddLife -= onLifeAdded;
		GameManager.LifeLostEvent -= OnLevelEnd;
		GameManager.LevelPassedEvent -= OnLevelEnd;
		GreyBonus.GreyBonusAcquired -= onLifeAdded;
	}

	private void Start()
	{
		transition = GetComponent<LevelTransition> ();
	}

	#region HUD methods

	public void UpdateScore(int newScore = 0)
	{
		scoreLabel.text = newScore.ToString ();
	}

	private void onLifeAdded()
	{
		StartCoroutine (addLife ());
	}

	private void onLifeLost()
	{
		StartCoroutine (removeLife ());
	}

	private IEnumerator addLife()
	{
		var newLifeImage = Instantiate (lifeImagePrefab, lifePanel.transform).gameObject;
		yield return blinkLife (newLifeImage);
	}

	private IEnumerator removeLife()
	{
		var lifeImageToRemove = lifePanel.transform.GetChild (lifePanel.transform.childCount - 1).gameObject;
		yield return blinkLife (lifeImageToRemove);
		Destroy(lifeImageToRemove);
	}

	private IEnumerator blinkLife(GameObject lifeImage)
	{
		const int blinkCount = 4;
		const float blinkTime = 1.0f;
		for (int i = 0; i < blinkCount; i++) {
			yield return new WaitForSecondsRealtime (blinkTime / (2 * blinkCount));
			lifeImage.SetActive (false);
			yield return new WaitForSecondsRealtime (blinkTime / (2 * blinkCount));
			lifeImage.SetActive (true);
		}
	}

	#endregion

	#region common game methods

	public void OnEndGame(GameResult result, Session session)
	{
		resultLabel.text = result == GameResult.WIN ? "WIN" : "LOST";
		endGameScore.text = "LEVEL " + session.LevelNumber + "\n\n" + session.Score + " points";
		AnimationUtils.MakeAnimatorVisible (endGamePanel.GetComponent<Animator>());
	}

	public void OnStartGame()
	{
		AnimationUtils.MakeAnimatorInvisible (pressBarLabel.GetComponent<Animator>());
		AnimationUtils.MakeAnimatorInvisible (levelNumberLabel.GetComponent<Animator> ());
		AnimationUtils.MakeAnimatorVisible (pauseButton.GetComponent<Animator> ());
	}

	public void OnResetGame(int levelNumber)
	{
		AnimationUtils.MakeAnimatorVisible (pressBarLabel.GetComponent<Animator>());
		levelNumberLabel.text = "LEVEL " + levelNumber.ToString ();
		AnimationUtils.MakeAnimatorVisible (levelNumberLabel.GetComponent<Animator> ());
	}

	public void OnLevelEnd()
	{
		AnimationUtils.MakeAnimatorInvisible (pauseButton.GetComponent<Animator> ());
	}

	public void OnPauseGame()
	{
		AnimationUtils.MakeAnimatorVisible (pausePanel.GetComponent<Animator>());
		AnimationUtils.MakeAnimatorInvisible (pauseButton.GetComponent<Animator> ());
	}

	public void OnResumeGame()
	{
		AnimationUtils.MakeAnimatorInvisible (pausePanel.GetComponent<Animator>());
		AnimationUtils.MakeAnimatorVisible (pauseButton.GetComponent<Animator> ());
	}

	#endregion

	#region screen control

	public IEnumerator ScreenFadeIn()
	{
		yield return transition.ScreenFadeIn ();
	}
	public IEnumerator ScreenFadeOut()
	{
		yield return transition.ScreenFadeOut ();
	}

	#endregion

	#region button callbacks

	public void onPauseButton()
	{
		PauseButtonClicked ();
	}

	public void OnMainMenuButton()
	{
		MainMenuButtonClicked ();	
	}
	public void OnRetryButton()
	{
		RetryButtonClicked ();
	}
	public void OnResumeButton()
	{
		ResumeButtonClicked ();
	}

	#endregion
}
