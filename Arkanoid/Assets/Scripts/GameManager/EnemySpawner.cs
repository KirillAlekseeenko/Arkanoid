using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	[SerializeField] private MovingEnemy[] enemyPrefabs;
	[SerializeField] private Transform enemySpawnPlace;
	[SerializeField] private float spawnTime;
	[SerializeField] private int maxEnemyNumber;

	private Transform enemies;

	private Coroutine enemySpawnerCoroutine;

	void OnEnable()
	{
		GameManager.GameStartedEvent += startSpawn;
		GameManager.LifeLostEvent += stopSpawn;
		GameManager.LevelPassedEvent += stopSpawn;
		GameManager.LevelCleanedEvent += OnLevelClean;
	}

	void OnDisable()
	{
		GameManager.GameStartedEvent -= startSpawn;
		GameManager.LifeLostEvent -= stopSpawn;
		GameManager.LevelPassedEvent -= stopSpawn;
		GameManager.LevelCleanedEvent -= OnLevelClean;
	}

	void Start()
	{
		enemies = new GameObject ("Enemies").transform;
		enemies.parent = transform;
	}

	private void startSpawn()
	{
		enemySpawnerCoroutine = StartCoroutine (spawner ());
	}

	private void stopSpawn()
	{
		StopCoroutine (enemySpawnerCoroutine);
	}

	private void OnLevelClean()
	{
		foreach (Transform enemy in enemies) {
			Destroy (enemy.gameObject);
		}
	}

	private IEnumerator spawner()
	{
		while (true) {
			yield return new WaitForSeconds (spawnTime);
			var randomIndex = Random.Range (0, enemyPrefabs.Length);
			var randomOffset = new Vector3(Random.Range (-GameField.Width / 2.5f, GameField.Width / 2.5f), 0); 
			if (enemies.childCount < maxEnemyNumber) {
				Instantiate (enemyPrefabs [randomIndex], enemySpawnPlace.position + randomOffset, Quaternion.identity, enemies);
			}
		}
	}	
}
