using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections.Generic;

public class ScannerState : IStateBase {

	private const string ScanLabel = "Scan      for      Zeggs";
	private const string BleErrorLabel = "Turn On Bluetooth And Restart";
	private const string CancelLabel = "Stop      Scanning";
	private const string UnavailableLabel = "Unavailable";
	private const string ZeggBtnLabel = "Zegg";

	private const string ScanButton = "ScanBtn";
	private const string Zegg1Button = "ZeggOneBtn";
	private const string Zegg2Button = "ZeggTwoBtn";
	private const string Zegg3Button = "ZeggThreeBtn";
	private const string Zegg4Button = "ZeggFourBtn";
	private const string Zegg5Button = "ZeggFiveBtn";
	private const string SignalIcon = "SignalIcon";
	
	private GameManager _manager;
	private DateTime _lastTimeoutCheck;

	private Button _scanBtn;
	private Button[] _buttons = new Button[5];
	private List<string> _zeggAddrMap = new List<string>();

	private bool _scanning = false;
	
	public ScannerState(GameManager aManager) {
		Debug.Log("Entered ScannerState");
		_manager = aManager;
		_manager.GetSceneRoot().SetActive(true);
	}
	
	public void Initialize() {
		Debug.Log("ScannerState Initialize");
		Screen.orientation = ScreenOrientation.AutoRotation;

		_scanBtn = GameObject.Find(ScanButton).GetComponent<Button>();
		//_scanBtn.interactable = false;
		_scanBtn.interactable = true;
		_scanBtn.GetComponentInChildren<Text>().text = ScanLabel;
		_scanBtn.onClick.AddListener(() => OnScanClick(_scanBtn));
		
		_buttons[0] = GameObject.Find(Zegg1Button).GetComponent<Button>();
		//UpdateBtnSignalImage(_buttons[0], 0);
		_buttons[1] = GameObject.Find(Zegg2Button).GetComponent<Button>();
		_buttons[2] = GameObject.Find(Zegg3Button).GetComponent<Button>();
		_buttons[3] = GameObject.Find(Zegg4Button).GetComponent<Button>();
		_buttons[4] = GameObject.Find(Zegg5Button).GetComponent<Button>();

		foreach (Button btn in _buttons) {
			//btn.interactable = true; // For debug - make all buttons available.
			btn.GetComponentInChildren<Text>().text = UnavailableLabel;
			// Make button copy for adding a listener to, or it won't stick.
			Button tempButton = btn;
			tempButton.onClick.AddListener(() => { OnZeggClick(tempButton); });
		}

		if (!_manager.BleInitialized) {
			BluetoothLEHardwareInterface.Initialize(true, false, () => {
				Debug.Log("BLE Initialized");
				//_scanBtn.interactable = true;
				_manager.BleInitialized = true;
			}, (aError) => {
				_scanBtn.interactable = false;
				_scanBtn.GetComponentInChildren<Text>().text = BleErrorLabel;
				Debug.LogError("Failed to initialize BLE Hardware.");
			});
		}

		// Unity Action Example
		//UnityEngine.Events.UnityAction action = () => { myMethod(); myOtherMethod(); }
		//_button.onClick.AddListener(action);
	}
	
	public void StateUpdate() {
		if (_scanning) {
			TimeSpan ts;
			ts = DateTime.Now - _lastTimeoutCheck;
			if (ts.TotalMilliseconds > Config.TimeoutCheckRateMillis) {
				_lastTimeoutCheck = DateTime.Now;
				for (int i = 0; i < _zeggAddrMap.Count; i++) {
					ts = DateTime.Now - _manager.GetZeggByAddress(_zeggAddrMap[i]).LastSeen;
					if (ts.TotalMilliseconds > Config.ZeggAdvrTimeoutMillis) {
						Debug.Log("Last seen: " + ts.TotalMilliseconds);
						Debug.Log("Device timeout: " + _manager.GetZeggByAddress(_zeggAddrMap[i]).ColorKey);
						RemoveZeggDevice(_zeggAddrMap[i]);
						// If we removed a device, rewind by one to check this index next loop.
						i -= 1;
					}
				}
			}
		}
	}
	
	public void StateOnGUI() {
	}

	/* Private Methods */

	private string BytesToString (byte[] aBytes) {
		string result = "";

		foreach (var b in aBytes) {
			result += b.ToString ("X2");
		}

		return result;
	}

	private string AsciiToString(byte[] aBytes) { 
		StringBuilder sb = new StringBuilder(aBytes.Length); 
		foreach (byte b in aBytes) {
			if (b == 0x00) {
				continue;
			}
			sb.Append(b <= 0x7f ? (char)b : '?'); 
		} 
		return sb.ToString(); 
	}

	private void UpdateBtnSignalImage(Button aBtn, int aRssi) {
		Image icon = aBtn.transform.FindChild(SignalIcon).GetComponent<Image>();

		if (aRssi <= Config.RSSIOutOfRangeThreshold) {
			Debug.Log("Signal: Out of Range");
		} else if (aRssi <= Config.RSSIWeakThreshold) {
			Debug.Log("Signal: Weak");
		} else if (aRssi <= Config.RSSIGoodThreshold) {
			Debug.Log("Signal: Good");
		} else {
			Debug.Log("Signal: Strong");
		}

		AnimationCurve curve = AnimationCurve.Linear(1, 0, 0, 1);
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.wrapMode = WrapMode.PingPong;
		clip.SetCurve("", typeof(Image), "color.r", curve);
		clip.SetCurve("", typeof(Image), "color.g", curve);
		clip.SetCurve("", typeof(Image), "color.b", curve);
		clip.SetCurve("", typeof(Image), "color.a", curve);

		icon.GetComponent<Animation>().AddClip(clip, "test");
		icon.GetComponent<Animation>().Play("test");

	}

