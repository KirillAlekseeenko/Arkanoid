using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencesUIHandler : MonoBehaviour {

	[SerializeField] private Animator mainPanel;
	[SerializeField] private Animator recordsPanel;
	[SerializeField] private Animator settingsPanel;
	[SerializeField] private Animator namePanel;

	public Animator MainPanel { get { return mainPanel; } }
	public Animator RecordsPanel { get { return recordsPanel; } }
	public Animator SettingsPanel { get { return settingsPanel; } }
	public Animator NamePanel { get { return namePanel; } }
}
