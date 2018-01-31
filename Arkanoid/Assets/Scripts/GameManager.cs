using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static GameManager instance;

	private static SaveUtils.Session currentSession;

	[SerializeField] private LevelGeneration levels;
	[SerializeField] private Level currentLevel;
	[SerializeField] private Transform enemiesTransform;
	[SerializeField] private Transform spheres;
	[SerializeField] private Transform bonuses;

	private bool isStarted = false;
	private bool isEnded = false;
	private bool isPaused = false;
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

	public static GameManager Instance {
		get {
			return instance;
		}
	}

	public static SaveUtils.Session CurrentSession {
		get {
			return currentSession;
		}
		set {
			currentSession = value;
		}
	}

	public int Score {
		get {
			return score;
		}
	}

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

	public bool Transition {
		get {
			return transition;
		}
	}

	public bool IsEnded
	{
		get {
			return isEnded; // winning conditions
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

	public void HandleDestroyedHittable(IHittable hittable)
	{
		var oldScore = score;
		score += hittable.RewardPoints;
		uiHandler.UpdateScore (score);
		checkWinConditions ();
		spawnBonus (hittable.Position, oldScore);
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

	public void CheckLostConditions()
	{
		if (spheres.childCount <= 1) { // last sphere
			lifeCount--;
			uiHandler.RemoveLife ();
			if (lifeCount > 0) {
				resetGameState ();
			} else {
				lost();
			}
		}
	}

	public void Lost()
	{
		lifeCount--;
		uiHandler.RemoveLife ();
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
		if (spheres.childCount > 1)
			return;
		Vector2 direction = spheres.GetChild (0).GetComponent<Rigidbody2D> ().velocity;
		Vector3 pos = spheres.GetChild (0).transform.position;
		for (int i = -1; i <= 1; i+=2) {
			var newSphere = Instantiate (spherePrefab, pos, Quaternion.identity, spheres) as GameObject;
			newSphere.GetComponent<Rigidbody2D> ().velocity = Utils.RotateVector (direction, 30.0f * Mathf.Deg2Rad * i);
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
		uiHandler.OnResetGame ();
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

		sphere.transform.position = sphereInitialPosition;
		platform.transform.position = platformInitialPosition;
		uiHandler.OnResetGame ();
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
	}

	#endregion

	private void checkWinConditions()
	{
		if (currentLevel.DestroyableBlocks.childCount <= 1) { // last block
			onLevelPassed();
		}
	}

	private void onLevelPassed()
	{
		currentLevel = levels.GetNextLevel (currentLevel);
		if (currentLevel == null) {
			win ();
		} else {
			resetGameState ();
		}
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
		isPaused = false;
		uiHandler.OnResumeGame ();
		Time.timeScale = 1;
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
				var oldMagnitude = sphereTransform.gameObject.GetComponent<SphereMovement> ().VelocityMagnitude;
				sphereTransform.gameObject.GetComponent<SphereMovement> ().VelocityMagnitude = sphereVelocity;
				sphereTransform.gameObject.GetComponent<Rigidbody2D> ().velocity *= (sphereVelocity / oldMagnitude);
			}
			yield return new WaitForSeconds (0.5f);
		}

	}

	private IEnumerator enemySpawner()
	{
		while (true) {
			yield return new WaitForSeconds (spawnTime);
			var randomIndex = Random.Range (0, enemies.Length);
			Instantiate (enemies [randomIndex], enemySpawnTransform.position, Quaternion.identity, enemiesTransform);
		}
	}
}
