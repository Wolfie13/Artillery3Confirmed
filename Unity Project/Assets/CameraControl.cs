using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	private GameController gameController;
	private Vector3 originalPosition;
	private GameController.GameState pastState;
	private float zoomLevel = 30;
	private float scrollSpeed = 5;

	private GameController.GameState lastState;

	private Vector3 lastPosition;
	private float mousePanSensitivity = 5;

	// Use this for initialization
	void Start () {
		originalPosition = this.transform.position;

		GameObject gC = GameObject.FindGameObjectWithTag ("GameController");
		this.gameController = gC.GetComponent<GameController> ();
	}

	void Update () {
		GameController.GameState currentState = gameController.State;

		if (currentState == GameController.GameState.TURN && lastState != GameController.GameState.TURN) {
			// a new turn has begun, thus focus on the current tank
			GameObject currentTank = this.gameController.activeTank;
			TankController currentTankState = currentTank.GetComponent<TankController> ();

			this.transform.position = new Vector3 (currentTank.transform.position.x,
			                                       currentTank.transform.position.y,
			                                       currentTank.transform.position.z + zoomLevel);
		}

		GameObject projectile = GameObject.FindGameObjectWithTag ("Projectile");
		if (projectile != null) {
			// if there is a projectile follow it with the camera
			this.transform.position = new Vector3 (projectile.transform.position.x,
			                                       projectile.transform.position.y,
			                                       projectile.transform.position.z + zoomLevel);
		}

		if (currentState == GameController.GameState.GG) {
			this.transform.position = originalPosition;
		}

		pastState = currentState;

		InputHandler ();
	}

	void InputHandler () {
		// handle zoom
		if (zoomLevel > 10.0f) {
			zoomLevel -= Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed;
		}

		// handle panning
		// TODO: NOT WORKING
		if (Input.GetMouseButton (1))
		{
			Vector3 deltaMov = Input.mousePosition - lastPosition;
			transform.Translate (deltaMov.x * mousePanSensitivity, deltaMov.y * mousePanSensitivity, 0);
		}

		lastPosition = Input.mousePosition;
	}
}
