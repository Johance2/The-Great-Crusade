﻿using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ExecuteGameCommand : MonoBehaviour {


    /// <summary>
    /// This routine is what processes the message received from the opponent computer
    /// </summary>
    /// <param name="message"></param>
    public static void processCommand(string message)
    {
        char[] delimiterChars = { ' ' };
        string[] switchEntries = message.Split(delimiterChars);

        string[] lineEntries = message.Split(delimiterChars);
        // I am going to use the same routine to read records that is used when reading from a file.
        // In order to do this I need to drop the first word on the line since the files don't have key words
        for (int index = 0; index < (lineEntries.Length - 1); index++)
            lineEntries[index] = lineEntries[index + 1];

        switch (switchEntries[0])
        {
            case GlobalDefinitions.PLAYSIDEKEYWORD:
                if (switchEntries[1] == "German")
                    GlobalDefinitions.sideControled = GlobalDefinitions.Nationality.German;
                else
                    GlobalDefinitions.sideControled = GlobalDefinitions.Nationality.Allied;
                break;
            case GlobalDefinitions.PASSCONTROLKEYWORK:
                GlobalDefinitions.localControl = true;
                GlobalDefinitions.writeToLogFile("processNetworkMessage: Message received to set local control");
                break;
            case GlobalDefinitions.SETCAMERAPOSITIONKEYWORD:
                Camera.main.transform.position = new Vector3(float.Parse(switchEntries[1]), float.Parse(switchEntries[2]), float.Parse(switchEntries[3]));
                Camera.main.GetComponent<Camera>().orthographicSize = float.Parse(switchEntries[4]);
                break;
            case GlobalDefinitions.MOUSESELECTIONKEYWORD:
                if (switchEntries[1] != "null")
                    GameControl.inputMessage.GetComponent<InputMessage>().hex = GameObject.Find(switchEntries[1]);
                else
                    GameControl.inputMessage.GetComponent<InputMessage>().hex = null;

                if (switchEntries[2] != "null")
                    GameControl.inputMessage.GetComponent<InputMessage>().unit = GameObject.Find(switchEntries[2]);
                else
                    GameControl.inputMessage.GetComponent<InputMessage>().unit = null;

                GameControl.gameStateControlInstance.GetComponent<gameStateControl>().currentState.executeMethod(GameControl.inputMessage.GetComponent<InputMessage>());
                break;
            case GlobalDefinitions.MOUSEDOUBLECLICKIONKEYWORD:
                GlobalDefinitions.Nationality passedNationality;

                if (switchEntries[2] == "German")
                    passedNationality = GlobalDefinitions.Nationality.German;
                else
                    passedNationality = GlobalDefinitions.Nationality.Allied;


                if (GlobalDefinitions.selectedUnit != null)
                    GlobalDefinitions.unhighlightUnit(GlobalDefinitions.selectedUnit);
                foreach (Transform hex in GameObject.Find("Board").transform)
                    GlobalDefinitions.unhighlightHex(hex.gameObject);
                GlobalDefinitions.selectedUnit = null;


                GameControl.movementRoutinesInstance.GetComponent<MovementRoutines>().callMultiUnitDisplay(GameObject.Find(switchEntries[1]), passedNationality);
                break;
            case GlobalDefinitions.DISPLAYCOMBATRESOLUTIONKEYWORD:
                CombatResolutionRoutines.combatResolutionDisplay();
                break;
            case GlobalDefinitions.NEXTPHASEKEYWORD:
                GameControl.gameStateControlInstance.GetComponent<gameStateControl>().currentState.executeQuit(GameControl.inputMessage.GetComponent<InputMessage>());
                break;

            case GlobalDefinitions.EXECUTETACTICALAIROKKEYWORD:
                TacticalAirToggleRoutines.tacticalAirOK();
                break;
            case GlobalDefinitions.ADDCLOSEDEFENSEKEYWORD:
                GameObject.Find("CloseDefense").GetComponent<TacticalAirToggleRoutines>().addCloseDefenseHex();
                break;
            case GlobalDefinitions.CANCELCLOSEDEFENSEKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<TacticalAirToggleRoutines>().cancelCloseDefense();
                break;
            case GlobalDefinitions.LOCATECLOSEDEFENSEKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<TacticalAirToggleRoutines>().locateCloseDefense();
                break;
            case GlobalDefinitions.ADDRIVERINTERDICTIONKEYWORD:
                GameObject.Find("RiverInterdiction").GetComponent<TacticalAirToggleRoutines>().addRiverInterdiction();
                break;
            case GlobalDefinitions.CANCELRIVERINTERDICTIONKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<TacticalAirToggleRoutines>().cancelRiverInterdiction();
                break;
            case GlobalDefinitions.LOCATERIVERINTERDICTIONKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<TacticalAirToggleRoutines>().locateRiverInterdiction();
                break;
            case GlobalDefinitions.ADDUNITINTERDICTIONKEYWORD:
                GameObject.Find("UnitInterdiction").GetComponent<TacticalAirToggleRoutines>().addInterdictedUnit();
                break;
            case GlobalDefinitions.CANCELUNITINTERDICTIONKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<TacticalAirToggleRoutines>().cancelInterdictedUnit();
                break;
            case GlobalDefinitions.LOCATEUNITINTERDICTIONKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<TacticalAirToggleRoutines>().locateInterdictedUnit();
                break;
            case GlobalDefinitions.TACAIRMULTIUNITSELECTIONKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<TacticalAirToggleRoutines>().multiUnitSelection();
                break;

            case GlobalDefinitions.MULTIUNITSELECTIONKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;
            case GlobalDefinitions.MULTIUNITSELECTIONCANCELKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<MultiUnitMovementToggleRoutines>().cancelGui();
                break;
            case GlobalDefinitions.LOADCOMBATKEYWORD:
                GameObject GUIButtonInstance = new GameObject("GUIButtonInstance");
                GUIButtonInstance.AddComponent<GUIButtonRoutines>();
                GUIButtonInstance.GetComponent<GUIButtonRoutines>().loadCombat();
                break;

            case GlobalDefinitions.SETCOMBATTOGGLEKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;
            case GlobalDefinitions.RESETCOMBATTOGGLEKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = false;
                break;
            case GlobalDefinitions.COMBATGUIOKKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<CombatGUIOK>().okCombatGUISelection();
                break;
            case GlobalDefinitions.COMBATGUICANCELKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<CombatGUIOK>().cancelCombatGUISelection();
                break;

            case GlobalDefinitions.ADDCOMBATAIRSUPPORTKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;
            case GlobalDefinitions.REMOVECOMBATAIRSUPPORTKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = false;
                break;
            case GlobalDefinitions.COMBATRESOLUTIONSELECTEDKEYWORD:
                // Load the combat results; the die roll is on the Global variable
                GameObject.Find(switchEntries[1]).GetComponent<CombatResolutionButtonRoutines>().resolutionSelected();
                break;
            case GlobalDefinitions.COMBATLOCATIONSELECTEDKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<CombatResolutionButtonRoutines>().locateAttack();
                break;
            case GlobalDefinitions.COMBATCANCELKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<CombatResolutionButtonRoutines>().cancelAttack();
                break;
            case GlobalDefinitions.COMBATOKKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<CombatResolutionButtonRoutines>().ok();
                break;
            case GlobalDefinitions.CARPETBOMBINGRESULTSSELECTEDKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;
            case GlobalDefinitions.RETREATSELECTIONKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;
            case GlobalDefinitions.POSTCOMBATMOVEMENTKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;
            case GlobalDefinitions.ADDEXCHANGEKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;
            case GlobalDefinitions.REMOVEEXCHANGEKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = false;
                break;
            case GlobalDefinitions.OKEXCHANGEKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<ExchangeOKRoutines>().exchangeOKSelected();
                break;
            case GlobalDefinitions.POSTCOMBATOKKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<PostCombatMovementOkRoutines>().executePostCombatMovement();
                break;
            case GlobalDefinitions.SETSUPPLYKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;
            case GlobalDefinitions.RESETSUPPLYKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = false;
                break;
            case GlobalDefinitions.LOCATESUPPLYKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<SupplyButtonRoutines>().locateSupplySource();
                break;
            case GlobalDefinitions.OKSUPPLYKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<SupplyButtonRoutines>().okSupplyWithEndPhase();
                break;
            case GlobalDefinitions.CHANGESUPPLYSTATUSKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;

            case GlobalDefinitions.SAVEFILENAMEKEYWORD:
                if (File.Exists(GameControl.path + "TGCOutputFiles\\TGCRemoteSaveFile.txt"))
                    File.Delete(GameControl.path + "TGCOutputFiles\\TGCRemoteSaveFile.txt");
                break;
            case GlobalDefinitions.SENDSAVEFILELINEKEYWORD:
                using (StreamWriter saveFile = File.AppendText(GameControl.path + "TGCOutputFiles\\TGCRemoteSaveFile.txt"))
                {
                    for (int index = 1; index < (switchEntries.Length); index++)
                        saveFile.Write(switchEntries[index] + " ");
                    saveFile.WriteLine();
                }
                break;
            case GlobalDefinitions.PLAYNEWGAMEKEYWORD:
                GameControl.gameStateControlInstance.GetComponent<gameStateControl>().currentState = GameControl.setUpStateInstance.GetComponent<SetUpState>();
                GameControl.gameStateControlInstance.GetComponent<gameStateControl>().currentState.initialize(GameControl.inputMessage.GetComponent<InputMessage>());

                // Set the global parameter on what file to use, can't pass it to the executeNoResponse since it is passed as a method delegate elsewhere
                GlobalDefinitions.germanSetupFileUsed = Convert.ToInt32(switchEntries[1]);

                GameControl.setUpStateInstance.GetComponent<SetUpState>().executeNewGame();
                break;

            case GlobalDefinitions.INVASIONAREASELECTIONKEYWORD:
                GlobalDefinitions.writeToLogFile("processNetworkMessage: Received INVASIONAREASELECTIONKEYWORD - turning toggle " + switchEntries[1] + " to true");
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;

            case GlobalDefinitions.CARPETBOMBINGSELECTIONKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<Toggle>().isOn = true;
                break;
            case GlobalDefinitions.CARPETBOMBINGLOCATIONKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<CarpetBombingToggleRoutines>().locateCarpetBombingHex();
                break;
            case GlobalDefinitions.CARPETBOMBINGOKKEYWORD:
                GameObject.Find(switchEntries[1]).GetComponent<CarpetBombingOKRoutines>().carpetBombingOK();
                break;

            case GlobalDefinitions.DIEROLLRESULT1KEYWORD:
                GlobalDefinitions.dieRollResult1 = Convert.ToInt32(switchEntries[1]);
                break;
            case GlobalDefinitions.DIEROLLRESULT2KEYWORD:
                GlobalDefinitions.dieRollResult2 = Convert.ToInt32(switchEntries[1]);
                break;
            case GlobalDefinitions.UNDOKEYWORD:
                GameControl.GUIButtonRoutinesInstance.GetComponent<GUIButtonRoutines>().executeUndo();
                break;
            case GlobalDefinitions.CHATMESSAGEKEYWORD:
                string chatMessage = "";
                for (int index = 0; index < (switchEntries.Length - 1); index++)
                    chatMessage += switchEntries[index + 1] + " ";
                GlobalDefinitions.writeToLogFile("Chat message received: " + chatMessage);
                GlobalDefinitions.addChatMessage(chatMessage);
                break;
            case GlobalDefinitions.SENDTURNFILENAMEWORD:
                // This command tells the remote computer what the name of the file is that will provide the saved turn file

                // The file name could have ' ' in it so need to reconstruct the full name
                string receivedFileName;
                receivedFileName = switchEntries[1];
                for (int i = 2; i < switchEntries.Length; i++)
                    receivedFileName = receivedFileName + " " + switchEntries[i];

                GlobalDefinitions.writeToLogFile("Received name of save file, calling FileTransferServer: fileName = " + receivedFileName + "  path to save = " + GameControl.path);
                GameControl.fileTransferServerInstance.GetComponent<FileTransferServer>().RequestFile(GlobalDefinitions.opponentIPAddress, receivedFileName, GameControl.path, true);
                break;

            case GlobalDefinitions.DISPLAYALLIEDSUPPLYRANGETOGGLEWORD:
                if (GameObject.Find("AlliedSupplyToggle").GetComponent<Toggle>().isOn)
                    GameObject.Find("AlliedSupplyToggle").GetComponent<Toggle>().isOn = false;
                else
                    GameObject.Find("AlliedSupplyToggle").GetComponent<Toggle>().isOn = true;
                break;

            case GlobalDefinitions.DISPLAYGERMANSUPPLYRANGETOGGLEWORD:
                if (GameObject.Find("GermanSupplyToggle").GetComponent<Toggle>().isOn)
                    GameObject.Find("GermanSupplyToggle").GetComponent<Toggle>().isOn = false;
                else
                    GameObject.Find("GermanSupplyToggle").GetComponent<Toggle>().isOn = true;
                break;

            case GlobalDefinitions.DISPLAYMUSTATTACKTOGGLEWORD:
                if (GlobalDefinitions.MustAttackToggle.GetComponent<Toggle>().isOn)
                    GlobalDefinitions.MustAttackToggle.GetComponent<Toggle>().isOn = false;
                else
                    GlobalDefinitions.MustAttackToggle.GetComponent<Toggle>().isOn = true;
                break;

            case GlobalDefinitions.TOGGLEAIRSUPPORTCOMBATTOGGLE:
                {
                    if (GlobalDefinitions.combatAirSupportToggle != null)
                    {
                        if (GlobalDefinitions.combatAirSupportToggle.GetComponent<Toggle>().isOn)
                            GlobalDefinitions.combatAirSupportToggle.GetComponent<Toggle>().isOn = false;
                        else
                            GlobalDefinitions.combatAirSupportToggle.GetComponent<Toggle>().isOn = true;
                    }
                    break;
                }

            case GlobalDefinitions.TOGGLECARPETBOMBINGCOMBATTOGGLE:
                {
                    if (GlobalDefinitions.combatCarpetBombingToggle != null)
                    {
                        if (GlobalDefinitions.combatCarpetBombingToggle.GetComponent<Toggle>().isOn)
                            GlobalDefinitions.combatCarpetBombingToggle.GetComponent<Toggle>().isOn = false;
                        else
                            GlobalDefinitions.combatCarpetBombingToggle.GetComponent<Toggle>().isOn = true;
                    }
                    break;
                }
            case GlobalDefinitions.DISCONNECTFROMREMOTECOMPUTER:
                {
                    // Quit the game and go back to the main menu
                    GameObject guiButtonInstance = new GameObject("GUIButtonInstance");
                    guiButtonInstance.AddComponent<GUIButtonRoutines>();
                    guiButtonInstance.GetComponent<GUIButtonRoutines>().yesMain();
                    break;
                }

            default:
                GlobalDefinitions.writeToLogFile("processNetworkMessage: Unknown network command received: " + message);
                break;
        }
    }
}
