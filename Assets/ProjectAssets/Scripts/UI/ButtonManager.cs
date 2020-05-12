using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {
    // TODO: More elegant way to do this? A giant switch statement is not ideal
    // Maybe at least separate the buttons by type?
    public void DoActionForButton(string buttonName) {
        switch (buttonName) {
            case "Smash":
                Debug.Log("Use Smash ability.");
                return;
            case "Inventory":
                Debug.Log("Inventory Screen to appear");
                return;
            default:
                Debug.Log("ActionBasedOnButton called with invalid buttonName. Check the string on the button.");
                break;
        }
    }
}
