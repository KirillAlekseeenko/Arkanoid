using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

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

	private Coroutine damageCoroutine;

	private float xRightWallPosition;

	private Vector3 initialPosition;

	private float yDirection { get { return tangentMaxDirectionAngle * HalfLength; } }
	private float tangentMaxDirectionAngle;

	public float HalfLength { get { return Mathf.Abs(GetComponent<EdgeCollider2D> ().points [0].x); } }

	public Bullet BulletPrefab { get { return bulletPrefab; } }

	public float ReloadTime { get { return reloadTime; } }

	public Vector3 LeftSpawn { get { return transform.position + new Vector3 (-HalfLength, bulletSpawnHeight); } }
	public Vector3 RightSpawn { get { return transform.position + new Vector3 (HalfLength, bulletSpawnHeight); } }

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

	void Start () {
		initialPosition = transform.position;
		xRightWallPosition = GameField.Width / 2;
		tangentMaxDirectionAngle = Mathf.Tan (maxDirectionAngle * Mathf.Deg2Rad);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Bonus")) {
			other.gameObject.GetComponent<Bonus> ().GetBonus (this);
			Destroy (other.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!activated) {
			return;
		}
		var sphere = other.gameObject.GetComponent<SphereMovement> ();
		if (sphere != null && sphere.transform.position.y >= transform.position.y) {
			var direction = new Vector2 (sphere.transform.position.x - transform.position.x, yDirection);
			var sticky = GetComponent<StickyPlatform> ();
			if (sticky == null) {
				AudioManager.Instance.PlayOnPlatformHitEffect ();
				sphere.Kick (direction);
			} else {
				sphere.Stop ();
				sticky.AddSphere (sphere, direction);
			}
		}
	}

	#endregion

	#region controls

	private void enableInput()
	{
		UserInput.MoveLeft += moveLeft;
		UserInput.MoveRight += moveRight;
		UserInput.SpecialAction += specialAction;
	}

	private void disableInput()
	{
		UserInput.MoveLeft -= moveLeft;
		UserInput.MoveRight -= moveRight;
		UserInput.SpecialAction -= specialAction;
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
}
