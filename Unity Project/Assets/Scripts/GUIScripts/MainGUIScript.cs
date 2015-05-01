using UnityEngine;
using System.Collections;

public class MainGUIScript : MonoBehaviour
{
	// provisional numbers
	int menuWidth = 250;
	int menuHeight = 350; 

	// button variables
	int buttonWidth = 200;
	int buttonHeight = 30;
	int buttonHeightOffset = 45;

	// dependant on the window's size
	int relativeStartWidth;
	int relativeStartHeight;
	int startButtonHeight;

	int relativeButtonWidth;

	void OnGUI ()
	{
		relativeStartWidth = Screen.width / 2 - menuWidth / 2;
		relativeStartHeight = Screen.height / 2 - menuHeight / 2;
		relativeButtonWidth = Screen.width / 2 - buttonWidth / 2;
		startButtonHeight = relativeStartHeight + buttonHeightOffset;

		// main box
		GUI.Box ( new Rect ( relativeStartWidth, relativeStartHeight, menuWidth, menuHeight ), "Main Menu" );

		// button for "New Game"
		if ( GUI.Button ( new Rect ( relativeButtonWidth, startButtonHeight, buttonWidth, buttonHeight ), "New Game" ))
		{
			Application.LoadLevel ( "Dummy" );
		}

		// button for "Load Game"
		if ( GUI.Button ( new Rect ( relativeButtonWidth, startButtonHeight + (buttonHeightOffset), buttonWidth, buttonHeight ), "Load Game" ))
		{
			// purposefully empty
		}

		if ( GUI.Button ( new Rect ( relativeButtonWidth, startButtonHeight + (buttonHeightOffset * 2), buttonWidth, buttonHeight ), "Multiplayer Game" ))
		{
			// purposefully empty
		}

		if ( GUI.Button ( new Rect ( relativeButtonWidth, startButtonHeight + (buttonHeightOffset), buttonWidth, buttonHeight ), "Quit Game" ))
		{
			Application.Quit ();
		}
	}
}
