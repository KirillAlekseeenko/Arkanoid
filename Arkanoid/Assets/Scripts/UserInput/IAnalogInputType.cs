using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnalogInputType : IInputType  {

	bool Movement (out Vector3 pos);
}
