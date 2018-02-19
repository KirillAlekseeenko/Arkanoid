using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		AudioManager.Instance.PlayOnHoverButtonEffect ();
	}

	#endregion

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		AudioManager.Instance.PlayOnClickButtonEffect ();
	}

	#endregion
}
