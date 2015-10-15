/* 
* @Author: jerrytron
* @Date:   2015-08-13 12:16:36
* @Last Modified by:   jerrytron
* @Last Modified time: 2015-08-14 12:25:00
*/

using UnityEngine;
using UnityEngine.UI;
using System;

public class ZeggDevice {
	public string Address { get; set; }
	public string Name { get; set; }
	public int Rssi { get; set; }
	public string ColorKey { get; set; }
	public DateTime LastSeen { get; set; }

	public ZeggDevice(string aAddress, string aName) {
		Address = aAddress;
		Name = aName;
		LastSeen = DateTime.Now;
	}

}
