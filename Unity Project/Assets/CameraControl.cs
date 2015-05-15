using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	private GameController gameController;
	private Vector3 originalPosition;
	private float zoomLevel = 30;
	private float scrollSpeed = 5;
	private GameController.GameState lastState;
	private bool followingBullet = false;
	private bool followingTank = true;

	private Vector3 lastPosition;
	private float mousePanSensitivity = 0.4f;

	// Use this for initialization
	void Start () {
		originalPosition = this.transform.position;

		GameObject gC = GameObject.FindGameObjectWithTag ("GameController");
		this.gameController = gC.GetComponent<GameController> ();
	}

	void Update () {
		GameController.GameState currentState = gameController.State;

		if (currentState == GameController.GameState.TURN && (lastState != GameController.GameState.TURN || followingTank)) {
			// a new turn has begun, thus focus on the current tank
			TankController currentTank = this.gameController.activeTank;

			this.transform.position = new Vector3 (currentTank.transform.position.x,
			                                       currentTank.transform.position.y,
			                                       zoomLevel);
		}

		GameObject projectile = GameObject.FindGameObjectWithTag ("Projectile");
		if (projectile != null) {
			// if there is a projectile follow it with the camera
			followingBullet = true;

			this.transform.position = new Vector3 (projectile.transform.position.x,
			                                       projectile.transform.position.y,
			                                       zoomLevel);
		}

		else {
			followingBullet = false;
		}

		if (currentState == GameController.GameState.GG) {
			this.transform.position = originalPosition;
		}

		lastState = currentState;

		if (currentState != GameController.GameState.COOLDOWN && currentState != GameController.GameState.GG) {
			InputHandler ();
		}
	}

	void InputHandler () {
		// handle zoom
		float change = Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed;
		
		if (change - Mathf.Abs (change) < 0.05f && zoomLevel - change > 10.0f) {
			this.transform.position = new Vector3 (this.transform.position.x,
			                                       this.transform.position.y,
			                                       zoomLevel);
			zoomLevel -= change;
			
			if (zoomLevel < 10.0f) {
				zoomLevel = 10.2f;
			}
		}

		if (Input.GetMouseButtonDown (2)) {
			followingTank = !followingTank;
		}

		// handle panning
		if (Input.GetMouseButton (1) && followingBullet == false && followingTank == false)
		{
			Vector3 deltaMov = Input.mousePosition - lastPosition;
			transform.Translate (deltaMov.x * mousePanSensitivity, deltaMov.y * mousePanSensitivity, 0);
		}

		lastPosition = Input.mousePosition;
	}
}
