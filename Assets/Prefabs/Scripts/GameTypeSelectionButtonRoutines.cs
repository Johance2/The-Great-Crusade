﻿using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class GameTypeSelectionButtonRoutines : MonoBehaviour
{
    public void toggleChange()
    {
        if (GetComponent<Toggle>().isOn == true)
        {
            if (GetComponent<Toggle>() != GlobalDefinitions.newGameToggle.GetComponent<Toggle>())
                GlobalDefinitions.newGameToggle.GetComponent<Toggle>().isOn = false;
            if (GetComponent<Toggle>() != GlobalDefinitions.savedGameToggle.GetComponent<Toggle>())
                GlobalDefinitions.savedGameToggle.GetComponent<Toggle>().isOn = false;
            if (GetComponent<Toggle>() != GlobalDefinitions.commandFileToggle.GetComponent<Toggle>())
                GlobalDefinitions.commandFileToggle.GetComponent<Toggle>().isOn = false;
        }
    }

    public void newSavedGameOK()
    {
        if (GlobalDefinitions.newGameToggle.GetComponent<Toggle>().isOn)
        {
            GlobalDefinitions.writeToLogFile("newSavedGameOK: Starting new game");

            // Since at this point we know we are starting a new game and not running the command file, remove the command file
            if (!GlobalDefinitions.commandFileBeingRead)
                GlobalDefinitions.deleteCommandFile();

            GlobalDefinitions.removeGUI(transform.parent.gameObject);
            GameControl.setUpStateInstance.GetComponent<SetUpState>().executeNewGame();
        }
        else if (GlobalDefinitions.savedGameToggle.GetComponent<Toggle>().isOn)
        {
            GlobalDefinitions.writeToLogFile("newSavedGameOK: Starting saved game");

            // Since at this point we know we are starting a new game and not running the command file, remove the command file
            if (!GlobalDefinitions.commandFileBeingRead)
                GlobalDefinitions.deleteCommandFile();

            GlobalDefinitions.removeGUI(transform.parent.gameObject);
            GameControl.setUpStateInstance.GetComponent<SetUpState>().executeSavedGame();
        }
        else if (GlobalDefinitions.commandFileToggle.GetComponent<Toggle>().isOn)
        {
            GlobalDefinitions.writeToLogFile("newSavedGameOK: Executing command file");

            GlobalDefinitions.removeGUI(transform.parent.gameObject);
            GameControl.setUpStateInstance.GetComponent<SetUpState>().readCommandFile();            
        }
    }

}
