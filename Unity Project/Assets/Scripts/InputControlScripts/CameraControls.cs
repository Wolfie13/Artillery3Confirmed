﻿using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour
{
	public float maxAngle;
	float angleVertical;
	float angleHorizontal;

	bool coupled = false;
	bool started = false;

	// needs to be passed in
	int currentPlayer = 0;
	GameObject[] playerList;

	public void PassPlayers (GameObject[] gameObjectList)
	{
		playerList = gameObjectList;
		started = true;
	}

	public void PlayerChanged (int p)
	{
		currentPlayer = p;
		coupled = true;

		transform.rotation = Quaternion.identity;
		transform.position = new Vector3 (playerList [currentPlayer].transform.position.x,
		                                  playerList [currentPlayer].transform.position.y,
		                                  playerList [currentPlayer].transform.position.z - 10);
	}

	public void PassInput (Input inputCode)
	{
		if (inputCode == Input.CUp && Mathf.Abs (angleVertical) <= maxAngle) 
		{
			transform.Translate (Vector3.up * Time.deltaTime);
			angleVertical += Time.deltaTime;

			if (angleVertical > maxAngle) { angleVertical = maxAngle; }
		}

		else if (inputCode == Input.CDown && Mathf.Abs (angleVertical) <= maxAngle)
		{
			transform.Translate (Vector3.down * Time.deltaTime);
			angleVertical -= Time.deltaTime;

			if (angleVertical < -maxAngle ) { angleVertical = -maxAngle; }
		}

		if (inputCode == Input.CLeft && Mathf.Abs (angleHorizontal) <= maxAngle)
		{
			transform.Translate (Vector3.right * Time.deltaTime);
			angleHorizontal += Time.deltaTime;

			if (angleHorizontal > maxAngle) { angleHorizontal = maxAngle; }
		}

		else if (inputCode == Input.CRight && Mathf.Abs (angleHorizontal) <= maxAngle)
		{
			transform.Translate (Vector3.left * Time.deltaTime);
			angleHorizontal -= Time.deltaTime;

			if (angleHorizontal < -maxAngle) { angleHorizontal = -maxAngle; }
		}

		if (inputCode == Input.CameraSwitch)
		{
			coupled = !coupled;
		}
	}

	void Start ()
	{
		PlayerChanged (0);
	}

	// Update is called once per frame
	void Update ()
	{
		if (started)
		{
			if (coupled == true)
			{
				transform.LookAt (playerList [currentPlayer].transform);			
			}

			else
			{
				transform.rotation = Quaternion.identity;
			}
		}
	}
}
