#include <RFduinoBLE.h>

/* Defines */
#define DEBUG 0

/* Constants */
const uint8_t kRxPin = 0;
const uint8_t kTxPin = 1;
const char kZeggName[] = "ZigZegg";
const char kZeggColor[] = "purple";
const char kZeggUnavailable[] = "unavail";

// Motor Control Pins
const uint8_t kMotorOnePin = 2;
const uint8_t kMotorTwoPin = 3;

// Motor IDs
const uint8_t kMotorOne = 1;
const uint8_t kMotorTwo = 2;

const uint16_t kBaudRate = 9600;

/* Variables */
int8_t _rssiStrength = 0;

void setup() {
  // For debugging.
#if DEBUG == 1
  Serial.begin(kBaudRate, kRxPin, kTxPin);
#endif

  // Setup our motor control pins.
  pinMode(kMotorOnePin, OUTPUT);
  pinMode(kMotorTwoPin, OUTPUT);
  analogWrite(kMotorOnePin, 0);
  analogWrite(kMotorTwoPin, 0);

  // this is the data we want to appear in the advertisement
  // (if the deviceName and advertisementData are too long to fit into the 31 byte
  // ble advertisement packet, then the advertisementData is truncated first down to
  // a single byte, then it will truncate the deviceName)

  // Max characters between name and ad data is 15 bytes
  RFduinoBLE.deviceName = kZeggName;
  RFduinoBLE.advertisementData = kZeggColor; // Max 8 bytes

  // Start the BLE stack.
  RFduinoBLE.begin();
}

void loop() {
  RFduino_ULPDelay(INFINITE);
}

void RFduinoBLE_onAdvertisement(bool aStart){
}

void RFduinoBLE_onConnect() {
  RFduinoBLE.advertisementData = kZeggUnavailable;
}

void RFduinoBLE_onDisconnect() {
  RFduinoBLE.advertisementData = kZeggColor;
  
  // Turn off motors.
  analogWrite(kMotorOnePin, 0);
  analogWrite(kMotorTwoPin, 0);
}

void RFduinoBLE_onReceive(char *aData, int aLength) {
  char command = aData[0];
  
  switch (command) {
    // Set Motor Speed
    case 'm': {
      uint8_t motorNumber = aData[1];
      uint8_t motorSpeed = aData[2];
      if (motorNumber == kMotorOne) {
        analogWrite(kMotorOnePin, motorSpeed);
      } else if (motorNumber == kMotorTwo) {
        analogWrite(kMotorTwoPin, motorSpeed);
      }
      break;
    }
    // Reset the RFduino
    case 'r': {
      RFduino_systemReset();
      break;
    }
    // Get Temperature
    case 't': {
      // Celsius is 0, Fahrenheit is 1
      uint8_t type = aData[1];
      float temp = RFduino_temperature(type);
      RFduinoBLE.sendFloat(temp);
      break;
    }
    // Get Signal Strength
    case 's': {
      RFduinoBLE.sendInt(_rssiStrength);
      break;
    }
  }
}

// Function automatically called by firmware.
void RFduinoBLE_onRSSI(int8_t aRssi) {
  // Save the latest signal strength value.
  _rssiStrength = aRssi;
}
