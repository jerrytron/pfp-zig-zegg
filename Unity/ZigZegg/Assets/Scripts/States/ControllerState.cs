using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ControllerState : IStateBase {

	private const string BackButton = "BackBtn";
	private const string LeftSlider = "LeftSlider";
	private const string RightSlider = "RightSlider";

	private Button _backBtn;
	private Slider _leftSlider;
	private Slider _rightSlider;
	
	private GameManager _manager;
	private ZeggDevice _zegg;
	private bool _connecting;
	private bool _connected;
	private string _serviceUuid = "2220";
	private string _readCharacteristicUuid = "2221";
	private string _writeCharacteristicUuid = "2222";
	
	public ControllerState(GameManager aManager) {
		Debug.Log("Entered ControllerState");
		_manager = aManager;
		_manager.GetSceneRoot().SetActive(true);
		Debug.Log("Zegg count: " + _manager.ZeggCount());
	}
	
	public void Initialize() {
		Debug.Log("Initializing ControllerState");
		Screen.orientation = ScreenOrientation.LandscapeLeft;

		_backBtn = GameObject.Find(BackButton).GetComponent<Button>();
		_backBtn.onClick.AddListener(() => OnBackClick(_backBtn));

		_leftSlider = GameObject.Find(LeftSlider).GetComponent<Slider>();
		_leftSlider.value = 0;
		_leftSlider.onValueChanged.AddListener(OnLeftSliderChange);

		_rightSlider = GameObject.Find(RightSlider).GetComponent<Slider>();
		_rightSlider.value = 0;
		_rightSlider.onValueChanged.AddListener(OnRightSliderChange);

		_connected = false;
		_connecting = false;
		_zegg = _manager.GetSelectedZegg();
		if (_zegg == null) {
			Application.LoadLevel(Scenes.SCANNER);
			return;
		}

		ConnectToZegg(_zegg.Address);
	}
	
	public void StateUpdate() {

	}
	
	public void StateOnGUI() {
		
	}

	/* Private Methods */

	private string FullUuid(string aUuid) {
		return "0000" + aUuid + "-0000-1000-8000-00805f9b34fb";
	}

	private bool IsEqual(string aUuid1, string aUuid2){
		if (aUuid1.Length == 4) {
			aUuid1 = FullUuid(aUuid1);
		}
		if (aUuid2.Length == 4) {
			aUuid2 = FullUuid(aUuid2);
		}

		return (aUuid1.ToUpper().CompareTo(aUuid2.ToUpper()) == 0);
	}

	private void SendMotorCommand(string aAddress, uint aMotorNumber, uint aValue, Action<string> aAction) {
		// Motor command as byte.
		byte cmd = Convert.ToByte('m');
		// Motor number.
		byte mtr = Convert.ToByte(aMotorNumber);
		// Value to send (0 - 255).
		byte val = Convert.ToByte(aValue);
		// Slider value as byte.
		byte[] data = new byte[] {
			cmd,  
			mtr,
			val   
		};

		SendBytes(aAddress, data, aAction);
	}

	private void SendByte(string aAddress, byte aValue, Action<string> aAction) {
		byte[] data = new byte[] { aValue };
		BluetoothLEHardwareInterface.WriteCharacteristic(aAddress, _serviceUuid, _writeCharacteristicUuid, data, data.Length, true, aAction);
	}
	
	private void SendBytes(string aAddress, byte[] aData, Action<string> aAction) {
		BluetoothLEHardwareInterface.WriteCharacteristic (aAddress, _serviceUuid, _writeCharacteristicUuid, aData, aData.Length, false, aAction);
	}

	private void ConnectToZegg(string aAddress) {
		if (!_connecting) {
			if (_connected) {
				_connecting = false;
			} else {
				BluetoothLEHardwareInterface.ConnectToPeripheral(aAddress, (aaAddress) => {
					Debug.Log("PART ONE");
				},
				(aaAddress, aServiceUuid) => {
					Debug.Log("PART TWO");
				},
				(aaAddress, aServiceUuid, aCharacteristicUuid) => {
					Debug.Log("PART THREE");
					// Discovered characteristics.
					if (IsEqual(aServiceUuid, _serviceUuid)) {
						_connected = true;

						if (IsEqual(aCharacteristicUuid, _readCharacteristicUuid)) {
							BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(aaAddress, _serviceUuid, _readCharacteristicUuid, (deviceAddress, notification) => {
								
							}, (deviceAddress2, characteristic, data) => {

								if (deviceAddress2.CompareTo(aaAddress) == 0) {
									if (IsEqual(aCharacteristicUuid, _readCharacteristicUuid)) {
										if (data.Length == 0) {
											//Button1Highlight.SetActive (false);
										} else {
											if (data[0] == 0x01) {
												//Button1Highlight.SetActive (true);
											} else {
												//Button1Highlight.SetActive (false);
											}
										}
									}
								}
								
							});
						} else if (IsEqual(aCharacteristicUuid, _writeCharacteristicUuid)) {
							//LEDButton.SetActive (true);
						}
					}
				}, (aaAddress) => {
					Debug.Log("PART FOUR");
					// This will get called when the device disconnects.
					// Be aware that this will also get called when the disconnect
					// is called above. Both methods get called for the same action.
					// This is for backwards compatibility.
					_connected = false;
				});

			_connecting = true;
			}
		}
	}

	private void OnConnect(string aAddress) {
		// NOT BEING USED
		Debug.Log("OnConnect");
	}

	private void DisconnectFromZegg(string aAddress, Action<string> aAction) {
		BluetoothLEHardwareInterface.DisconnectPeripheral(aAddress, aAction);
	}

	#region Button Listeners

	private void OnBackClick(Button aBtn) {
		Debug.Log("OnBackClick");
		
		// Remove listeners so we can turn off motors manually.
		_leftSlider.onValueChanged.RemoveAllListeners();
		_rightSlider.onValueChanged.RemoveAllListeners();
		// Send motor shut off to first motor.
		SendMotorCommand(_zegg.Address, 1, 0, (aCharacteristicUuid) => {
			Debug.Log("Send Left Slider Data");
			// First motor shut off sent, ready for the second.
			SendMotorCommand(_zegg.Address, 2, 0, (aCharacterictisUuid) => {
				// Motors are shut off, ready to disconnect.
				DisconnectFromZegg(_zegg.Address, (aAddress) => {
					Debug.Log("DisconnectFromZegg");
					_manager.UnselectZegg();
					Application.LoadLevel(Scenes.SCANNER);
					/*BluetoothLEHardwareInterface.DeInitialize(() => {
						Debug.Log("DeInitialize");
						
					});*/
				});
			});
		});
	}

	private void OnLeftSliderChange(float aValue) {
		SendMotorCommand(_zegg.Address, 1, Convert.ToUInt16(aValue), (aCharacteristicUuid) => {
			Debug.Log("Send Left Slider Data");
		});
	}

	private void OnRightSliderChange(float aValue) {
		SendMotorCommand(_zegg.Address, 2, Convert.ToUInt16(aValue), (aCharacteristicUuid) => {
			Debug.Log("Send Right Slider Data");
		});
	}

	#endregion
	
}