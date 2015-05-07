using UnityEngine;
using System.Collections;

[RequireComponent (typeof (InputController))]
[RequireComponent (typeof (TurnOrderList))]
public class TurnController : MonoBehaviour
{
	// TODO: Check if players are dead
	//       Check if above is working

	// Add team turn system

	// camera details
	public Camera mainCamera;
	public GameObject inputController;

	// public variables
	public int turnTime = 20;
	public Transform turnMarker;

	// turn variables
	float currentTurnTime = 0F;
	int numberPlayers = 0;
	int currentPlayer = 0;
	GameObject[] players;

	// GUI variables
	int relativeScreenWidth;
	int timerBoxSize = 25;

	public void PassInput (InputCode inputCode)
	{
		EntityScript entity = players[currentPlayer].GetComponent (typeof (EntityScript)) as EntityScript;
		entity.PassInput (inputCode);
	}

	public void EndTurn ()
	{
		currentTurnTime = 0.0F;
	}

	// Use this for initialization
	void Start ()
	{
		players = GameObject.FindGameObjectsWithTag ("Player");
		turnMarker.transform.position = new Vector3 ( players [currentPlayer].transform.position.x, 
		                                              players [currentPlayer].transform.position.y - 1F, 
		                                              players [currentPlayer].transform.position.z );
		numberPlayers = players.Length;

		InputController input = inputController.GetComponent (typeof (InputController)) as InputController;
		input.PassPlayerList (players);

		TurnOrderList orderGUI = gameObject.GetComponent (typeof (TurnOrderList)) as TurnOrderList;
		string[] playerNames = new string[players.Length];
		for (int i = 0; i < players.Length; i++) 
		{
			playerNames[i] = players[i].name;
		}
		orderGUI.PassPlayers (playerNames);

		CameraControls cControls = mainCamera.GetComponent (typeof (CameraControls)) as CameraControls;
		cControls.PassPlayers (players);
		cControls.PlayerChanged (0);

		EntityScript script = players[currentPlayer].GetComponent (typeof (EntityScript)) as EntityScript;
		script.ActiveSwitch ();
		currentTurnTime = turnTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < players.Length; i++)
		{
			EntityScript playerState = players[i].GetComponent (typeof (EntityScript)) as EntityScript;
			if (playerState.IsDead())
			{
				GameObject.Destroy (players[i]);
				GameObject[] newPlayerList = new GameObject[players.Length - 1];
				for (int j = 0; j < players.Length; j++)
				{
					if (i != j)
					{
						newPlayerList[j] = players[j];
					}
				}

				players = newPlayerList;
				PassList ();
			}
		}

		if (currentTurnTime > 0) 
		{
			currentTurnTime -= Time.deltaTime;
		}

		else
		{
			// check if this is the last player
			// for now it doesn't have any fancy stuff happening between turns
			EntityScript script = players[currentPlayer].GetComponent (typeof (EntityScript)) as EntityScript;
			script.ActiveSwitch ();

			if (currentPlayer < numberPlayers - 1)
			{
				currentPlayer++;
			}
			else
			{
				currentPlayer = 0;
			}

			script = players[currentPlayer].GetComponent (typeof (EntityScript)) as EntityScript;
			script.ActiveSwitch ();
			PassPlayer ();
			currentTurnTime = turnTime;
		}

		UpdateMarker ();
	}

	void OnGUI ()
	{
		relativeScreenWidth = Screen.width / 2 - timerBoxSize / 2;
		GUI.Box (new Rect (relativeScreenWidth, 5, timerBoxSize, timerBoxSize), ((int)(Mathf.Round(currentTurnTime))).ToString());
	}

	void UpdateMarker ()
	{
		turnMarker.transform.position = new Vector3 ( players [currentPlayer].transform.position.x, 
		                                              players [currentPlayer].transform.position.y + 5F, 
		                                              players [currentPlayer].transform.position.z );
	}

	void PassPlayer ()
	{
		TurnOrderList orderGUI = gameObject.GetComponent (typeof (TurnOrderList)) as TurnOrderList;
		orderGUI.PlayerChanged (currentPlayer);
		
		InputController input = inputController.GetComponent (typeof (InputController)) as InputController;
		input.PassCurrentPlayer (currentPlayer);
		
		CameraControls controls = mainCamera.GetComponent (typeof (CameraControls)) as CameraControls;
		controls.PlayerChanged (currentPlayer);
	}
	

	void PassList ()
	{
		InputController input = gameObject.GetComponent (typeof (InputController)) as InputController;
		input.PassPlayerList (players);
		
		TurnOrderList orderGUI = gameObject.GetComponent (typeof (TurnOrderList)) as TurnOrderList;
		string[] playerNames = new string[players.Length];
		for (int i = 0; i < players.Length; i++) 
		{
			playerNames[i] = players[i].name;
		}
		orderGUI.PassPlayers (playerNames);
		
		CameraControls cControls = mainCamera.GetComponent (typeof (CameraControls)) as CameraControls;
		cControls.PassPlayers (players);
	}
}
