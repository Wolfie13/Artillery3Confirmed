using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
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
	
	}
}
