using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level {

	[SerializeField] private Transform levelTransform;
	[SerializeField] private Transform destroyableBlocks;
	[SerializeField] private Transform staticBlocks;

	[SerializeField] private int number;

	public Transform DestroyableBlocks { get { return destroyableBlocks; } }
	public Transform StaticBlocks { get { return staticBlocks; } }
	public Transform LevelTransform { get { return levelTransform; } }

	public int Number { get { return number; } }

	public Level(Transform levelTransform, int number)
	{
		this.levelTransform = levelTransform;
		this.destroyableBlocks = levelTransform.Find ("DestroyableBlocks");
		this.staticBlocks = levelTransform.Find ("StaticBlocks");
		this.number = number;
	}
}