	private string UppercaseFirst(string aString) {
		// Check for empty string.
		if (string.IsNullOrEmpty(aString)) {
			return string.Empty;
		}
		// Return char and concat substring.
		return char.ToUpper(aString[0]) + aString.Substring(1);
	}

	private void ChangeBtnColorSet(Button aBtn, string aColorKey) {
		ColorBlock colors = aBtn.colors;
		int index = System.Array.IndexOf(Colors.ZeggColorKeys, aColorKey);
		if (index != -1) {
			colors.normalColor = Colors.NormalColors[aColorKey];
			colors.highlightedColor = Colors.HighlightColors[aColorKey];
			colors.pressedColor = Colors.PressedColors[aColorKey];
			aBtn.colors = colors;
			aBtn.GetComponentInChildren<Text>().text = UppercaseFirst(aColorKey) + " " + ZeggBtnLabel;
		} else {
			Debug.LogWarning("Color key not found: " + aColorKey);
		}
	}

	private void RemoveZeggDevice(string aAddress) {
		Debug.Log("ZDevice " + aAddress + " is unavailable!");
		// Look for the device address in our live device map.
		int index = _zeggAddrMap.IndexOf(aAddress);
		if (index != -1) { // Found the device.
			// Iterate from address we need to remove, shifting the other addresses
			// to fill in the empty space.
			_zeggAddrMap.RemoveAt(index);
			// Set the last button, now empty of a valid address, off.
			_buttons[_zeggAddrMap.Count].interactable = false;
			// Update the label to be unavailable.
			_buttons[_zeggAddrMap.Count].GetComponentInChildren<Text>().text = UnavailableLabel;
			for (int i = index; i < _zeggAddrMap.Count; i++) {
				ChangeBtnColorSet(_buttons[i], _manager.GetZeggByAddress(_zeggAddrMap[i]).ColorKey);
			}
		}
	}

	#region Button Listeners

	private void OnScanClick(Button aBtn) {
		Debug.Log("OnScanClick");

		if (_scanning) {
			Debug.Log("Stop BLE scan!");
			_scanBtn.GetComponentInChildren<Text>().text = ScanLabel;
			BluetoothLEHardwareInterface.StopScan();
			_scanning = false;

			_zeggAddrMap.Clear();
			_manager.RemoveZeggDevices();
			foreach (Button btn in _buttons) {
				btn.interactable = false;
				btn.GetComponentInChildren<Text>().text = UnavailableLabel;
			}
		} else {
			Debug.Log("Start BLE scan!");
			_scanBtn.GetComponentInChildren<Text>().text = CancelLabel;
			
			BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (aAddress, aName) => {
				if (aName == Config.ZeggDeviceName) {
					Debug.Log("Adding: " + aAddress + " " + aName);
					_manager.AddZeggDevice(aAddress, aName);
				}
			}, (aAddress, aName, aRssi, aAdvertisingInfo) => {
				if (aAdvertisingInfo != null) {
					// Take the advertising bytes, trim off any 0x00 bytes, and convert to an ASCII string.
					string zeggColor = AsciiToString(aAdvertisingInfo);
					Debug.Log("Ad Color: " + zeggColor);

					// See if we need to remove this device from availability.
					if (zeggColor == Config.ZeggUnavailable) {
						RemoveZeggDevice(aAddress);
					} else {
						// See if the color key is valid.
						int index = System.Array.IndexOf(Colors.ZeggColorKeys, zeggColor);
						Debug.Log("Color index: " + index);
						if (index != -1) {
							// It's valid!
							// Update the device record.
							Debug.Log("Updating device");
							_manager.UpdateZeggDevice(aAddress, aRssi, zeggColor);

							Debug.Log(_manager.GetZeggByAddress(aAddress));

							// See if this device is already listed.

							index = _zeggAddrMap.IndexOf(aAddress);
							Debug.Log("Device index: " + index);
							if (index == -1) {
								// Isn't in the list, add it!
								Debug.Log("Length: " + _zeggAddrMap.Count);
								index = _zeggAddrMap.Count;
								ChangeBtnColorSet(_buttons[index], zeggColor);
								_zeggAddrMap.Add(aAddress);
								_buttons[index].interactable = true;
							}
							// TODO: GET THIS WORKING
							//UpdateBtnSignalImage(_buttons[index], aRssi);
						}
					}	

					Debug.Log(string.Format ("Device: {0} RSSI: {1} Data Length: {2} Bytes: {3}", aName, aRssi, aAdvertisingInfo.Length, BytesToString(aAdvertisingInfo)));
				}
			});
			_scanning = true;
		}
	}

	private void OnZeggClick(Button aBtn) {
		int index = System.Array.IndexOf(_buttons, aBtn);
		if (index < _zeggAddrMap.Count) {
			// TODO: Connect here and confirm maybe?

			_scanning = false;
			BluetoothLEHardwareInterface.StopScan();
			_manager.SelectZegg(_zeggAddrMap[index]);
			Application.LoadLevel(Scenes.CONTROLLER);
		}
	}

	#endregion
	
}