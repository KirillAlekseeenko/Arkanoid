using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

	[SerializeField] private List<Level> levelList;

	public Level FirstLevel()
	{
		var newLevel = levelList [0];
		newLevel.DestroyableBlocks.transform.parent.gameObject.SetActive (true);
		return newLevel;
	}

	public Level GetNextLevel(Level level)
	{
		level.DestroyableBlocks.transform.parent.gameObject.SetActive (false);

		if (levelList.Count > level.Number) {
			var newLevel = levelList [level.Number];
			return newLevel;
		} else {
			return null;
		}
	}

	public bool isLastLevel(Level level)
	{
		return levelList.Count == level.Number;
	}

	public List<Level> LevelList {
		get {
			return levelList;
		}
	}
}
