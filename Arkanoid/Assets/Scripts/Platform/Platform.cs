using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	private PlatformBonusComponent bonusComponent;
	private Rigidbody2D rigidbodyComponent;

	[SerializeField] private float speed;
	[SerializeField] private float maxDirectionAngle;

	private float halfLength { get { return Mathf.Abs(GetComponent<EdgeCollider2D> ().points [0].x); } }
	private float yDirection { get { return tangentMaxDirectionAngle * halfLength; } }
	private float tangentMaxDirectionAngle;

	[Header("GunProperties")]
	[SerializeField] private Bullet bulletPrefab;
	[SerializeField] private float bulletSpawnHeight;
	[SerializeField] private float reloadTime;

	public Bullet BulletPrefab {
		get {
			return bulletPrefab;
		}
	}

	public float ReloadTime {
		get {
			return reloadTime;
		}
	}

	public Vector3 LeftSpawn { get { return new Vector3(-halfLength, transform.position.y + bulletSpawnHeight); } }
	public Vector3 RightSpawn { get { return new Vector3(halfLength, transform.position.y + bulletSpawnHeight); } }



	#region MonoBehaviour

	void Start () {
		tangentMaxDirectionAngle = Mathf.Tan (maxDirectionAngle * Mathf.Deg2Rad);
		rigidbodyComponent = GetComponent<Rigidbody2D> ();


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
		if (!GameManager.Instance.IsStarted || GameManager.Instance.IsEnded)
			return;
		
		var sphere = other.gameObject.GetComponent<SphereMovement> ();
		if (sphere != null) {
			var direction = new Vector2 (sphere.transform.position.x - transform.position.x, yDirection);
			var sticky = GetComponent<StickyPlatform> ();
			if (sticky == null) {
				sphere.Kick (direction);
			} else {
				sphere.Stop ();
				sticky.AddSphere (sphere, direction);
			}
		}
	}

	#endregion

	#region controls

	public void MoveLeft()
	{
		rigidbodyComponent.velocity = Vector2.left * speed;
	}

	public void MoveRight()
	{
		rigidbodyComponent.velocity = Vector2.right * speed;
	}

	public void Stop()
	{
		rigidbodyComponent.velocity = Vector2.zero;
	}

	public void SpecialAction()
	{
		if((bonusComponent = GetComponent<PlatformBonusComponent>()) != null)
			bonusComponent.SpecialAction ();
	}

	#endregion
}
