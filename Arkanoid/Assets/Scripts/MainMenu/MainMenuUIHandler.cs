using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIHandler : MonoBehaviour {

	void Start()
	{
		Application.targetFrameRate = 60;
		AnimationUtils.MakeAnimatorVisible (GetComponent<ReferencesUIHandler> ().MainPanel);
	}

	public void OnStartGameButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		AnimationUtils.MakePanelTransition (refs.MainPanel, refs.NamePanel);
	}
	public void OnRecordsGameButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		AnimationUtils.MakePanelTransition (refs.MainPanel, refs.RecordsPanel);
	}
	public void OnSettingsGameButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		AnimationUtils.MakePanelTransition (refs.MainPanel, refs.SettingsPanel);
	}
	public void OnInfoGameButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		AnimationUtils.MakePanelTransition (refs.MainPanel, refs.InfoPanel);
	}
}
	
