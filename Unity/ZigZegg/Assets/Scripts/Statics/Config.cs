using UnityEngine;
//using System.Collections;

public static class Config {
	// UUID for Jerrytron BLE devices.
	public const string IBeaconUuid = "ACC8A68A-85A8-4564-BA5A-0B39B8D24025";
	// Broadcast device name.
	public const string ZeggDeviceName = "ZigZegg";
	// Breadcast ad info cancel availability string.
	public const string ZeggUnavailable = "unavail";

	public const int TimeoutCheckRateMillis = 100;
	public const int ZeggAdvrTimeoutMillis = 600;

	public const int RSSIOutOfRangeThreshold = -75;
	public const int RSSIWeakThreshold = -65;
	public const int RSSIGoodThreshold = -55;

}