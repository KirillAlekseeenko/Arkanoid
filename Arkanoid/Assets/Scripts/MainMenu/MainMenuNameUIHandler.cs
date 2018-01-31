using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuNameUIHandler : MonoBehaviour {

	[SerializeField] private InputField nameField;

	public void OnConfirmButton()
	{
		GameManager.CurrentSession = new SaveUtils.Session (nameField.text);
		SceneManager.LoadScene ("GameScene");
	}
	public void OnBackButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		MainMenuUtils.MakePanelTransition (refs.NamePanel, refs.MainPanel);
	}
}
