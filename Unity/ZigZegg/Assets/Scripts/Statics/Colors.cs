/* 
* @Author: jerrytron
* @Date:   2015-08-13 16:42:55
* @Last Modified by:   jerrytron
* @Last Modified time: 2015-08-14 10:17:24
*/

using UnityEngine;
using System.Collections.Generic;

public static class Colors {

	public const string PurpleKey = "purple";
	public const string OrangeKey = "orange";
	public const string GreenKey = "green";
	public const string BlueKey = "blue";
	public const string MaroonKey = "maroon";

	public static readonly string[] ZeggColorKeys = {
		PurpleKey,
		OrangeKey,
		GreenKey,
		BlueKey,
		MaroonKey
	};

	public static readonly Dictionary<string, Color> NormalColors = new Dictionary<string, Color>() {
		{ PurpleKey, new Color(178.0f / 255.0f, 81.0f  / 255.0f, 212.0f / 255.0f, 1.0f) },
		{ OrangeKey, new Color(251.0f / 255.0f, 172.0f / 255.0f, 54.0f  / 255.0f, 1.0f) },
		{ GreenKey,  new Color(134.0f / 255.0f, 192.0f / 255.0f, 63.0f  / 255.0f, 1.0f) },
		{ BlueKey,   new Color(54.0f  / 255.0f, 74.0f  / 255.0f, 210.0f / 255.0f, 1.0f) },
		{ MaroonKey, new Color(137.0f / 255.0f, 24.0f  / 255.0f, 16.0f  / 255.0f, 1.0f) }
	};

	public static readonly Dictionary<string, Color> HighlightColors = new Dictionary<string, Color>() {
		{ PurpleKey, new Color(201.0f / 255.0f, 133.0f / 255.0f, 225.0f / 255.0f, 1.0f) },
		{ OrangeKey, new Color(252.0f / 255.0f, 197.0f / 255.0f, 114.0f / 255.0f, 1.0f) },
		{ GreenKey,  new Color(170.0f / 255.0f, 211.0f / 255.0f, 121.0f / 255.0f, 1.0f) },
		{ BlueKey,   new Color(14.0f  / 255.0f, 29.0f  / 255.0f, 224.0f / 255.0f, 1.0f) },
		{ MaroonKey, new Color(172.0f / 255.0f, 93.0f  / 255.0f, 88.0f  / 255.0f, 1.0f) }
	};

	public static readonly Dictionary<string, Color> PressedColors = new Dictionary<string, Color>() {
		{ PurpleKey, new Color(71.0f  / 255.0f, 32.0f / 255.0f, 85.0f / 255.0f, 1.0f) },
		{ OrangeKey, new Color(100.0f / 255.0f, 69.0f / 255.0f, 21.0f / 255.0f, 1.0f) },
		{ GreenKey,  new Color(54.0f  / 255.0f, 77.0f / 255.0f, 25.0f / 255.0f, 1.0f) },
		{ BlueKey,   new Color(22.0f  / 255.0f, 30.0f / 255.0f, 84.0f / 255.0f, 1.0f) },
		{ MaroonKey, new Color(55.0f  / 255.0f, 10.0f / 255.0f, 6.0f  / 255.0f, 1.0f) }
	};

	public static readonly Color OutOfRangeSignalColor = Color.black;
	public static readonly Color WeakSignalColor = Color.red;
	public static readonly Color GoodSignalColor = Color.yellow;
	public static readonly Color StrongSignalColor = Color.green;
	// new Color(63.0f / 255.0f, 193.0f / 255.0f, 0.0f / 255.0f, 1.0f);

	/*public static readonly string[] ZeggColorKeys = {
		"purple",
		"orange",
		"green",
		"blue",
		"maroon"
	};
	public static readonly Color[] NormalColors = {
		new Color(178.0f / 255.0f, 81.0f  / 255.0f, 212.0f / 255.0f, 1.0f), // purple
		new Color(251.0f / 255.0f, 172.0f / 255.0f, 54.0f  / 255.0f, 1.0f), // orange
		new Color(134.0f / 255.0f, 192.0f / 255.0f, 63.0f  / 255.0f, 1.0f), // green
		new Color(54.0f  / 255.0f, 74.0f  / 255.0f, 210.0f / 255.0f, 1.0f), // blue
		new Color(137.0f / 255.0f, 24.0f  / 255.0f, 16.0f  / 255.0f, 1.0f)  // maroon
	};
	public static readonly Color[] HighlightColors = {
		new Color(201.0f / 255.0f, 133.0f / 255.0f, 225.0f / 255.0f, 1.0f), // purple
		new Color(252.0f / 255.0f, 197.0f / 255.0f, 114.0f / 255.0f, 1.0f), // orange
		new Color(170.0f / 255.0f, 211.0f / 255.0f, 121.0f / 255.0f, 1.0f), // green
		new Color(14.0f  / 255.0f, 29.0f  / 255.0f, 224.0f / 255.0f, 1.0f), // blue
		new Color(172.0f / 255.0f, 93.0f  / 255.0f, 88.0f  / 255.0f, 1.0f)  // maroon
	};
	public static readonly Color[] PressedColors = {
		new Color(71.0f  / 255.0f, 32.0f / 255.0f, 85.0f / 255.0f, 1.0f), // purple
		new Color(100.0f / 255.0f, 69.0f / 255.0f, 21.0f / 255.0f, 1.0f), // orange
		new Color(54.0f  / 255.0f, 77.0f / 255.0f, 25.0f / 255.0f, 1.0f), // green
		new Color(22.0f  / 255.0f, 30.0f / 255.0f, 84.0f / 255.0f, 1.0f), // blue
		new Color(55.0f  / 255.0f, 10.0f / 255.0f, 6.0f  / 255.0f, 1.0f)  // maroon
	};*/

}