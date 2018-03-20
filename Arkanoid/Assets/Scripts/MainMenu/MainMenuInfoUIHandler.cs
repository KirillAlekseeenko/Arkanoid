using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInfoUIHandler : MonoBehaviour {

	public void OnBackButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		AnimationUtils.MakePanelTransition (refs.InfoPanel, refs.MainPanel);
	}
}
