using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager {

	public bool BleInitialized { get; set; }

	private GameObject _sceneRoot;
	private IStateBase _activeState;
	private Dictionary<string, ZeggDevice> _zeggDeviceList;
	private string _selectedZeggAddress;

	public GameManager() {
	}

	public void Initialize() {
		Debug.Log("GameManager Initiailize!");

		_sceneRoot = GameObject.Find(SceneObjects.SCENE_ROOT);
		if (_sceneRoot != null) {
			_sceneRoot.SetActive(false);
		} else {
			Debug.LogError("A 'SceneRoot' GameObject is required in each scene.");
		}

		SwitchState(new LoadingState(this));
	}

	public void StateOnEnable() {
	}

	public void StateOnDisable() {
	}

	public void StateOnLevelWasLoaded() {
		Debug.Log("StateOnLevelWasLoaded");
		_sceneRoot = GameObject.Find(SceneObjects.SCENE_ROOT);
		if (_sceneRoot != null) {
			_sceneRoot.SetActive(false);
		} else {
			Debug.LogError("A 'SceneRoot' GameObject is required in each scene.");
		}
		switch(Application.loadedLevelName) {
			case Scenes.SCANNER:
				SwitchState(new ScannerState(this));
				break;
			case Scenes.CONTROLLER:
				SwitchState(new ControllerState(this));
				break;
			default:
				Debug.LogError("Unknown scene, can't change state: " + Application.loadedLevelName);
				break;
		}
	}

	#region Monobehaviour Methods

	public void StateStart() {
	}

	public void StateUpdate() {
		if (_activeState != null) {
			_activeState.StateUpdate();
		}
	}

	public void StateOnGUI() {
		if (_activeState != null) {
			_activeState.StateOnGUI();
		}
	}

	public void StateOnApplicationPause(bool aPaused) {
		if (aPaused) {
			Debug.Log("*** PAUSED ***");
		} else {
			Debug.Log("*** UNPAUSED ***");
		}
	}

	public void StateOnApplicationFocus() {
		Debug.Log("OnApplicationFocus");
	}

	#endregion

	public GameObject GetSceneRoot() {
		return _sceneRoot;
	}

	public void SwitchState(IStateBase aNewState) {
		_activeState = aNewState;
		Debug.Log("Switching State!");
		_activeState.Initialize();
	}

	public void AddZeggDevice(string aAddress, string aName) {
		if (_zeggDeviceList == null) {
			_zeggDeviceList = new Dictionary<string, ZeggDevice> ();
		}

		if (!_zeggDeviceList.ContainsKey(aAddress)) {
			_zeggDeviceList[aAddress] = new ZeggDevice(aAddress, aName);
		}
	}

	public void UpdateZeggDevice(string aAddress, int aRssi, string aZeggColor) {
		if (_zeggDeviceList != null) {
			_zeggDeviceList[aAddress].Rssi = aRssi;
			_zeggDeviceList[aAddress].ColorKey = aZeggColor;
			_zeggDeviceList[aAddress].LastSeen = DateTime.Now;
		}
	}

	public ZeggDevice GetZeggByAddress(string aAddress) {
		if (_zeggDeviceList != null) {
			return _zeggDeviceList[aAddress];
		}
		return null;
	}

	public void RemoveZeggDevices() {
		Debug.Log("RemoveZeggDevices");
		if (_zeggDeviceList != null) {
			_zeggDeviceList.Clear();
			_selectedZeggAddress = null;
		}
	}

	public int ZeggCount() {
		return _zeggDeviceList.Count;
	}

	public void UnselectZegg() {
		_selectedZeggAddress = null;
	}

	public void SelectZegg(string aAddress) {
		Debug.Log("SelectZegg: " + aAddress);
		_selectedZeggAddress = aAddress;
	}

	public ZeggDevice GetSelectedZegg() {
		Debug.Log("GetSelectedZegg: " + _selectedZeggAddress);
		if (_zeggDeviceList.ContainsKey(_selectedZeggAddress)) {
			Debug.Log("Got it!");
			return _zeggDeviceList[_selectedZeggAddress];
		}
		return null;
	}

}