using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour {

	private List<Level> levelList;

	public Level GetLevelByNumber(int number)
	{
		if (levelList == null)
			initializeLevelList ();
		if (levelList.Count >= number) {
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
			levelList.Add(new Level (transform.GetChild(i), i + 1));
		}
	}
}
