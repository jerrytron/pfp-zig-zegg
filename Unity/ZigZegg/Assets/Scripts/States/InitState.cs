using UnityEngine;
using System.Collections;

public class InitState : IStateBase {
	
	private GameManager _manager;
		
	public InitState(GameManager aManager) {
		Debug.Log("Entered InitState");
		_manager = aManager;
	}

	public void Initialize() {
		// We want any scene to be able to act as the starting scene,
		// so after initial loading, check where we are and change state accordingly.
		switch(Application.loadedLevelName) {
			case Scenes.SCANNER:
				_manager.SwitchState(new ScannerState(_manager));
				break;
			case Scenes.CONTROLLER:
				_manager.SwitchState(new ControllerState(_manager));
				return;
			default:
				Debug.LogError("Unknown scene, can't begin state machine: " + Application.loadedLevelName);
				break;
		}
	}
	
	public void StateUpdate() {

	}
	
	public void StateOnGUI() {
		
	}


	
}
