using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveUtils
{
	private static class SaveUtility<T>
	{
		public static void Save(string key, T par)
		{
			PlayerPrefs.SetString(key, JsonUtility.ToJson(par));
		}
		public static T Load(string key, T returnValue)
		{
			if (PlayerPrefs.HasKey (key)) {
				return JsonUtility.FromJson<T> (PlayerPrefs.GetString (key));
			} else {
				return returnValue;
			}
		}
	}
	public static class SettingsSaveUtility
	{
		private const string key = "SETTINGS";

		public static void SaveSettings(Settings settings)
		{
			SaveUtility<Settings>.Save (key, settings);
		}
		public static Settings LoadSettings()
		{
			return SaveUtility<Settings>.Load (key, new Settings ());
		}
	}
	public static class RecordsSaveUtility
	{
		private const string key = "RECORDS";

		public static void SaveRecords(Records records)
		{
			SaveUtility<Records>.Save (key, records);
		}
		public static Records LoadRecords()
		{
			return SaveUtility<Records>.Load (key, new Records ());
		}
	}
	public static class PlayerNameSaveUtility
	{
		private const string key = "PLAYERNAME";

		public static void SaveName(string name)
		{
			PlayerPrefs.SetString(key, name);
		}
		public static string LoadName()
		{
			return PlayerPrefs.GetString (key);
		}
	}
}
