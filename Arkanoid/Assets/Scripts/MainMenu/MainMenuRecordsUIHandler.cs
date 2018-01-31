using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuRecordsUIHandler : MonoBehaviour {

	[SerializeField] private GameObject recordList;
	[SerializeField] private Text textPrefab;

	void Start()
	{
		var records = SaveUtils.RecordsSaveUtility.LoadRecords ();
		foreach (SaveUtils.Session session in records) {
			var text = Instantiate (textPrefab.gameObject, recordList.transform).GetComponent<Text>();
			text.text = session.Name + " - " + session.Score;
		}
	}

	public void OnBackButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		MainMenuUtils.MakePanelTransition (refs.RecordsPanel, refs.MainPanel);
	}

}
