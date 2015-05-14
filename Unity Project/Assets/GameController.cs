using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public GameObject[] spawnPoints = null;
	public GameObject tankPrefab = null;
	public GameObject marker = null;

	[HideInInspector]
	public GameObject activeTank = null;
	[HideInInspector]
	public LinkedList<GameObject> tanks;
	[HideInInspector]
	public GameState State;

	public enum GameState {
		START, TURN, COOLDOWN, GG
	}

	float turnTimer = TURN_LENGTH;

	const float TURN_LENGTH = 15.0f;
	const float COOLDOWN_LENGTH = 5.0f;
	

	// Use this for initialization
	void Start () {
		//TODO: Instantiate a Marker.
		tanks = new LinkedList<GameObject>();
	}

	void StartGame() {
		int i = 1;
		foreach (GameObject spawnPoint in spawnPoints) {
			GameObject newTank = Instantiate(tankPrefab, spawnPoint.transform.position, Quaternion.AngleAxis(90.0f, Vector3.left)) as GameObject;
			tanks.AddLast(newTank);
			newTank.name = "Tank " + (i++).ToString();
		}

		GameObject firstTank = tanks.First.Value;
		TankController controller = firstTank.GetComponent<TankController>();
		activeTank = firstTank;
		controller.BeginTurn();
		this.State = GameState.COOLDOWN;
		turnTimer = 1.0f;
	}

	private void NextTank(int callNum = 0)
	{
		if (callNum > tanks.Count) {
			return;
		}
		
		TankController oldTankController = activeTank.GetComponent<TankController>();
		
		tanks.AddFirst (tanks.Last.Value);
		tanks.RemoveLast ();
		
		activeTank = tanks.First.Value;
		TankController newTankController = activeTank.GetComponent<TankController>();
		if (newTankController.IsDead ()) {
			NextTank(callNum++);
		}

		oldTankController.Endturn ();
	}

	void OnGUI () {
		GUI.BeginGroup (new Rect (Screen.width / 2 - 50, 10, 100, 100));
		//Don't Ask What This Function Does It's Not Important
		string TurnPhase = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase (this.State.ToString ());
		GUI.Box (new Rect(0, 0, 100, 40), "");
		GUI.Label(new Rect(12, 0, 90, 20), TurnPhase);
		GUI.Label(new Rect(12, 20, 90, 20), string.Format("{0:00}", this.turnTimer));
		GUI.EndGroup ();
		
		GUI.BeginGroup (new Rect (Screen.width - 300, Screen.height - 200, 300, 200));
		GUI.Box(new Rect(0,0,300,200), "");
		
		GUI.EndGroup ();
	}
	
	// Update is called once per frame
	void Update () {
		if (activeTank != null && this.marker != null) {
			this.marker.transform.position = this.activeTank.transform.position + new Vector3(0, 5, 0);
		}

		if (State == GameState.START) {
			if (GameObject.Find("Terrain").GetComponent<Terrain>().isLoaded())
			{
				this.StartGame();
				Debug.Log ("Going to pregame cooldown state.");
			} else {
				return;
			}
		}

		if (State == GameState.COOLDOWN) {
			this.turnTimer -= Time.deltaTime;
			if (this.turnTimer < 0)
			{
				this.State = GameState.TURN;
				//Turn start Logic
				this.turnTimer = TURN_LENGTH;
				TankController newTankController = activeTank.GetComponent<TankController>();
				newTankController.BeginTurn();
				Debug.Log ("Turn Cooldown ended.");
			}
			return;
		}

		if (State == GameState.TURN) {
			TankController activeTankController = activeTank.GetComponent<TankController> ();

			//TODOS:
			if (marker != null) {
				//Put Marker above active tank
			}


			//Count the number of dead tanks.
			int numDead = 0;
			foreach (GameObject g in tanks)
			{
				TankController controller = g.GetComponent<TankController>();
				if (controller.IsDead())
				{
					numDead++;
				}
			}

			if (numDead == tanks.Count - 1)
			{
				//Game over.
				this.State = GameState.GG;
				return;
			}

			//Reduce turn tidmer.
			turnTimer -= Time.deltaTime;

			//Check if tank has fired or not
			//end turn if appropriate
			if (turnTimer < 0 || activeTankController.HasShot ()) {
				NextTank ();
				State = GameState.COOLDOWN;
				turnTimer = COOLDOWN_LENGTH;
				Debug.Log ("Turn Ended. Going to cooldown.");
				return;
			}
		}
	}
}
