using UnityEngine;
using System.Collections;

public class JoyStick_Example : MonoBehaviour {

public float Speed = 5;		
	void Update () {

		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) { // If we press any of these A-S-D-W or any arrow key
			HardwareCursor.SimulateController(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),Speed); // Their input will be translated into cursor movement
		}
}}