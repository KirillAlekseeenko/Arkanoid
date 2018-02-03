using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour {

	[SerializeField] private Text scoreLabel;
	[SerializeField] private Text pressBarLabel;
	[SerializeField] private Text levelNumberLabel;
	[SerializeField] private Text resultLabel;
	[SerializeField] private Text endGameScore;

	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject endGamePanel;
	[SerializeField] private GameObject lifePanel;

	[SerializeField] private Image lifeImagePrefab;

	private Transition transition;

	private void Start()
	{
		transition = GetComponent<Transition> ();
		UpdateScore ();
	}

	#region HUD methods

	public void UpdateScore(int newScore = 0)
	{
		scoreLabel.text = "SCORE: " + newScore.ToString ();
	}

	public void SetLife(int lifeCount)
	{
		foreach (Transform image in lifePanel.transform) {
			Destroy (image.gameObject);
		}
		for (int i = 0; i < lifeCount; i++) {
			Instantiate (lifeImagePrefab, lifePanel.transform);
		}
	}

	public void AddLife()
	{
		Instantiate (lifeImagePrefab, lifePanel.transform);
	}

	public void RemoveLife()
	{
		Destroy(lifePanel.transform.GetChild (lifePanel.transform.childCount - 1).gameObject);
	}

	#endregion

	#region common game methods

	public void OnEndGame(bool result)
	{
		resultLabel.text = result? "WIN" : "LOST";
		endGameScore.text = GameManager.CurrentSession.Name + " - " + GameManager.CurrentSession.Score + " points";
		endGamePanel.GetComponent<Animator> ().SetBool ("Visible", true);
	}

	public void OnStartGame()
	{
		pressBarLabel.gameObject.GetComponent<Animator> ().SetBool ("Visible", false);
		levelNumberLabel.gameObject.GetComponent<Animator> ().SetBool ("Visible", false);
	}

	public void OnResetGame(int levelNumber)
	{
		pressBarLabel.gameObject.GetComponent<Animator> ().SetBool ("Visible", true);
		levelNumberLabel.text = "LEVEL " + levelNumber.ToString ();
		levelNumberLabel.gameObject.GetComponent<Animator> ().SetBool ("Visible", true);
	}

	public void OnPauseGame()
	{
		pausePanel.GetComponent<Animator> ().SetBool ("Visible", true);
	}

	public void OnResumeGame()
	{
		pausePanel.GetComponent<Animator> ().SetBool ("Visible", false);
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

	public void OnMainMenuButton()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene ("MainMenu");
	}
	public void OnRetryButton()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene ("GameScene");
	}
	public void OnResumeButton()
	{
		GameManager.Instance.PauseHandle ();
	}

	#endregion
}
