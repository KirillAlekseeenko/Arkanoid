using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy, IHittable {

	[SerializeField] protected int rewardPoints;

	protected Vector3 direction = Vector3.down;
	protected bool canSee = true;

	private const float sphereCastRadius = 0.1f;
	private const float sphereCastMaxDistance = 0.5f;

	#region MonoBehaviour

	private void Update()
	{
		movePattern ();
	}

	protected void Start()
	{
		StartCoroutine (visionCoroutine ());
	}

	#endregion

	#region IHittable implementation

	public Vector3 Position { get { return transform.position; } }

	public int RewardPoints { get { return rewardPoints; } }

	public void Hit ()
	{
		Destroy (gameObject);
	}

	#endregion

	#region base movement

	private IEnumerator visionCoroutine()
	{
		while (canSee) {
			if (isObstacleOnTheWay(direction)) {
				var dir = getRandomDirection ();
				if (isObstacleOnTheWay (dir)) {
					dir = -dir;
					if (isObstacleOnTheWay (dir)) {
						dir = -direction;
					}
				}
				direction = dir;
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	private void movePattern ()
	{
		transform.Translate (direction * speed * Time.deltaTime);
	}

	#endregion

	#region support methods

	private bool isObstacleOnTheWay(Vector2 dir)
	{
		return Physics2D.CircleCast (transform.position, sphereCastRadius, dir, sphereCastMaxDistance, LayerMask.GetMask ("Block", "Wall"));
	}

	private Vector3 getRandomDirection()
	{
		int randomVariable = Random.Range (0, 2) == 0 ? -1 : 1; // -1 or 1 
		return Quaternion.AngleAxis (90.0f * randomVariable, Vector3.forward) * direction;
	}

	#endregion

}
