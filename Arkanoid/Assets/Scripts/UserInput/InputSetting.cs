using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSetting : MonoBehaviour {

	void Start () 
	{
		initialize ();
	}
	
	private void initialize()
	{
		var settings = SaveUtils.SettingsSaveUtility.LoadSettings ();
		if (settings.isAnalogInputOn)
			gameObject.AddComponent<AnalogInput> ();
		else
			gameObject.AddComponent<DiscreteInput> ();
	}
}
