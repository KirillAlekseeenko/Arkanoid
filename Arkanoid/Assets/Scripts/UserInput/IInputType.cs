using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputType {
	
	bool Left { get; }
	bool Right { get; }
	bool SpecialAction { get; }

	bool Pause { get; }
	bool Start { get; }
}
