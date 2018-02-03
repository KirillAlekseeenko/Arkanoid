using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuRecordsUIHandler : MonoBehaviour {

	[SerializeField] private GameObject recordList;
	[SerializeField] private Text textPrefab;

	void Start()
	{
		fillRecords ();
	}

	private void fillRecords()
	{
		var records = SaveUtils.RecordsSaveUtility.LoadRecords ();
		for (int i = 0; i < records.MaxSessionCount; i++) {
			var label = Instantiate (textPrefab, recordList.transform).GetComponent<Text> ();
			label.text = printNumber (i + 1);
			if (i < records.Count) {
				label.text += records [i].ToString ();
			}
		}
	}

	private string printNumber(int number)
	{
		var str = number.ToString () + ". ";
		if (number < 10) {
			str += "  ";
		}
		return str;
	}

	public void OnBackButton()
	{
		var refs = GetComponent<ReferencesUIHandler> ();
		MainMenuUtils.MakePanelTransition (refs.RecordsPanel, refs.MainPanel);
	}

}
