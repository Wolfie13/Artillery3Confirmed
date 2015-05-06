using UnityEngine;
using System.Collections;

public enum InputCode
{
	CUp,
	CLeft,
	CRight,
	CDown,
	Up,
	Left,
	Right,
	Down,
	Fire,
	Pause,
	CameraSwitch
}

public class InputController : MonoBehaviour
{
	public Camera mainCamera;
	public GameObject turnController;

	int currentPlayer;
	GameObject[] playerList;
	CameraControls cControls;
	TurnController turnControllerScript;

	public void PassPlayerList (GameObject[] players)
	{
		playerList = players;
	}

	public void PassCurrentPlayer (int currentPlayerIndex)
	{
		currentPlayer = currentPlayerIndex;
	}

	void Start ()
	{
		cControls = mainCamera.GetComponent (typeof (CameraControls)) as CameraControls;
		turnControllerScript = turnController.GetComponent (typeof (TurnController)) as TurnController;
	}

	// Update is called once per frame
	void Update ()
	{
		// check the buttons
		if (Input.GetKey ("up"))
		{
			cControls.PassInput (InputCode.CUp);
		}

		else if (Input.GetKey ("down"))
		{
			cControls.PassInput (InputCode.CDown);	
		}

		if (Input.GetKey ("left"))
		{
			cControls.PassInput (InputCode.CLeft);	
		}

		else if (Input.GetKey ("right"))
		{
			cControls.PassInput (InputCode.CRight);
		}

		if (Input.GetKey ("right shift"))
		{
			cControls.PassInput (InputCode.CameraSwitch);
		}

		if (Input.GetKey ("w"))
		{
			turnControllerScript.PassInput (InputCode.Up);
		}

		if (Input.GetKey ("s"))
		{
			turnControllerScript.PassInput (InputCode.Down);
		}

		if (Input.GetKey ("a"))
		{
			turnControllerScript.PassInput (InputCode.Left);
		}

		if (Input.GetKey ("d"))
		{
			turnControllerScript.PassInput (InputCode.Right);
		}
	}
}
