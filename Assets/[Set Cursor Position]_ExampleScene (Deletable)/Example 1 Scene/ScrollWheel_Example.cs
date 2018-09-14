using UnityEngine;
using System.Collections;
public class ScrollWheel_Example : MonoBehaviour {

	void Update () {

		if (Input.GetKeyDown(KeyCode.K)) {
			HardwareCursor.MiddleClick(); //Translate "K" keydown to a full middle mouse click
		}
				
		if (Input.GetKey(KeyCode.O)) {
			HardwareCursor.ScrollWheel(1); //Translate "O" keydown to a positive (forward) scrollwheel
		}
		if (Input.GetKey(KeyCode.I)) {
			HardwareCursor.ScrollWheel(-1); //Translate "I" keydown to a negative (backward) scrollwheel
		}
		
		HardwareCursor.MiddleClickEquals(KeyCode.J); //Defines the "J" key as a middle mouse

	}

}