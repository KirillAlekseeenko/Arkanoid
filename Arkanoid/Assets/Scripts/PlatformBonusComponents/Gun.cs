using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : PlatformBonusComponent {

	private Bullet bulletPrefab;
	private Vector3 leftSpawn;
	private Vector3 rightSpawn;
	private float reloadTime;

	private Coroutine bulletSpawnerCoroutine;

	protected void Start () {
		bulletPrefab = GetComponent<Platform> ().BulletPrefab;
		leftSpawn = GetComponent<Platform> ().LeftSpawn;
		rightSpawn = GetComponent<Platform> ().RightSpawn;
		reloadTime = GetComponent<Platform> ().ReloadTime;

		bulletSpawnerCoroutine = StartCoroutine (bulletSpawn ());
	}

	private void spawnBullet(Vector3 place)
	{
		Instantiate (bulletPrefab.gameObject, place, Quaternion.identity);
	}

	private IEnumerator bulletSpawn()
	{
		while (true) {
			yield return new WaitForSeconds (reloadTime);
			spawnBullet (leftSpawn);
			spawnBullet (rightSpawn);
		}
	}
		
	#region implemented abstract members of PlatformBonusComponent

	public override void RemoveBonus ()
	{
		StopCoroutine (bulletSpawnerCoroutine);
		Destroy (this);
	}

	#endregion
}
