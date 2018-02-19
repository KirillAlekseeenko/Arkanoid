using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour {
	private static Transform leftWall;
	private static Transform rightWall;
	private static Transform floor;

	private static bool initialized = false;

	void OnEnable()
	{
		initialized = true;
		lazyInitialization ();
	}

	void OnDisable()
	{
		initialized = false;
	}

	public static float Width {
		get {
			if (!initialized)
				throw new UnityException ("GameFieldInfo is not initialized");
			return Mathf.Abs (leftWall.transform.position.x - rightWall.transform.position.x); 
		} 
	}
	public static float Height {
		get {
			if (!initialized)
				throw new UnityException ("GameFieldInfo is not initialized");
			return Mathf.Abs (floor.transform.position.y) * 2; 
		} 
	}

	private static void lazyInitialization()
	{
		var staticObjects = GameObject.Find ("LevelStatic");
		var walls = staticObjects.transform.Find ("Walls");
		leftWall = walls.Find ("LeftWall");
		rightWall = walls.Find ("RightWall");
		floor = walls.Find ("Floor");
		initialized = true;
	}
}