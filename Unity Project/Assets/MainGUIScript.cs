using UnityEngine;
using System;
using System.Collections;

public class MainGUIScript : MonoBehaviour
{
	// provisional numbers
	int menuWidth = 250;
	int menuHeight = 350; 

	// skirmish menu numbers
	public string[] mapList;
	Vector2 scrollPos;
	int listButtonHeight = 35;
	int skirmishMenuWidth = 400;
	int skirmishMenuHeight = 250;
	int mapPickOptionNumber = 0;
	int listStartOffset;
	int listWidth = 150;
	int listHeight;

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

		listStartOffset = buttonHeightOffset - 5;
		listHeight = skirmishMenuHeight - listStartOffset - 5;

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
			Application.LoadLevel ( 1 );
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

		GUILayout.BeginArea (new Rect (relativeStartWidth, relativeStartHeight, skirmishMenuWidth, skirmishMenuHeight));

		GUI.Box (new Rect (0, 0, skirmishMenuWidth, skirmishMenuHeight), "Skirmish Menu");

		scrollPos = GUI.BeginScrollView (new Rect (5, listStartOffset, listWidth + 20, listHeight), scrollPos, new Rect (0, 0, listWidth, mapList.Length * listButtonHeight));
		mapPickOptionNumber = GUI.SelectionGrid (new Rect (0, 0, listWidth, mapList.Length * listButtonHeight), mapPickOptionNumber, mapList, 1);
		GUI.EndScrollView ();

		GUI.Label (new Rect (listWidth + 40, listStartOffset, skirmishMenuWidth - (listWidth + 40), 30), "Number of Players");

		GUILayout.EndArea ();
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
