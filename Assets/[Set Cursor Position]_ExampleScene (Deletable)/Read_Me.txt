Supports: Unity 3x,4x,5x,2017x,2018x

With over 300 purchases and nearly 100% happy customers this is a reliable solution.
Works on Windows only. Both for unity free and pro.

Summary:
"Set Cursor Position" is an Unity Plugin that gives you the ability to control the actual position of your Hardware (OS) Cursor in your screen via Scripting.
Not only that but you can also :
-Emulate mouse clicks
-Emulate scrolling
-Save/load positions
-Cursor can be set to go outside of the unity window !!
[NEW] Multi Monitor Support
[NEW] Fullscreen Support
[NEW] JoyStick Support
There are tons of creative ways to use it and implement it to your games/programms.
The package comes with 2 Example Scenes and many scripts to help you start and understand its use.
Can be combined with Unity's current API to achieve great effects !

What you actually get:

 ~Code examples
	- Canvas & OnGUI Examples
	- Javascript Examples [Upon request]
	- LocalPosition Examples
	- GlobalPosition Examples
	- Store/Load Position Examples
	- JoyStick Example
	- ScrollWheel Example
	- Movement Simulation Examples
	- KeyBinding Examples
  + a few more
 
 ~Source Code
 
 ~Forum/Email support
  (As far as its possible and related to the plugin)
 
 ~New Scripting API.
  ~CrossSceneSupport : boolean;
  -HardwareCursor.SetPosition (Vector2 Pos);
  -HardwareCursor.SetPosition (int x,int y);
  -HardwareCursor.SetLocalPosition (Vector2 Pos);
  -HardwareCursor.SetLocalPosition (int x,int y);
  -HardwareCursor.GetPosition ();
  -HardwareCursor.GetLocalPosition ();
  -HardwareCursor.SetSavesLength (int Saves);
  -HardwareCursor.SavePosition ();
  -HardwareCursor.SavePosition (int SaveNumber);
  -HardwareCursor.LoadPosition ();
  -HardwareCursor.LoadPosition (int LoadNumber);
  -HardwareCursor.SimulateMove (Vector2 FinalPosition,float Speed);
  -HardwareCursor.SimulateMove (int FinalPositionX,int FinalPositionY,float Speed);
  -HardwareCursor.SimulateMove (Vector2 FinalPosition,float Speed,bool Automaticaly);
  -HardwareCursor.SimulateMove (int FinalPositionX,int FinalPositionY,float Speed,bool Automaticaly);
  -HardwareCursor.SimulateAutoMove (Vector2 FinalPosition,float Speed);
  -HardwareCursor.SimulateAutoMove (int FinalPositionX,int FinalPositionY,float Speed);
  -HardwareCursor.SimulateLocalMove (Vector2 FinalPosition,float Speed);
  -HardwareCursor.SimulateLocalMove (int FinalPositionX,int FinalPositionY,float Speed);
  -HardwareCursor.SimulateLocalMove (Vector2 FinalPosition,float Speed,bool Automaticaly);
  -HardwareCursor.SimulateLocalMove (int FinalPositionX,int FinalPositionY,float Speed,bool Automaticaly);
  -HardwareCursor.SimulateAutoLocalMove (Vector2 FinalPosition,float Speed);
  -HardwareCursor.SimulateAutoLocalMove (int FinalPositionX,int FinalPositionY,float Speed);
  -HardwareCursor.SimulateController (float Horizontal,float Vertical);
  -HardwareCursor.SimulateController (float Horizontal,float Vertical,float Speed);
  -HardwareCursor.SimulateSmoothController (float Horizontal,float Vertical,float Speed,float SpeedScale);
  -HardwareCursor.ScrollWheel (int Direction);
  -HardwareCursor.LeftClick();
  -HardwareCursor.LeftClickDown();
  -HardwareCursor.LeftClickUp();
  -HardwareCursor.RightClick();
  -HardwareCursor.RightClickDown();
  -HardwareCursor.RightClickUp();
  -HardwareCursor.MiddleClick();
  -HardwareCursor.MiddleClickDown();
  -HardwareCursor.MiddleClickUp();
  -HardwareCursor.LeftClickEquals(KeyCode KeycodeId);
  -HardwareCursor.RightClickEquals(KeyCode KeycodeId);
  -HardwareCursor.MiddleClickEquals(KeyCode KeycodeId);
  
Unity uselful API:
 -Cursor.visible : boolean;
 -Cursor.lockState : CursorLockMode
 -Input.GetKey(KeyCode.Mouse0) // Mouse1 and Mouse2 and so on...
 -Input.GetKeyDown(KeyCode.Mouse0)
 -Input.GetKeyUp(KeyCode.Mouse0) 
and many more...

Example Uses:
Simulate mouse movement with external controllers like the ones from PS3 or Xbox.
Restrict cursor within the window or a certain area.
Rotate a character with a mouse button and when u press it down turn the cursor invisible and store its position so u can turn it visible and load its position when u release the button.
Lock the cursor in a desired position not only in the center by constantly calling HardwareCursor.SetCursorPosition() every frame or so.
Make the cursor teleport in random coordinates.
and many more, just use your imagination and creativity...

