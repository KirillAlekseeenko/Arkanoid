using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	public delegate void PlatformEvent ();
	public static event PlatformEvent SphereSticked;

	private bool activated = false;

	[SerializeField] private float speed;
	[SerializeField] private float maxDirectionAngle;

	[Header("Sprites")]
	[SerializeField] private Sprite defaultSprite;
	[SerializeField] private Sprite damagedSprite;

	[Header("GunProperties")]
	[SerializeField] private Bullet bulletPrefab;
	[SerializeField] private float bulletSpawnHeight;
	[SerializeField] private float reloadTime;
	[SerializeField] private float timeToRemove;
	[SerializeField] private float offset;

	private Coroutine damageCoroutine;

	private float xRightWallPosition;

	private Vector3 initialPosition;

	private float yDirection { get { return tangentMaxDirectionAngle * HalfLength; } }
	private float tangentMaxDirectionAngle;

	private Vector3 analogInputPosition;
	private bool analogInput = false;

	public float HalfLength { get { return GetComponent<SpriteRenderer> ().bounds.extents.x; } }

	public Bullet BulletPrefab { get { return bulletPrefab; } }

	public float ReloadTime { get { return reloadTime; } }

	public float TimeToRemove { get { return timeToRemove; } }

	public Vector3 LeftSpawn { get { return transform.position + new Vector3 (-HalfLength + offset, bulletSpawnHeight); } }
	public Vector3 RightSpawn { get { return transform.position + new Vector3 (HalfLength - offset, bulletSpawnHeight); } }

	#region MonoBehaviour

	void OnEnable()
	{
		GameManager.GameStartedEvent += activate;
		GameManager.LevelPassedEvent += deactivate;
		GameManager.LifeLostEvent += setDamaged;
		GameManager.LifeLostEvent += deactivate;
		GameManager.LevelCleanedEvent += onLevelClean;
	}

	void OnDisable()
	{
		GameManager.GameStartedEvent -= activate;
		GameManager.LevelPassedEvent -= deactivate;
		GameManager.LifeLostEvent -= setDamaged;
		GameManager.LifeLostEvent -= deactivate;
		GameManager.LevelCleanedEvent -= onLevelClean;
		disableInput ();
	}

	void Start ()
	{
		initialPosition = transform.position;
		xRightWallPosition = GameField.Width / 2;
		tangentMaxDirectionAngle = Mathf.Tan (maxDirectionAngle * Mathf.Deg2Rad);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!activated) {
			return;
		}
		
		if (other.gameObject.layer == LayerMask.NameToLayer ("Bonus")) {
			other.gameObject.GetComponent<Bonus> ().GetBonus (this);
			Destroy (other.gameObject);
		}

		var sphere = other.gameObject.GetComponent<SphereMovement> ();
		if (sphere != null && sphere.CanInteractWithPlatform && isSphereCanBeKicked(sphere)) {
			var direction = new Vector2 (sphere.transform.position.x - transform.position.x, yDirection);
			var sticky = GetComponent<StickyPlatform> ();
			if (sticky == null) {
				sphere.CanInteractWithPlatform = false;
				AudioManager.Instance.PlayOnPlatformHitEffect ();
				sphere.Kick (direction);
			} else {
				sphere.Stop ();
				sticky.AddSphere (sphere, direction);
				if(SphereSticked != null)
					SphereSticked ();
			}
		}

	}

	#endregion

	#region controls

	private void enableInput()
	{
		DiscreteInput.MoveLeft += moveLeft;
		DiscreteInput.MoveRight += moveRight;
		DiscreteInput.SpecialAction += specialAction;
		AnalogInput.Move += move;
	}

	private void disableInput()
	{
		DiscreteInput.MoveLeft -= moveLeft;
		DiscreteInput.MoveRight -= moveRight;
		DiscreteInput.SpecialAction -= specialAction;
		AnalogInput.Move -= move;
		analogInputPosition = initialPosition;
		analogInput = false;
	}

	private void moveLeft()
	{
		transform.Translate (Vector3.left * Time.deltaTime * speed, Space.World);
		clampPosition ();
	}

	private void moveRight()
	{
		transform.Translate (Vector3.right * Time.deltaTime * speed, Space.World);
		clampPosition ();
	}

	private void move(Vector3 pos)
	{
		analogInput = true;
		analogInputPosition = pos;
		if (Mathf.Abs(analogInputPosition.x - transform.position.x) > 0.001f && analogInput) {
			if (Mathf.Abs (analogInputPosition.x - transform.position.x) < speed * Time.deltaTime) {
				transform.Translate (analogInputPosition.x - transform.position.x, 0, 0, Space.World);
			} else if (analogInputPosition.x < transform.position.x) {
				moveLeft ();
			} else
				moveRight ();

			clampPosition ();
		}
	}

	private void specialAction()
	{
		var bonus = GetComponent<PlatformBonusComponent> ();
		if(bonus != null)
			bonus.SpecialAction ();
	}

	#endregion

	private void activate()
	{
		activated = true;
		enableInput ();
	}

	private void deactivate()
	{
		activated = false;
		disableInput ();
	}

	private void setDamaged()
	{
		damageCoroutine = StartCoroutine (damage());
	}

	private void onLevelClean()
	{
		var bonus = GetComponent<PlatformBonusComponent> ();
		if (bonus != null)
			bonus.RemoveBonus ();
		foreach (Transform bullet in transform)
			Destroy (bullet.gameObject);
		if(damageCoroutine != null)
			StopCoroutine (damageCoroutine);
		GetComponent<SpriteRenderer> ().sprite = defaultSprite;
		transform.position = initialPosition;
	}

	private IEnumerator damage()
	{
		const float deltatime = 0.3f;
		bool damagedSpriteSet = false;
		
		while (true) {
			if (damagedSpriteSet) {
				GetComponent<SpriteRenderer> ().sprite = damagedSprite; 
			} else {
				GetComponent<SpriteRenderer> ().sprite = defaultSprite;
			}
			damagedSpriteSet = !damagedSpriteSet;
			yield return new WaitForSecondsRealtime (deltatime);
		}
	}

	private void clampPosition()
	{
		var clampXValue = xRightWallPosition - HalfLength;
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -clampXValue , clampXValue), transform.position.y);
	}

	private bool isSphereCanBeKicked(SphereMovement sphere)
	{
		//return sphere.transform.position.y > transform.position.y || Mathf.Abs (sphere.transform.position.x - transform.position.x) > HalfLength;
		return true;
	}
}
