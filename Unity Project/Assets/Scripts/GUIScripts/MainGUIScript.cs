using UnityEngine;
using System;
using System.Collections;

public class MainGUIScript : MonoBehaviour
{
	// provisional numbers
	int menuWidth = 250;
	int menuHeight = 350; 

	// skirmish menu numbers
	int skirmishMenuWidth = 400;
	int skirmishMenuHeight = 250;

	// button variables
	int buttonWidth = 200;
	int buttonHeight = 30;
	int buttonHeightOffset = 45;

	// dependant on the window's size
	int relativeStartWidth;
	int relativeStartHeight;
	int startButtonHeight;

	int relativeButtonWidth;

	Action GUIMethod;

	void Start ()
	{
		GUIMethod = MainMenuScreen;
	}

	void OnGUI ()
	{
		relativeStartWidth = Screen.width / 2 - menuWidth / 2;
		relativeStartHeight = Screen.height / 2 - menuHeight / 2;
		relativeButtonWidth = Screen.width / 2 - buttonWidth / 2;
		startButtonHeight = relativeStartHeight + buttonHeightOffset - 10;

		if (GUIMethod != null)
		{
			GUIMethod ();
		}
	}

	void MainMenuScreen ()
	{
		// main box
		GUI.Box (new Rect ( relativeStartWidth, relativeStartHeight, menuWidth, menuHeight ), "Main Menu" );

		// button for "New Game"
		if ( GUI.Button (new Rect ( relativeButtonWidth, startButtonHeight, buttonWidth, buttonHeight ), "New Game" ))
		{
			// random stuff
			Application.LoadLevel ( "Dummy" );
		}

		// button for "Load Game"
		if ( GUI.Button (new Rect ( relativeButtonWidth, startButtonHeight + (buttonHeightOffset), buttonWidth, buttonHeight ), "Load Game" ))
		{
			// purposefully empty
		}

		if ( GUI.Button (new Rect ( relativeButtonWidth, startButtonHeight + (buttonHeightOffset * 2), buttonWidth, buttonHeight ), "Skirmish Game" ))
		{
			GUIMethod = SkirmishScreen;
		}

		if ( GUI.Button (new Rect ( relativeButtonWidth, startButtonHeight + (buttonHeightOffset * 3), buttonWidth, buttonHeight ), "Multiplayer Game" ))
		{
			GUIMethod = MultiplayerGameScreen;
		}

		if ( GUI.Button (new Rect ( relativeButtonWidth, startButtonHeight + (buttonHeightOffset * 5), buttonWidth, buttonHeight ), "Options" ))
		{
			GUIMethod = OptionsScreen;
		}

		if ( GUI.Button (new Rect ( relativeButtonWidth, startButtonHeight + (buttonHeightOffset * 6), buttonWidth, buttonHeight ), "Quit Game" ))
		{
			Application.Quit ();
		}
	}

	void SkirmishScreen ()
	{
		relativeStartWidth = Screen.width / 2 - skirmishMenuWidth / 2;
		relativeStartHeight = Screen.height / 2 - skirmishMenuHeight / 2;

		GUI.Box (new Rect ( relativeStartWidth, relativeStartHeight, skirmishMenuWidth, skirmishMenuHeight ), "Skirmish Menu" );
	}

	void MultiplayerGameScreen ()
	{
		GUI.Box (new Rect ( relativeStartWidth, relativeStartHeight, menuWidth, menuWidth ), "Multiplayer Options" );
	}

	void OptionsScreen ()
	{
		GUI.Box (new Rect ( relativeStartWidth, relativeStartHeight, menuWidth, menuHeight ), "Options" );
	}
}
