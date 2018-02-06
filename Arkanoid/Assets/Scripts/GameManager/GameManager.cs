using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static GameManager instance;

	private static Session currentSession;

	[SerializeField] private CommonReferences commonReferences;

	[SerializeField] private LevelGeneration levels;
	[SerializeField] private Level currentLevel;
	[SerializeField] private Transform enemiesTransform;
	[SerializeField] private Transform spheres;
	[SerializeField] private Transform bonuses;

	private int sphereCount;
	private int blocksCount;

	[SerializeField] private bool isStarted = false;
	[SerializeField] private bool isEnded = false;
	[SerializeField] private bool isPaused = false;
	private bool transition = false;

	[SerializeField] private int scoreToGetReward;
	[SerializeField] private Bonus[] bonus;
	[SerializeField] private MovingEnemy[] enemies;
	[SerializeField] private Transform enemySpawnTransform;
	[SerializeField] private float spawnTime;

	private int score;

	[SerializeField] private Platform platform;
	[SerializeField] private SphereMovement sphere;
	[SerializeField] private GameObject spherePrefab;
	[SerializeField] private Teleport teleport;

	[Header("UI")]

	[SerializeField] private UIHandler uiHandler;

	[Header("Balance")]

	[SerializeField] private int initialLifeCount;
	private int lifeCount;

	[SerializeField] private float initialSphereVelocity;
	[SerializeField] private float sphereAcceleration;
	[SerializeField] private float sphereVelocity;


	private Vector3 sphereInitialPosition;
	private Vector3 platformInitialPosition;

	private Coroutine enemySpawnCoroutine;
	private Coroutine updateSphereVelocityCoroutine;

	#region public properties

	public static GameManager Instance { get { return instance; } }
	public static Session CurrentSession { get { return currentSession; } set { currentSession = value; } }

	public CommonReferences CommonReferences { get{ return commonReferences;} }

	public int Score { get { return score; } }

	public bool Transition { get { return transition; } }

	public bool IsStarted {
		get {
			return isStarted;
		}
		set {
			if (isStarted == value)
				return;
			if(!isStarted)
				startGame ();
			isStarted = value;
		}
	}
		
	public bool IsEnded
	{
		get {
			return isEnded; 
		}
		private set {
			isEnded = value;
			if (value) {
				StopCoroutine (updateSphereVelocityCoroutine);
				StopCoroutine (enemySpawnCoroutine);
			}
		}
	}

	#endregion

	#region MonoBehaviour

	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy (gameObject);
		lifeCount = initialLifeCount;
		sphereVelocity = initialSphereVelocity;
	}

	void Start()
	{
		score = 0;
		currentLevel = levels.FirstLevel ();
		sphereCount = 1;
		blocksCount = currentLevel.DestroyableBlocks.childCount;
		sphereInitialPosition = sphere.transform.position;
		platformInitialPosition = platform.transform.position;
	}

	void Update()
	{
		if (IsStarted && !IsEnded) {
			sphereVelocity *= (1 + sphereAcceleration * Time.deltaTime);
		}
	}

	#endregion

	public void HittableDestroyed(IHittable hittable)
	{
		var oldScore = score;
		score += hittable.RewardPoints;
		uiHandler.UpdateScore (score);
		spawnBonus (hittable.Position, oldScore);

		if (hittable is Block) {
			blocksCount--;
			checkWinConditions ();
		}
	}

	public void PauseHandle()
	{
		if (isEnded)
			return;
		if (isPaused)
			resumeGame ();
		else
			pauseGame();
	}

	public void SphereLost()
	{
		sphereCount--;
		if (sphereCount == 0 && !transition) { // last sphere
			OnLifeLost();
			if (lifeCount > 0) {
				resetGameState ();
			} else {
				lost();
			}
		}
	}

	public void Lost()
	{
		if (transition)
			return;
		OnLifeLost ();
		if (lifeCount > 0) {
			resetGameState ();
		} else {
			lost();
		}
	}

	#region bonuses

	public void SlowSphereDown()
	{
		foreach (Transform sphereTransform in spheres) {
			sphereVelocity /= 2;
			var velocity = sphereTransform.gameObject.GetComponent<Rigidbody2D> ().velocity;
			sphereTransform.gameObject.GetComponent<Rigidbody2D> ().velocity = velocity / 2;
			sphereTransform.gameObject.GetComponent<SphereMovement> ().VelocityMagnitude /= 2;
		}
	}

	public void SpawnSpheres()
	{
		if (sphereCount > 1)
			return;
		if (platform.GetComponent<StickyPlatform> () != null) {
			spheres.GetChild (0).GetComponent<SphereMovement> ().Kick (Vector3.up);
		}
		Vector2 direction = spheres.GetChild (0).GetComponent<Rigidbody2D> ().velocity;
		Vector3 pos = spheres.GetChild (0).transform.position;
		for (int i = -1; i <= 1; i+=2) {
			sphereCount++;
			var newSphere = Instantiate (spherePrefab, pos, Quaternion.identity, spheres);
			newSphere.GetComponent<Rigidbody2D>().velocity = Utils.RotateVector (direction, 60.0f * Mathf.Deg2Rad * i);
		}
	}

	public void AdditionalLife()
	{
		lifeCount++;
		uiHandler.AddLife ();
	}

	public void CreatePortal()
	{
		teleport.gameObject.SetActive (true);
	}

	public void OnPortalEnter()
	{
		onLevelPassed ();
	}

	#endregion

	private void startGame()
	{
		updateSphereVelocityCoroutine = StartCoroutine (updateSphereVelocity ());
		enemySpawnCoroutine = StartCoroutine (enemySpawner ());
		uiHandler.OnStartGame ();
		sphere.FirstKick ();
	}

	#region reset game

	private void resetGameState() 
	{
		IsStarted = false;
		IsEnded = false;
		platform.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		StartCoroutine (resetGameCoroutine ());
	}

	private IEnumerator resetGameCoroutine()
	{
		transition = true;
		yield return uiHandler.ScreenFadeIn ();
		behindTheScene ();
		yield return uiHandler.ScreenFadeOut ();
		transition = false;
		yield return null;
	}

	private void behindTheScene()
	{
		StopCoroutine (updateSphereVelocityCoroutine);
		StopCoroutine (enemySpawnCoroutine);
		sphereVelocity = initialSphereVelocity;
		currentLevel.DestroyableBlocks.transform.parent.gameObject.SetActive (true);
		platform.transform.position = platformInitialPosition;
		foreach (Transform sphereTransform in spheres) {
			Destroy (sphereTransform.gameObject);
		}
		foreach (Transform enemy in enemiesTransform) {
			Destroy (enemy.gameObject);
		}
		sphere = Instantiate (spherePrefab, spheres).GetComponent<SphereMovement>();
		sphereCount = 1;
		sphere.transform.position = sphereInitialPosition;
		platform.transform.position = platformInitialPosition;
		uiHandler.OnResetGame (currentLevel.Number);
		destroyAllBonuses ();
	}

	private void destroyAllBonuses ()
	{
		teleport.gameObject.SetActive (false);
		var bonusComponent = platform.GetComponent<PlatformBonusComponent> ();
		if (bonusComponent != null)
			bonusComponent.RemoveBonus ();
		foreach (Transform bonus in bonuses) {
			Destroy (bonus.gameObject);
		}
		foreach (Transform enemy in enemiesTransform) {
			Destroy (enemy.gameObject);
		}
		foreach (Transform bullet in platform.transform) {
			Destroy (bullet.gameObject);
		}
	}

	#endregion

	private void checkWinConditions()
	{
		if (blocksCount == 0) { // last block
			onLevelPassed();
		}
	}

	private void onLevelPassed()
	{
		currentLevel = levels.GetNextLevel (currentLevel);
		blocksCount = currentLevel.DestroyableBlocks.childCount;
		AudioManager.Instance.PlayOnLevelPassedEffect ();
		if (currentLevel == null) {
			win ();
		} else {
			resetGameState ();
		}
	}

	private void OnLifeLost()
	{
		lifeCount--;
		uiHandler.RemoveLife ();
		AudioManager.Instance.PlayOnLifeLostEffect ();
	}

	private void win()
	{
		GameManager.CurrentSession.Score = score;
		IsEnded = true;
		uiHandler.OnEndGame (true);
		Time.timeScale = 0;
		saveResults ();
	}

	private void lost()
	{
		GameManager.CurrentSession.Score = score;
		IsEnded = true;
		uiHandler.OnEndGame (false);
		Time.timeScale = 0;
		saveResults ();
	}

	private void saveResults()
	{
		var records = SaveUtils.RecordsSaveUtility.LoadRecords ();
		records.AddSession (GameManager.CurrentSession);
		SaveUtils.RecordsSaveUtility.SaveRecords (records);
	}

	private void pauseGame()
	{
		isPaused = true;
		uiHandler.OnPauseGame ();
		Time.timeScale = 0;
	}

	private void resumeGame ()
	{
		Time.timeScale = 1;
		isPaused = false;
		uiHandler.OnResumeGame ();
	}

	private void spawnBonus(Vector3 position, int oldScore)
	{
		if (score / scoreToGetReward > oldScore / scoreToGetReward) {
			var randIndex = Random.Range (0, bonus.Length);
			Instantiate (bonus[randIndex].gameObject, position, Quaternion.identity, bonuses);
		}
	}
		
	private IEnumerator updateSphereVelocity()
	{
		while (true) {
			foreach (Transform sphereTransform in spheres) {
				sphereTransform.gameObject.GetComponent<SphereMovement> ().VelocityMagnitude = sphereVelocity;
			}
			yield return new WaitForSeconds (0.5f);
		}

	}

	private IEnumerator enemySpawner()
	{
		while (true) {
			yield return new WaitForSeconds (spawnTime);
			var randomIndex = Random.Range (0, enemies.Length);
			if(enemiesTransform.childCount < 3)
				Instantiate (enemies [randomIndex], enemySpawnTransform.position, Quaternion.identity, enemiesTransform);
		}
	}
}