How to install the plugin:
 The first thing you may want to do is use it , but you will have read the Installation_Guide.txt

Script Variables:
---------------------------------------------------------------
CrossSceneSupport : boolean;
If checked during awake will prevent the current gameobject from beeing destoyed during scene change. (recommended to keep true)

API Documentation:
---------------------------------------------------------------
"HardwareCursor.SetPosition (int x,int y);"
This function can be called at any time and requires 2 integers ( int for short ) one for the X coordinate and one for the Y coordinate of the screen.
Once its called the Cursor will automaticaly teleport to the (GLOBAL) coordinates that was beeing told to go.
---------------------------------------------------------------
"HardwareCursor.SetPosition (Vector2 Pos);"
Same as above but takes a Vecotor2 instead.
---------------------------------------------------------------
"HardwareCursor.GetPosition ();"
This function can be called at any time and returns the real cursor values related to your screen and not unity.
---------------------------------------------------------------
"HardwareCursor.SetLocalPosition (int x,int y);"
This function can be called at any time and requires 2 integers ( int for short ) one for the X coordinate and one for the Y coordinate of the Unity-local screen.
Once its called the Cursor will automaticaly teleport to the (LOCAL) coordinates that was beeing told to go.
---------------------------------------------------------------
"HardwareCursor.SetLocalPosition (Vector2 Pos);"
Same as above but takes a Vecotor2 instead.
---------------------------------------------------------------
"HardwareCursor.GetLocalPosition ();"
Same as Input.mousePosition.
---------------------------------------------------------------
"HardwareCursor.SetSavesLength (int Saves);"
Is one very basic function to be called if the user wants to use "HardwareCursor.SavePosition (int);" or "HardwareCursor.LoadPosition (int);"
What it actualy does is to tell the system to create as many Save Slots as the number defined in the brackets.
That's why it requires 1 integer ( int for short ) to determine the save slots length.
If you know from the beginning how many save slots you should determine it from the beginning but the new system is automated now so it doesnt really matter :P
Note: it can be called as many times as you want and at any time although it is recomended to call it once at the Start () function.
--------------------------------------------------------------
"HardwareCursor.SavePosition (int SaveNumber);"
This function can be called at any time and requires 1 integers ( int for short ) that determines the Save Slot you want to save.
Save Slots are beeing created by the use of "HardwareCursor.SetSavesLength (int);" (Read Above)
In other words this function will Store the current mouse position so it can be used later with "HardwareCursor.LoadPosition (int);" (Read Below)
Overwriting an already saved slot is possible and supported !!
Please note this saves the positions in (GLOBAL) coordinates.
--------------------------------------------------------------
"HardwareCursor.SavePosition();"
It is used as a QuickSave default int value is 0 (zero). In other words it is the same as using "HardwareCursor.SavePosition(0)"
--------------------------------------------------------------
"HardwareCursor.LoadPosition (int LoadNumber);"
This function can be called at any time and requires 1 integers ( int for short ) that determines the Save Slot you want to load.
Save Slots are beeing created by the use of "HardwareCursor.SetSavesLength (int);" (Read Above)
In other words this function will Load/Teleport the Cursor to the position that was stored in this Save Slot by "HardwareCursor.SavePosition (int);" (Read Above)
So it is important to call first "HardwareCursor.SavePosition (int);" and then "HardwareCursor.LoadPosition (int);" or else the default position will be loaded ( 0,0 ).
Loading the same saved position more than one times is possible and supported !!
--------------------------------------------------------------
"HardwareCursor.LoadPosition();"
It is used as a QuickLoad default int value is 0 (zero). In other words it is the same as using "HardwareCursor.LoadPosition(0)"
--------------------------------------------------------------
"HardwareCursor.SimulateMove (Vector2 FinalPosition,float Speed,bool Automaticaly)"
Once its called the Cursor will start moving toward the coordinates that was beeing told to go.
This function can be called at any time and requires one Vector2 as the final destination in Global coordinates, one float as the speed value that determines the speed
of the cursor, one boolean that determines whether the cursor should persist no matter what to go to its final position and stop until it does so or if the function
should be a one time only meaning that you will have to call it in some form of update (see the example).
--------------------------------------------------------------
"HardwareCursor.SimulateMove (int FinalPositionX,int FinalPositionY,float Speed,bool Automaticaly);"
Same as above but takes 2 integers instead.
--------------------------------------------------------------
"HardwareCursor.SimulateAutoMove (Vector2 FinalPosition,float Speed)"
Same as above but as default the bool is set to true;
--------------------------------------------------------------
"HardwareCursor.SimulateAutoMove (int FinalPositionX,int FinalPositionY,float Speed);"
Same as above but takes 2 integers instead.
--------------------------------------------------------------
"HardwareCursor.SimulateMove (Vector2 FinalPosition,float Speed)"
Same as SimulateMove but as default the bool is set to false;
--------------------------------------------------------------
"HardwareCursor.SimulateMove (int FinalPositionX,int FinalPositionY,float Speed);"
Same as above but takes 2 integers instead.
--------------------------------------------------------------
"HardwareCursor.SimulateLocalMove (Vector2 FinalPosition,float Speed,bool Automaticaly)"
Once its called the Cursor will start moving toward the coordinates that was beeing told to go.
This function can be called at any time and requires one Vector2 as the final destination in Unity-local coordinates, one float as the speed value that determines the speed
of the cursor, one boolean that determines whether the cursor should persist no matter what to go to its final position and stop until it does so or if the function
should be a one time only meaning that you will have to call it in some form of update (see the example).
--------------------------------------------------------------
"HardwareCursor.SimulateLocalMove (int FinalPositionX,int FinalPositionY,float Speed,bool Automaticaly);"
Same as above but takes 2 integers instead.
--------------------------------------------------------------
"HardwareCursor.SimulateLocalMove (Vector2 FinalPosition,float Speed)"
Same as SimulateLocalMove but as default the bool is set to false;
--------------------------------------------------------------
"HardwareCursor.SimulateLocalMove (int FinalPositionX,int FinalPositionY,float Speed);"
Same as above but takes 2 integers instead.
--------------------------------------------------------------
"SimulateAutoLocalMove (Vector2 FinalPosition,float Speed)"
Same as SimulateLocalMove but as default the bool is set to true;
--------------------------------------------------------------
"HardwareCursor.SimulateAutoLocalMove (int FinalPositionX,int FinalPositionY,float Speed);"
Same as above but takes 2 integers instead.
--------------------------------------------------------------
"HardwareCursor.SimulateController (float Horizontal,float Vertical,float Speed)" --> [RECOMMENDED]
This function can be called at any time and requires 3 float values one float as the horizontal input , on as the vertical input and one as the speed value that
determines the speed of the cursor. This function should be called once every frame to work and is frame dependent (see the example).
--------------------------------------------------------------
"HardwareCursor.SimulateController (float Horizontal,float Vertical);"
Same as above but without a speed parameter , this could be used to test controllers raw data.
--------------------------------------------------------------
"HardwareCursor.SimulateSmoothController (float Horizontal,float Vertical,float Speed,float SpeedScale)" --> [NOT RECOMMENDED]
Is like the SimulateController function but a filter is applyied that doent make any visual difference and neither does improve the performance so its not recomended to use this function.
The filter or "SpeedScale" values should range between 0 and 1.
Use the SimulateController instead !!
--------------------------------------------------------------
"HardwareCursor.ScrollWheel(int Direction)"
Once called it will keep scrolling with a speed of the set int.
(Remember , positive values will scoll upwards[forward] and negative values will scroll downwards[backward])
--------------------------------------------------------------
"HardwareCursor.LeftClick()"
A fully left mouse click.
--------------------------------------------------------------
"HardwareCursor.LeftClickDown()"
A left click down - useful for dragging stuff or drag-drop with the keyboard.
--------------------------------------------------------------
"HardwareCursor.LeftClickUp()"
A left click release.
--------------------------------------------------------------
"HardwareCursor.RightClick()"
A fully right mouse click.
--------------------------------------------------------------
"HardwareCursor.RightClickDown()"
A right click down.
--------------------------------------------------------------
"HardwareCursor.RightClickUp()"
A right click release.
--------------------------------------------------------------
"HardwareCursor.MiddleClick()"
A fully middle mouse click.
--------------------------------------------------------------
"HardwareCursor.MiddleClickDown()"
A middle click down.
--------------------------------------------------------------
"HardwareCursor.MiddleClickUp()"
A middle click release.
--------------------------------------------------------------
"HardwareCursor.LeftClickEquals(KeyCode KeycodeId)"
Binds the given keycode as a left mouse. In other words when you press that keycode a left mouseclick will happen, when you release it a left mouse release will happen.
--------------------------------------------------------------
"HardwareCursor.RightClickEquals(KeyCode KeycodeId)"
Binds the given keycode as a right mouse. In other words when you press that keycode a right mouseclick will happen, when you release it a right mouse release will happen.
--------------------------------------------------------------
"HardwareCursor.MiddleClickEquals(KeyCode KeycodeId)"
Binds the given keycode as a middle mouse. In other words when you press that keycode a middle mouseclick will happen, when you release it a middle mouse release will happen.
--------------------------------------------------------------
--------------------------------------------------------------

Missing .dll files:
 There are supposed to be 2 .dll files in the Plugins folder , in case the .dll files are missing you can get them from
 ...Unity/Editor/Data/Mono/lib/mono/2.0/ in your computer and then copy the files named "System.Windows.Forms.dll"
, "System.Drawing.dll" and paste them inside the Plugins folder in your unity Project directory !!
These 2 .dll files are vital and the asset will not work without them !

Conctact Info :
Email : DarknessBlade.Original@gmail.com
Tutorial Videos : https://www.youtube.com/DarknessBladeOrigin
Forum Thread: http://forum.unity3d.com/threads/242832-Official-Set-Cursor-Position?p=1606714#post1606714