using UnityEngine;
using System.Collections;

public interface IStateBase {

	// Run immediately after the constructor.
	// Can't switch states in the constructor.
	void Initialize();
	// Run on the Unity Update call.
	void StateUpdate();
	// Run on the Unity OnGUI call.
	void StateOnGUI();

}