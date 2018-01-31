using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIHandler : MonoBehaviour {

	void Start()
	{
		MainMenuUtils.MakePanelVisible (GetComponent<ReferencesUIHandler> ().MainPanel);
	}

	public void OnStartGameButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		MainMenuUtils.MakePanelTransition (refs.MainPanel, refs.NamePanel);
	}
	public void OnRecordsGameButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		MainMenuUtils.MakePanelTransition (refs.MainPanel, refs.RecordsPanel);
	}
	public void OnSettingsGameButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		MainMenuUtils.MakePanelTransition (refs.MainPanel, refs.SettingsPanel);
	}
	public void OnQuitButtonDown()
	{
		Application.Quit ();
	}
}
	
