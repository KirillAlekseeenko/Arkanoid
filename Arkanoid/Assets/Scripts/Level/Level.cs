using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level {

	[SerializeField] private Transform destroyableBlocks;
	[SerializeField] private Transform staticBlocks;

	[SerializeField] private int number;

	public Transform DestroyableBlocks { get { return destroyableBlocks; } }
	public Transform StaticBlocks { get { return staticBlocks; } }

	public int Number { get { return number; } }

	public Level(Transform destroyableBlocks, Transform staticBlocks, int number)
	{
		this.destroyableBlocks = destroyableBlocks;
		this.staticBlocks = staticBlocks;
		this.number = number;
	}
}

