using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Session
{
	[SerializeField] private string name;
	[SerializeField] private int score;

	public Session(string name, int score)
	{
		this.name = name;
		this.score = score;
	}

	public string Name { get { return name; } }
	public int Score { get { return score; } }

	public override string ToString ()
	{
		return (name + " ").PadRight (15, '–') + " " + score.ToString ();
	}
}
