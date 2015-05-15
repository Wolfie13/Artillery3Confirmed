using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public GameObject[] spawnPoints = null;
	public GameObject tankPrefab = null;
	public MarkerControl marker = null;
	public AudioClip  newTurnAudio = null;

	[HideInInspector]
	public TankController activeTank = null;
	[HideInInspector]
	public LinkedList<TankController> tanks;
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
		tanks = new LinkedList<TankController>();
	}

	void StartGame() {
		int i = 1;
		foreach (GameObject spawnPoint in spawnPoints) {
			GameObject t = Instantiate(tankPrefab, spawnPoint.transform.position, Quaternion.AngleAxis(90.0f, Vector3.left)) as GameObject;
			TankController newTank = t.GetComponent<TankController>();
			tanks.AddLast(newTank);
			newTank.gameObject.name = "Tank " + (i++).ToString();
		}

		TankController firstTank = tanks.First.Value;
		activeTank = firstTank;
		firstTank.BeginTurn();
		this.State = GameState.COOLDOWN;
		turnTimer = 1.0f;
	}

	private void NextTank()
	{
		NextTank (0);
	}

	private void NextTank(int callNum)
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
		GUIStyle labelStyle = new GUIStyle (GUI.skin.GetStyle ("Label"));

		// Timer & Turn State
		GUI.BeginGroup (new Rect (Screen.width / 2 - 50, 10, 100, 50));

			GUI.Box (new Rect (0, 0, 100, 40), "");
			string TurnPhase = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase (this.State.ToString ());

			labelStyle.alignment = TextAnchor.UpperCenter;
			labelStyle.fontStyle = FontStyle.Bold;
			labelStyle.fontSize = 15;

			GUI.Label (new Rect (0, 0, 100, 20), TurnPhase, labelStyle);
			GUI.Label (new Rect (0, 15, 100, 20), ((int)(Mathf.Round (this.turnTimer))).ToString (), labelStyle);

		GUI.EndGroup ();

		// Tank info
		GUI.BeginGroup (new Rect (Screen.width - 150, Screen.height - 100, 150, 100));

			if (activeTank != null) {
					labelStyle.alignment = TextAnchor.MiddleLeft;

					GUI.Box (new Rect (0, 0, 140, 90), "");
					GUI.Label (new Rect (5, 0, 140, 20), activeTank.name, labelStyle);

					TankController tankCont = activeTank.GetComponent<TankController> ();
					string healthString = "Health: " + tankCont.health;
					GUI.Label (new Rect (5, 20, 140, 20), healthString, labelStyle);

					GUI.Label (new Rect (5, 40, 140, 25), "Gun A: " + (int) tankCont.GetGunAngle() + " P: " + (int) tankCont.GetGunPower(), labelStyle);
					GUI.Label (new Rect (20, 65, 140, 25), activeTank.GetSelectedWeaponName(), labelStyle);
					if (GUI.Button (new Rect (120, 65, 20, 25), ">"))
					{
						tankCont.SwitchWeapon(true);
					}

					if (GUI.Button (new Rect (0, 65, 20, 25), "<"))
					{
						tankCont.SwitchWeapon(false);
					}
			}
		GUI.EndGroup ();
	}

	// Update is called once per frame
	void Update () {
		if (activeTank != null && this.marker != null) {
			this.marker.targetObject = activeTank.gameObject;
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
				if(newTurnAudio)
				{
					AudioSource.PlayClipAtPoint (newTurnAudio, transform.position);
				}
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
			if (tanks != null)
			{
				foreach (TankController t in tanks)
				{
					if (t.IsDead())
					{
						numDead++;
					}
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
