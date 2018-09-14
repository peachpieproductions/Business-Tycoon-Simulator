using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Canvas_Example : MonoBehaviour {
	public GameObject WindowSizeText;
	public GameObject LocalCoordsText;
	public GameObject GlobalCoordsText;
	public GameObject InputX;
	public GameObject InputY;
	public int X;
	public int Y;
	void Start () {
	
	}
	void Update () {
	LocalCoordsText.GetComponent<Text>().text = "Cursor Local Position :    "+HardwareCursor.GetLocalPosition();
	GlobalCoordsText.GetComponent<Text>().text = "Cursor Global Position :  "+HardwareCursor.GetPosition();
	WindowSizeText.GetComponent<Text>().text = "Window Size :                  ("+Screen.width+".0, "+Screen.height+".0)";
	int.TryParse(InputX.GetComponent<Text>().text,out X);
	int.TryParse(InputY.GetComponent<Text>().text,out Y);
	}
	public void SetPosition() {
	HardwareCursor.SetPosition(X,Y);
	}
	public void SetLocalPosition() {
	HardwareCursor.SetLocalPosition(X,Y);
	}
	public void RandomLocalMovement() {
	HardwareCursor.SimulateAutoLocalMove(Random.Range(0,Screen.width),Random.Range(0,Screen.height),0.2f);		
	}
}
