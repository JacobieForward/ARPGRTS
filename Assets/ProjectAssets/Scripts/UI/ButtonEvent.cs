using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/**
 * For now buttons will be handled based on which string they are set to. This may not be great code wise but should make it easy to change the functionality of buttons.
 */
[RequireComponent(typeof(Button))]
public class ButtonEvent : MonoBehaviour {
    public enum ButtonType {
        Ability,
        Menu
    }
    public ButtonType buttonType = ButtonType.Ability;

    ButtonManager buttonManager;
    Button button;
    string buttonAction;
    Combat playerCombat;

    void Awake() {
        button = GetComponent<Button>();
        buttonAction = GetComponentInChildren<Text>().text; // Cannot have more than one text component in childed objects, but that shouldn't be a problem
                                                            // Could always just move that to this button, or simply set the string in the script in the editor
        SetButtonManagerAndCheckForIssues();
        button.onClick.AddListener(CallActionForButton);

        playerCombat = GameObject.Find("Player").GetComponent<Combat>();
    }

    void SetButtonManagerAndCheckForIssues() {
        ButtonManager[] allButtonManagers = GameObject.FindObjectsOfType<ButtonManager>();
        if (allButtonManagers.Length > 1) {
            Debug.Log("Too many button managers in scene.");
        } else if (allButtonManagers.Length < 1) {
            Debug.Log("No button manager in scene.");
        } else {
            buttonManager = allButtonManagers[0];
        }
    }

    void CallActionForButton() {
        // If ability button type make current ability for player
        // Do this by enumerating through all abilities for the player and match the string based on the ability name and button name
        if (buttonType == ButtonType.Ability) {
            playerCombat.ActivateAbility(buttonAction);
        }

        // If ability is a menu type do something else, maybe just have a switch here for all menu types
        if (buttonType == ButtonType.Menu) {
            buttonManager.DoActionForButton(buttonAction);
        }
    }
}
