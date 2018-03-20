using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputType {

	bool Pause { get; }
	bool SpecialAction { get; }
}
