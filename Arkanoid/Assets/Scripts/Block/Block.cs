using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour, IHittable {

	public delegate void BlockEvent(IHittable hittable);
	public static event BlockEvent BlockDestroyed;

	[SerializeField] protected int rewardPoints;

	protected SpriteRenderer spriteRenderer;

	protected void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void OnDisable()
	{
		GameManager.LevelCleanedEvent -= onClean;
	}
		
	#region IHittable implementation

	public abstract void Hit ();

	public int RewardPoints { get { return rewardPoints; } }

	public Vector3 Position{ get { return transform.position; } }

	#endregion

	#region illumination

	protected virtual void illumination()
	{
		StartCoroutine (fadeInFadeOut ());
	}

	private IEnumerator fadeInFadeOut()
	{
		const float time = 0.2f;
		const int steps = 10;
		const float fadedAlpha = 0.2f;

		GameManager.LevelCleanedEvent += onClean;

		for (int i = -steps + 1; i <= steps; i++) {
			yield return new WaitForSeconds (time / steps);
			var newAlpha = fadedAlpha + Mathf.Sign (i) * i * (1.0f - fadedAlpha) / steps;
			var color = spriteRenderer.color;
			spriteRenderer.color = new Color (color.r, color.g, color.b, newAlpha);
		}

		GameManager.LevelCleanedEvent -= onClean;
	}

	#endregion

	protected void destroy()
	{
		BlockDestroyed (this);
		Destroy (gameObject);
	}

	private void onClean()
	{
		var color = spriteRenderer.color;
		spriteRenderer.color = new Color (color.r, color.g, color.b, 1.0f);
		GameManager.LevelCleanedEvent -= onClean;
	}
}
