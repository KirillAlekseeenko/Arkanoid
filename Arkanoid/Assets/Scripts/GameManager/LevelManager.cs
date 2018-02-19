using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public delegate void LevelEvents(bool isLast);
	public static event LevelEvents LevelIsClear;

	private int blocksNumber;

	[SerializeField] private LevelList levelList;
	private Level currentLevel;

	void OnEnable()
	{
		GameManager.LevelBuiltEvent += onBuildLevel;
		GameManager.LevelCleanedEvent += onCleanLevel;
		Block.BlockDestroyed += onHittableDestroyed;
		Teleport.OnPortalEnter += onTeleportEnter;
	}

	void OnDisable()
	{
		GameManager.LevelBuiltEvent -= onBuildLevel;
		GameManager.LevelCleanedEvent -= onCleanLevel;
		Block.BlockDestroyed -= onHittableDestroyed;
		Teleport.OnPortalEnter -= onTeleportEnter;
	}

	private void onBuildLevel(int levelNumber)
	{
		currentLevel = levelList.GetLevelByNumber (levelNumber);
		currentLevel.DestroyableBlocks.parent.gameObject.SetActive (true);
		blocksNumber = currentLevel.DestroyableBlocks.childCount;
	}

	private void onHittableDestroyed(IHittable hittable)
	{
		blocksNumber--;
		if (blocksNumber == 0) {
			LevelIsClear (levelList.isLevelLast(currentLevel));
		}
	}

	private void onTeleportEnter()
	{
		LevelIsClear (levelList.isLevelLast (currentLevel));
	}

	private void onCleanLevel()
	{
		currentLevel.DestroyableBlocks.parent.gameObject.SetActive (false);
	}
}
