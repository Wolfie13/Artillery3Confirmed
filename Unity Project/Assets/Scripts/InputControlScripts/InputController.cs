using UnityEngine;
using System.Collections;

public enum Input
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

	int currentPlayer;
	GameObject[] playerList;

	public void PassPlayerList (GameObject[] players)
	{
		playerList = players;
	}

	public void PassCurrentPlayer (int currentPlayerIndex)
	{
		currentPlayer = currentPlayerIndex;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// check the buttons
		
	}
}
