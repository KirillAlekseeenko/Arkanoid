using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour {

	[SerializeField] private Bonus[] bonusPrefabs;
	[SerializeField] private Bonus purpleBonusPrefab;
	[SerializeField] [Range(0, 1)] private float purpleBonusChance;
	[SerializeField] private int scoreToGetRandomReward;

	private Transform bonuses;

	private int currentScore = 0;

	void OnEnable()
	{
		MovingEnemy.EnemyDestroyed += onHittableDestroyed;
		Block.BlockDestroyed += onHittableDestroyed;
		GameManager.LevelCleanedEvent += onCleanLevel;
	}

	void OnDisable()
	{
		MovingEnemy.EnemyDestroyed -= onHittableDestroyed;
		Block.BlockDestroyed -= onHittableDestroyed;
		GameManager.LevelCleanedEvent -= onCleanLevel;
	}

	void Start()
	{
		bonuses = new GameObject ("Bonuses").transform;
		bonuses.parent = transform;
	}

	private void onHittableDestroyed(IHittable hittable)
	{
		currentScore += hittable.RewardPoints;
		if (currentScore >= scoreToGetRandomReward) {
			currentScore -= scoreToGetRandomReward;
			spawnBonus (hittable.Position);
		}
	}

	private void onCleanLevel()
	{
		foreach (Transform bonus in bonuses) {
			Destroy (bonus.gameObject);
		}
	}

	private void spawnBonus(Vector3 position)
	{
		Bonus bonusToSpawn;
		var randNumber = Random.value;
		if (randNumber < purpleBonusChance) {
			bonusToSpawn = purpleBonusPrefab;
		} else {
			var randIndex = Random.Range (0, bonusPrefabs.Length);
			bonusToSpawn = bonusPrefabs [randIndex];
		}

		Instantiate (bonusToSpawn.gameObject, position, Quaternion.identity, bonuses);
	}
}
