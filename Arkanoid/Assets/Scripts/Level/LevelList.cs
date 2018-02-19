using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour {

	private List<Level> levelList;

	public Level FirstLevel()
	{
		if (levelList == null)
			initializeLevelList ();
		if (levelList.Count > 0)
			return levelList [0];
		else
			return null;
	}

	public Level GetNextLevel(Level level)
	{
		if (levelList.Count > level.Number) {
			var newLevel = levelList [level.Number];
			return newLevel;
		} else {
			return null;
		}
	}

	public Level GetLevelByNumber(int number)
	{
		if (levelList == null)
			initializeLevelList ();
		if (levelList.Count > number) {
			return levelList [number - 1];
		} else
			return null;
	}

	public bool isLevelLast(Level level)
	{
		return level.Number == levelList.Count;
	}
		
	private void initializeLevelList()
	{
		levelList = new List<Level> (transform.childCount);
		for (int i = 0; i < transform.childCount; i++) {
			var destroyableBlocks = transform.GetChild (i).Find ("DestroyableBlocks");
			var staticBlocks = transform.GetChild (i).Find ("StaticBlocks");
			levelList.Add(new Level (destroyableBlocks, staticBlocks, i + 1));
		}
	}
}
