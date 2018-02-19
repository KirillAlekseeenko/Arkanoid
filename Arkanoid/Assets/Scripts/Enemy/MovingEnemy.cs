using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy, IHittable {

	public delegate void Destroyed(IHittable hittable);
	public static event Destroyed EnemyDestroyed;

	[SerializeField] protected int rewardPoints;

	protected Vector3 direction = Vector3.down;

	protected bool lowerThanAnyBlock = true;

	private const float visionUpdateTime = 0.1f;
	private float visionDistance;
	private Movement movement;

	#region MonoBehaviour

	private void Update()
	{
		transform.Translate (direction * speed * Time.deltaTime);
	}

	protected void Start()
	{
		visionDistance = speed * visionUpdateTime * 1.5f;
		movement = new Movement ();
		StartCoroutine (moveDown ());
	}

	#endregion

	#region IHittable implementation

	public Vector3 Position { get { return transform.position; } }

	public int RewardPoints { get { return rewardPoints; } }

	public void Hit ()
	{
		EnemyDestroyed (this);
		Destroy (gameObject);
	}

	#endregion

	#region base movement

	private IEnumerator moveDown()
	{
		while (lowerThanAnyBlock) {
			var tData = new VisionData ();
			tData.canMoveDown = !isObstacleOnTheWay (Vector3.down);
			tData.canMoveUp = !isObstacleOnTheWay (Vector3.up);
			tData.canMoveToTheLeft = !isObstacleOnTheWay (Vector3.left, out tData.wallOnTheLeft);
			tData.canMoveToTheRight = !isObstacleOnTheWay (Vector3.right, out tData.wallOnTheRight);
			tData.stuck = isStucked ();

			direction = movement.UpdateDirection(tData);

			yield return new WaitForSeconds (visionUpdateTime);
		}
	}

	#endregion

	#region support methods

	private bool isStucked()
	{
		return Physics2D.OverlapBox (transform.position, GetComponent<BoxCollider2D> ().size * 0.85f, 0, LayerMask.GetMask ("Block"));
	}

	private bool isObstacleOnTheWay(Vector2 dir)
	{
		return Physics2D.BoxCast (transform.position, GetComponent<BoxCollider2D> ().size, 0, dir, visionDistance, LayerMask.GetMask ("Block", "Wall"));
	}

	private bool isObstacleOnTheWay(Vector2 dir, out bool wall)
	{
		var hit = Physics2D.BoxCast (transform.position, GetComponent<BoxCollider2D> ().size, 0, dir, visionDistance, LayerMask.GetMask ("Block", "Wall"));
		if (hit.collider == null) {
			wall = false;
			return false;
		}
		wall = (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Wall"));
		return true;
	}

	private Vector3 getRandomDirection()
	{
		int randomVariable = Random.Range (0, 2) == 0 ? -1 : 1; // -1 or 1 
		if (direction.Equals (Vector3.down) || direction.Equals (Vector3.up)) {
			return new Vector3 (randomVariable, 0);
		} else {
			return new Vector3 (0, randomVariable);
		}
	}

	#endregion

	#region movement AI

	private class Movement
	{
		private bool transitionProhibited = false;

		private bool rightPriority;  
		private int inversedCount = 0;
		private bool inversedPriority { get { return (inversedCount % 2) != 0; } }

		private int recursiveCallsCount = 0;

		private Vector3 currentDirection;

		public Movement()
		{
			currentDirection = Vector3.down;
			rightPriority = Random.Range(0, 2) == 0 ? true : false; 
		}

		public Vector3 UpdateDirection(VisionData tData)
		{
			if (!transitionProhibited) {
				var previousDirection = currentDirection;
				recursiveCallsCount = 0;
				currentDirection = getDirection (tData);
				if (!currentDirection.Equals (previousDirection)) {
					transitionProhibited = !tData.stuck;
				}
			} else {
				transitionProhibited = false;
			}

			return currentDirection;
		}

		private Vector3 getDirection(VisionData tData)
		{
			var localRightPriority = inversedPriority ^ rightPriority;
			var localDownPriority = !inversedPriority;
			var downPriorityMultiplier = localDownPriority ? 1 : -1;
			var rightPriorityMultiplier = localRightPriority ? 1 : -1;

			if (tData.stuck || recursiveCallsCount > 10) {                                       // enemy stuck in blocks
				if(currentDirection.Equals(Vector3.down * downPriorityMultiplier))
					return Vector3.right * rightPriorityMultiplier + Vector3.up * downPriorityMultiplier;
				else
					return Vector3.left * rightPriorityMultiplier + Vector3.up * downPriorityMultiplier;
			}

			if (inversedCount > 0
				&& currentDirection.Equals (Vector3.down * downPriorityMultiplier)
				&& (localRightPriority ? tData.canMoveToTheLeft : tData.canMoveToTheRight)) {
				inversedCount--;
				recursiveCallsCount++;
				return getDirection (tData);
			}

			if ((localDownPriority ? tData.canMoveDown : tData.canMoveUp) && !currentDirection.Equals (Vector3.up * downPriorityMultiplier)) {
				return Vector3.down * downPriorityMultiplier;
			} else if ((localRightPriority ? tData.canMoveToTheRight : tData.canMoveToTheLeft)) {
				return Vector3.right * rightPriorityMultiplier;
			} else if ((localRightPriority ? tData.wallOnTheRight : tData.wallOnTheLeft)) {
				rightPriority = !rightPriority;
				return Vector3.left * rightPriorityMultiplier;
			} else if (localDownPriority ? tData.canMoveUp : tData.canMoveDown) {
				return Vector3.up * downPriorityMultiplier;
			} else {
				recursiveCallsCount++;
				inversedCount++;
				return getDirection (tData);
			}

		}
	}

	private struct VisionData
	{
		public bool canMoveToTheRight;
		public bool canMoveToTheLeft;
		public bool wallOnTheLeft;
		public bool wallOnTheRight;
		public bool canMoveUp;
		public bool canMoveDown;
		public bool stuck;

		public VisionData (bool canMoveToTheRight, bool canMoveToTheLeft, bool wallOnTheLeft, bool wallOnTheRight, bool canMoveUp, bool canMoveDown, bool stuck)
		{
			this.stuck = stuck;
			this.canMoveToTheRight = canMoveToTheRight;
			this.canMoveToTheLeft = canMoveToTheLeft;
			this.wallOnTheLeft = wallOnTheLeft;
			this.wallOnTheRight = wallOnTheRight;
			this.canMoveUp = canMoveUp;
			this.canMoveDown = canMoveDown;
		}

		public VisionData(VisionData tData)
		{
			this.canMoveToTheRight = tData.canMoveToTheRight;
			this.canMoveToTheLeft = tData.canMoveToTheLeft;
			this.wallOnTheLeft = tData.wallOnTheLeft;
			this.wallOnTheRight = tData.wallOnTheRight;
			this.canMoveUp = tData.canMoveUp;
			this.canMoveDown = tData.canMoveDown;
			this.stuck = tData.stuck;
		}

	}

	#endregion

}
