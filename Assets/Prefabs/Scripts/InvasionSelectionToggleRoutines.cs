﻿using UnityEngine;
using UnityEngine.UI;

public class InvasionSelectionToggleRoutines : MonoBehaviour
{
    public int index;

    public void invadedAreaSelected()
    {
        if (GetComponent<Toggle>().isOn)
        {
            GlobalDefinitions.writeToLogFile("invadedAreaSelected: exeucuting");
            if (GlobalDefinitions.localControl && (GlobalDefinitions.gameMode == GlobalDefinitions.GameModeValues.Network))
                TransportScript.SendSocketMessage(GlobalDefinitions.INVASIONAREASELECTIONKEYWORD + " " + name);
            GameControl.invasionRoutinesInstance.GetComponent<InvasionRoutines>().setInvasionArea(index);

            GameControl.gameStateControlInstance.GetComponent<gameStateControl>().currentState.executeMethod =
                    GameControl.gameStateControlInstance.GetComponent<gameStateControl>().currentState.GetComponent<AlliedInvasionState>().executeSelectUnit;
            GlobalDefinitions.writeToLogFile("invadedAreaSelected: execute method set to executeSelectUnit()");

            GlobalDefinitions.writeToLogFile("invadedAreaSelected: removing gui with transform parent = " + transform.parent.name + "   game object name = " + transform.parent.gameObject.name);
            GlobalDefinitions.removeGUI(transform.parent.gameObject);
            GlobalDefinitions.nextPhaseButton.GetComponent<Button>().interactable = true;
            GlobalDefinitions.undoButton.GetComponent<Button>().interactable = true;
            GlobalDefinitions.MustAttackToggle.GetComponent<Toggle>().interactable = true;
            GlobalDefinitions.AssignCombatButton.GetComponent<Button>().interactable = true;
            GlobalDefinitions.DisplayAllCombatsButton.GetComponent<Button>().interactable = true;
            GlobalDefinitions.AlliedSupplyRangeToggle.GetComponent<Toggle>().interactable = true;
            GlobalDefinitions.GermanSupplyRangeToggle.GetComponent<Toggle>().interactable = true;
            GlobalDefinitions.AlliedSupplySourcesButton.GetComponent<Button>().interactable = true;
        }
    }
}
