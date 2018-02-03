using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public string Name { get { return name; } }

	public int Score { get { return score; } set { score = value; } }

	public override string ToString ()
	{
		return (name + " ").PadRight (15, '–') + " " + score.ToString ();
	}
}
