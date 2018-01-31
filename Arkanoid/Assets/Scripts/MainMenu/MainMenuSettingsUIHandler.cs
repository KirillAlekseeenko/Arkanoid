using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSettingsUIHandler : MonoBehaviour {

	public void OnBackButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		MainMenuUtils.MakePanelTransition (refs.SettingsPanel, refs.MainPanel);
	}
}
