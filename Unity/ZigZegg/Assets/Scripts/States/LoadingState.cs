using UnityEngine;
using System.Collections;

public class LoadingState : IStateBase {
	
	private GameManager _manager;
	
	public LoadingState(GameManager aManager) {
		Debug.Log("Entered LoadingState");
		_manager = aManager;
	}

	public void Initialize() {
		_manager.SwitchState(new InitState(_manager));
	}

	public void StateUpdate() {

	}
	
	public void StateOnGUI() {
		
	}

}