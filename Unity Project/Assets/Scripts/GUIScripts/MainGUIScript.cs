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

	void OnGUI ()
	{
		relativeStartWidth = Screen.width / 2 - menuWidth / 2;
		relativeStartHeight = Screen.height / 2 - menuHeight / 2;
		startButtonHeight = relativeStartHeight + buttonHeightOffset;

		// main box
		GUI.Box ( new Rect ( relativeStartWidth, relativeStartHeight, menuWidth, menuHeight ), "Main Menu" );

		// button for "New Game"
		if ( GUI.Button ( new Rect ( relativeStartWidth, startButtonHeight, buttonWidth, buttonHeight ), "New Game" ))
		{

		}

		// button for "Load Game"
		if ( GUI.Button ( new Rect ( relativeStartWidth, startButtonHeight + (buttonHeightOffset), buttonWidth, buttonHeight ), "Load Game" ))
		{
			// purposefully empty
		}
	}
}
