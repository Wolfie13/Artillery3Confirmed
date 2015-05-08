using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour
{
	public float maxAngle;
	float angleVertical;
	float angleHorizontal;

	int lookSpeed = 7;

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
		if ( started )
		{
			currentPlayer = p;
			coupled = true;

			transform.rotation = Quaternion.identity;
			transform.position = new Vector3 (playerList [currentPlayer].transform.position.x,
		                                 	  playerList [currentPlayer].transform.position.y + 10,
		                                 	  playerList [currentPlayer].transform.position.z - 35);


		}
	}

	public void PassInput (InputCode inputCode)
	{
		if (inputCode == InputCode.CUp && Mathf.Abs (angleVertical) <= maxAngle) 
		{
			transform.Translate (Vector3.up * Time.deltaTime * lookSpeed);
			angleVertical += Time.deltaTime;

			if (angleVertical > maxAngle) { angleVertical = maxAngle; }
		}

		else if (inputCode == InputCode.CDown && Mathf.Abs (angleVertical) <= maxAngle)
		{
			transform.Translate (Vector3.down * Time.deltaTime * lookSpeed);
			angleVertical -= Time.deltaTime;

			if (angleVertical < -maxAngle ) { angleVertical = -maxAngle; }
		}

		if (inputCode == InputCode.CLeft && Mathf.Abs (angleHorizontal) <= maxAngle)
		{
			transform.Translate (Vector3.right * Time.deltaTime * lookSpeed);
			angleHorizontal += Time.deltaTime;

			if (angleHorizontal > maxAngle) { angleHorizontal = maxAngle; }
		}

		else if (inputCode == InputCode.CRight && Mathf.Abs (angleHorizontal) <= maxAngle)
		{
			transform.Translate (Vector3.left * Time.deltaTime * lookSpeed);
			angleHorizontal -= Time.deltaTime;

			if (angleHorizontal < -maxAngle) { angleHorizontal = -maxAngle; }
		}

		if (inputCode == InputCode.CameraSwitch)
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
