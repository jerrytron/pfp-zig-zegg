using UnityEngine;
using System.Collections;

public class FatalErrorState : IStateBase {
	
	private GameManager _manager;
	
	public FatalErrorState(GameManager aManager) {
		Debug.Log("Entered FatalErrorState");
		_manager = aManager;
		_manager.GetSceneRoot().SetActive(true);
	}
	
	public void Initialize() {
		
	}
	
	public void StateUpdate() {
		
	}
	
	public void StateOnGUI() {
		
	}
	
}