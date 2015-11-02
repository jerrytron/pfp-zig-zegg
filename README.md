# Zig Zegg
##### Created By: Jerry Belich ([@j3rrytron](https://twitter.com/j3rrytron) - [jerrytron.com](http://jerrytron.com))
##### Video Series: Prototyping for Play
##### Segment: 11. Making Connected Games
##### Link: http://jerrytron.com/pfp

This is the repository for the code and project files required for the Zig Zegg project as demonstrated in segment 11 of the Prototyping for Play course by Jerry Belich, published by O'Reilly Media.

# RFduino

## Board / Library Setup
1. Download and install the [**Arduino IDE**])(https://www.arduino.cc/en/Main/Software), then open Arduino.
2. Navigate to *Preferences*, and find the *Additional Boards Manager URLs* field.
3. Insert the following URL: *http://rfduino.com/package_rfduino_index.json*.
4. Close the window, and navigate to *Tools* -> *Boards:* -> *Boards Manager*.
5. Finally, enter *RFduino* into the search field. It should review an entry for RFduino, which you can simple click *install* to add. 

## Firmware
1. Plug the RFduino into it's USB shield, and plug that into your computer.
2. Select the RFduino board via *Tools* -> *Boards:* -> *RFduino*, listed at the bottom.
3. Select the port for the plugged in board via the list at *Tools* -> *Port*
4. Open the [**RFduino/ZigZegg/ZigZegg.ino**](https://github.com/jerrytron/pfp-zig-zegg/blob/master/RFduino/ZigZegg/ZigZegg.ino) file from this repository, and click *Upload* to flash the firmware.

# Unity Application
There is one requirement missing in order to build this application. You need to purchase the [**Bluetooth LE for iOS and Android**](https://www.assetstore.unity3d.com/en/#!/content/26661) plugin from the Unity Asset store ($10 at time of writing). After purchase, simply import into the project and it should build. If the plugin is no longer available, please get in touch with me.
