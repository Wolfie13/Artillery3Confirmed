using UnityEngine;
using System.Collections;

public class TurnController : MonoBehaviour
{
	// TODO: Add list of players to be displayed
	//       Add animation
	//       Test movement
	// 		 Make 3D triangle

	// public variables
	public int turnTime = 20;
	public Transform turnMarker;
	public GUIText timer;

	// turn variables
	float currentTurnTime = 0F;
	int numberPlayers = 0;
	int currentPlayer = 0;
	GameObject[] players;

	// Use this for initialization
	void Start ()
	{
		players = GameObject.FindGameObjectsWithTag ("Player");
		turnMarker.transform.position = new Vector3 ( players [currentPlayer].transform.position.x, 
		                                              players [currentPlayer].transform.position.y - 1F, 
		                                              players [currentPlayer].transform.position.z );
		numberPlayers = players.Length;

		currentTurnTime = turnTime;
		timer.text = ((int)(Mathf.Round(turnTime))).ToString();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentTurnTime > 0) 
		{
			currentTurnTime -= Time.deltaTime;
		}

		else
		{
			// check if this is the last player
			// for now it doesn't have any fancy stuff happening between turns
			if (currentPlayer < numberPlayers - 1)
			{
				currentPlayer++;
			}
			else
			{
				currentPlayer = 0;
			}

			currentTurnTime = turnTime;
		}

		UpdateMarker ();
		timer.text = ((int)(Mathf.Round(currentTurnTime))).ToString();
	}

	void UpdateMarker ()
	{
		turnMarker.transform.position = new Vector3 ( players [currentPlayer].transform.position.x, 
		                                              players [currentPlayer].transform.position.y + 1F, 
		                                              players [currentPlayer].transform.position.z );
	}
}
