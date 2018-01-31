using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovementState;

public class UserInput : MonoBehaviour {

	[SerializeField] private Platform platform;

	private delegate void MovePlatform();
	private MovePlatform moveAction;

	private Automat movementAutomat;

	// Use this for initialization
	void Start () {
		movementAutomat = new Automat (platform);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (GameManager.Instance.IsStarted && !GameManager.Instance.IsEnded) {
			movementAutomat.ChangeState (new TransitionData (Input.GetKey (KeyCode.A), Input.GetKey (KeyCode.D)));
			if (Input.GetKey (KeyCode.Space)) {
				platform.SpecialAction ();
			}
		}
		else {
			if (!GameManager.Instance.IsStarted && !GameManager.Instance.Transition) {
				if (Input.GetKey (KeyCode.Space)) {
					GameManager.Instance.IsStarted = true;
				}
			}
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			GameManager.Instance.PauseHandle ();
		}
	}

}

namespace MovementState
{
	public struct TransitionData
	{
		public bool A;
		public bool D;
		public TransitionData(bool _A, bool _D)
		{
			A = _A;
			D = _D;
		}
	}

	public abstract class AutomatState
	{
		protected IAutomat automat;

		public AutomatState(IAutomat _automat)
		{
			automat = _automat;
		}

		public abstract void Transition (TransitionData transitionData);
	}

	public interface IAutomat
	{
		AutomatState State { get; set; }

		AutomatState LeftMovementState { get; }
		AutomatState RightMovementState { get; }
		AutomatState StopState { get; }
	}

	public class Automat : IAutomat
	{
		private AutomatState state;

		private LeftMovement leftMovementState;
		private RightMovement rightMovementState;
		private Stop stopState;

		private Platform platform;
		
		public Automat(Platform _platform)
		{
			platform = _platform;
			
			leftMovementState = new LeftMovement (this);
			rightMovementState = new RightMovement (this);
			stopState = new Stop (this);

			state = stopState;
		}

		public AutomatState State {
			get {
				return state;
			}
			set {
				if (value is LeftMovement) {
					platform.MoveLeft ();
				} else if (value is RightMovement) {
					platform.MoveRight ();
				} else if (value is Stop) {
					platform.Stop ();
				}
				state = value;
			}
		}

		public AutomatState LeftMovementState {
			get {
				return leftMovementState;
			}
		}

		public AutomatState RightMovementState {
			get {
				return rightMovementState;
			}
		}

		public AutomatState StopState {
			get {
				return stopState;
			}
		}

		public void ChangeState (TransitionData transitionData)
		{
			state.Transition (transitionData);
		}
	}

	public class LeftMovement : AutomatState
	{
		public LeftMovement (IAutomat _automat) : base (_automat){}
		public override void Transition (TransitionData transitionData)
		{
			var A = transitionData.A;
			var D = transitionData.D;
			if (!A && D) {
				automat.State = automat.RightMovementState;
			} else if ((!A && !D) || (A && D)) {
				automat.State = automat.StopState;
			}
		}
	}
	public class RightMovement : AutomatState
	{
		public RightMovement (IAutomat _automat) : base (_automat){}
		public override void Transition (TransitionData transitionData)
		{
			var A = transitionData.A;
			var D = transitionData.D;
			if (A && !D) {
				automat.State = automat.LeftMovementState;
			} else if ((!A && !D) || (A && D)) {
				automat.State = automat.StopState;
			}
		}
	}
	public class Stop : AutomatState
	{
		public Stop (IAutomat _automat) : base (_automat){}
		public override void Transition (TransitionData transitionData)
		{
			var A = transitionData.A;
			var D = transitionData.D;
			if (!A && D) {
				automat.State = automat.RightMovementState;
			} else if (A && !D) {
				automat.State = automat.LeftMovementState;
			}
		}
	}
}
