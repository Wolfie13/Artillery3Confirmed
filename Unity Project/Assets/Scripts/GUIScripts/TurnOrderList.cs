using UnityEngine;
using System.Collections;

public class TurnOrderList : MonoBehaviour
{
	// menu variables
	int menuWidth = 120; 
	int textHeight = 25;
	int textHorizontalOffset = 10;
	
	int textLength;
	int menuHeight;

	// needs to be passed in
	int currentPlayer = 0;
	string[] playerList;

	// public (since it is easier)
	public GUISkin selectedPlayerSkin;

	// TODO: Make current player red

	public void PassPlayers (string[] playerNames)
	{
		playerList = playerNames;
	}

	public void PlayerChanged (int p)
	{
		currentPlayer = p;
	}

	// Update is called once per frame
	void OnGUI ()
	{
		textLength = menuWidth - 20;
		GUI.BeginGroup (new Rect (5, 5, menuWidth, menuHeight));

		int textStartPosition = 20;
		for (int i = 0; i < playerList.Length; i++)
		{
			GUIContent text = new GUIContent (playerList[i]);
			if (i != currentPlayer)
			{
				GUI.Label (new Rect(textHorizontalOffset, textStartPosition, textLength, textHeight), text);
			}
			else
			{
				GUISkin normal = GUI.skin;
				GUI.skin = selectedPlayerSkin;
				GUI.Label (new Rect(textHorizontalOffset, textStartPosition, textLength, textHeight), text);
				GUI.skin = normal;
			}

			textStartPosition += (int)GUI.skin.label.CalcHeight (text, textLength) + 5;
		}

		menuHeight = textStartPosition;
		GUI.Box (new Rect (0, 0, menuWidth, menuHeight), "Turn Order");

		GUI.EndGroup ();
	}
}
