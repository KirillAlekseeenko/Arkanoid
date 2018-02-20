using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpheresManager : MonoBehaviour {

	public delegate void SphereEvents();
	public static event SphereEvents AllSpheresLost;

	private int sphereCount = 1;

	[SerializeField] private SphereMovement mainSphere;

	[SerializeField] private SphereMovement spherePrefab;

	[SerializeField] private float initialVelocity;
	[SerializeField] private float acceleration;

	private float currentVelocity;

	private Coroutine updateVelocityCoroutine;

	private Vector3 sphereInitialPosition;

	private Transform spheres;

	void OnEnable()
	{ 
		GameManager.LevelCleanedEvent += onCleanLevel;
		GameManager.GameStartedEvent += startVelocityIncrease;
		GameManager.LevelPassedEvent += stopVelocityIncrease;
		GameManager.LifeLostEvent += stopVelocityIncrease;
		PinkBonus.PinkBonusAcquired += slowSpheres;
		LightBlueBonus.LightBlueBonusAcquired += spawnSpheres;
		Border.SphereLost += onSphereLost;
	}

	void OnDisable()
	{
		GameManager.LevelCleanedEvent -= onCleanLevel;
		GameManager.GameStartedEvent -= startVelocityIncrease;
		GameManager.LevelPassedEvent -= stopVelocityIncrease;
		GameManager.LifeLostEvent -= stopVelocityIncrease;
		PinkBonus.PinkBonusAcquired -= slowSpheres;
		LightBlueBonus.LightBlueBonusAcquired -= spawnSpheres;
		Border.SphereLost -= onSphereLost;
	}

	void Start () {
		spheres = new GameObject ("Spheres").transform;
		spheres.parent = transform;
		mainSphere.transform.parent = spheres;
		sphereInitialPosition = mainSphere.transform.position;
	}

	private void spawnSpheres()
	{
		if (spheres.childCount > 1) {
			return;
		}
		Vector2 direction = spheres.GetChild (0).GetComponent<Rigidbody2D> ().velocity;
		Vector3 pos = spheres.GetChild (0).transform.position;
		for (int i = -1; i <= 1; i+=2) {
			sphereCount++;
			var newSphere = Instantiate (spherePrefab, pos, Quaternion.identity, spheres);
			newSphere.VelocityMagnitude = currentVelocity;
			newSphere.Kick(MathUtils.RotateVector (direction, 15.0f * Mathf.Deg2Rad * i));
		}
	}

	private void slowSpheres()
	{
		currentVelocity /= 2;
	}

	private void onCleanLevel()
	{
		foreach (Transform sphere in spheres)
			Destroy (sphere.gameObject);
		mainSphere = Instantiate (spherePrefab, sphereInitialPosition, Quaternion.identity, spheres);
		sphereCount = 1;
	}

	private void startVelocityIncrease()
	{
		currentVelocity = initialVelocity;
		updateVelocityCoroutine = StartCoroutine (updateSphereVelocity ());
	}

	private void stopVelocityIncrease()
	{
		StopCoroutine (updateVelocityCoroutine);
	}

	private IEnumerator updateSphereVelocity()
	{
		const float deltatime = 0.5f;
		
		while (true) {
			currentVelocity += acceleration;
			foreach (Transform sphereTransform in spheres) {
				sphereTransform.gameObject.GetComponent<SphereMovement> ().VelocityMagnitude = currentVelocity;
			}
			yield return new WaitForSeconds (deltatime);
		}
	}

	private void onSphereLost()
	{
		sphereCount--;
		if (sphereCount == 0) {
			AllSpheresLost ();
		}
	}
}
