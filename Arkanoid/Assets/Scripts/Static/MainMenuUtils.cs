using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainMenuUtils
{
	/// <summary>
	/// transition between two panels
	/// </summary>
	public static void MakePanelTransition(Animator from, Animator to)
	{
		MakePanelInvisible (from);
		MakePanelVisible (to);
	}
	public static void MakePanelVisible(Animator panel)
	{
		panel.SetBool ("isVisible", true);
	}
	public static void MakePanelInvisible(Animator panel)
	{
		panel.SetBool ("isVisible", false);
	}
}

