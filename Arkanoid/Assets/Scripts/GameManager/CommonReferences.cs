using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonReferences : MonoBehaviour {
	[SerializeField] private GameObject leftWall;
	[SerializeField] private GameObject rightWall;
	[SerializeField] private GameObject floor;

	public Vector3 LeftWallPosition{ get { return leftWall.transform.position; } }
	public Vector3 RightWallPosition{ get { return rightWall.transform.position; } }
	public Vector3 FloorPosition{ get { return floor.transform.position; } }
}
