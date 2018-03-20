using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Session
{
	[SerializeField] private string name;
	[SerializeField] private int score;
	[SerializeField] private int levelNumber;

	public Session(string name, int score, int levelNumber)
	{
		this.name = name;
		this.score = score;
		this.levelNumber = levelNumber;
	}

	public string Name { get { return name; } }
	public int Score { get { return score; } }
	public int LevelNumber { get { return levelNumber; } }

	public override string ToString ()
	{
		return (name + " ").PadRight (15, '–') + " " + score.ToString ();
	}
}
