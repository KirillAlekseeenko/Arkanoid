using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour, IHittable {

	[SerializeField] protected int rewardPoints;

	protected SpriteRenderer spriteRenderer;

	private const float illuminationTime = 0.2f;
	private const int illuminationSteps = 10;
	private const float fadedAlpha = 0.2f;

	protected void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	#region IHittable implementation

	public abstract void Hit ();

	public int RewardPoints { get { return rewardPoints; } }

	public Vector3 Position{ get { return transform.position; } }

	#endregion

	#region illumination

	protected virtual void illumination()
	{
		StartCoroutine (fadeInOut ());
	}

	private IEnumerator fadeInOut()
	{
		for (int i = -illuminationSteps + 1; i <= illuminationSteps; i++) {
			yield return new WaitForSeconds (illuminationTime / illuminationSteps);
			var newAlpha = fadedAlpha + Mathf.Sign (i) * i * (1.0f - fadedAlpha) / illuminationSteps;
			var color = spriteRenderer.color;
			spriteRenderer.color = new Color (color.r, color.b, color.b, newAlpha);
		}
	}

	#endregion

	protected void destroy()
	{
		GameManager.Instance.HandleDestroyedHittable (this);
		Destroy (gameObject);
	}
}
