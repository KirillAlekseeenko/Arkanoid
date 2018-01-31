using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveUtils
{
	public static class SettingsSaveUtility
	{
		public static void SaveSettings(Settings settings)
		{
			PlayerPrefs.SetString("SETTINGS", JsonUtility.ToJson(settings));
		}
		public static Settings LoadSettings()
		{
			if (PlayerPrefs.HasKey ("SETTINGS")) {
				return JsonUtility.FromJson<Settings> (PlayerPrefs.GetString ("SETTINGS"));
			} else {
				return new Settings ();
			}
		}
	}
	public static class RecordsSaveUtility
	{
		public static void SaveRecords(Records records)
		{
			PlayerPrefs.SetString("RECORDS", JsonUtility.ToJson(records));
		}
		public static Records LoadRecords()
		{
			if (PlayerPrefs.HasKey ("RECORDS")) {
				return JsonUtility.FromJson<Records> (PlayerPrefs.GetString ("RECORDS"));
			} else {
				return new Records ();
			}
		}
	}
	[System.Serializable]
	public class Session
	{
		[SerializeField] private string name;
		[SerializeField] private int score;
		public Session(string _name)
		{
			name = _name;
			score = 0;
		}

		public string Name {
			get {
				return name;
			}
		}

		public int Score {
			get {
				return score;
			}
			set {
				score = value;
			}
		}
	}
	[System.Serializable]
	public class Records : IEnumerable
	{
		[SerializeField] private List<Session> sessionList;

		public Records()
		{
			sessionList = new List<Session>();
		}
		public void AddSession(Session session)
		{
			int index = 0;
			while (index < sessionList.Count && sessionList[index].Score > session.Score) {
				index++;
			}
			sessionList.Insert (index, session);
		}

		#region IEnumerable implementation

		public IEnumerator GetEnumerator ()
		{
			return sessionList.GetEnumerator ();
		}

		#endregion

	}
	public class Settings
	{
		// audio settings
	}
}
