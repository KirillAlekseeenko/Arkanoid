using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LearningImage : MonoBehaviour, IPointerClickHandler {

	private bool hided = false;

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		if (hided)
			return;
		hided = true;
		AnimationUtils.MakeAnimatorInvisible (GetComponent<Animator> ());
		StartCoroutine (waitForAnimationEnd ());
	}

	#endregion

	private IEnumerator waitForAnimationEnd()
	{
		while (GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName("Show") || GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime < 1 ) {
			yield return new WaitForSecondsRealtime (0.2f);
		}
		gameObject.SetActive (false);
		Time.timeScale = 1;
	}
}
