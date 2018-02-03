using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuNameUIHandler : MonoBehaviour {

	[SerializeField] private InputField nameField;

	private void Start()
	{
		fillNameField ();
	}

	private void fillNameField()
	{
		var name = SaveUtils.PlayerNameSaveUtility.LoadName ();
		if (name != null) {
			nameField.text = name;
		}
	}
		
	public void OnConfirmButton()
	{
		SaveUtils.PlayerNameSaveUtility.SaveName (nameField.text);
		GameManager.CurrentSession = new Session (nameField.text);
		SceneManager.LoadScene ("GameScene");
	}

	public void OnBackButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		MainMenuUtils.MakePanelTransition (refs.NamePanel, refs.MainPanel);
	}
}
