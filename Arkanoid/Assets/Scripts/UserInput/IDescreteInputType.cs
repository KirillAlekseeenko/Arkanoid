using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDescreteInputType : IInputType {
	
	bool Left { get; }
	bool Right { get; }
}
