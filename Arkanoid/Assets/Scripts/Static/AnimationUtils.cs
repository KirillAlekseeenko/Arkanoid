using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtils
{
	/// <summary>
	/// transition between two panels
	/// </summary>
	public static void MakePanelTransition(Animator from, Animator to)
	{
		MakeAnimatorInvisible (from);
		MakeAnimatorVisible (to);
	}
	public static void MakeAnimatorVisible(Animator animator)
	{
		animator.SetBool ("isVisible", true);
	}
	public static void MakeAnimatorInvisible(Animator animator)
	{
		animator.SetBool ("isVisible", false);
	}
}

