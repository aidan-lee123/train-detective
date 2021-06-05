using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp;
    string input;

    public static DebugCommand KILL_ALL;
    public static DebugCommand HELP;
    public static DebugCommand<float> SET_MONEY;
    public static DebugCommand<float> ADD_MONEY;

    public List<object> commandList;

    public void OnToggleDebug(InputAction.CallbackContext value) {
        showConsole = !showConsole;
    }

    public void OnReturn(InputAction.CallbackContext value) {
        if (showConsole) {
            HandleInput();
            input = "";
        }
    }

    private void Awake() {
        KILL_ALL = new DebugCommand("kill_all", "Removes all NPCs from the scene.", "kill_all", () => {
            Debug.Log("Killing all NPCs");
            NPCManager.Instance.KillAllNPCs();
        });

        SET_MONEY = new DebugCommand<float>("set_money", "Sets the amount of money", "set_money <money_amount>", (x) => {
            Player.Instance.SetMoney(x);
        });

        ADD_MONEY = new DebugCommand<float>("add_money", "Adds an amout of money to the player", "add_money <amount>", (x) => {
            Player.Instance.AddMoney(x);
        });

        HELP = new DebugCommand("help", "Shows a list of Commands", "help", () => {
            showHelp = true;
        });

        commandList = new List<object> {
            KILL_ALL,
            SET_MONEY,
            ADD_MONEY,
            HELP,
        };
    }

    Vector2 scroll;

    private void OnGUI() {
        if (!showConsole) return;

        float y = 0f;

        if (showHelp) {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            for(int i=0; i<commandList.Count; i++) {
                DebugCommandBase command = commandList[i] as DebugCommandBase;

                string label = $"{command.commandFormat} - {command.commanDescription}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();

            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

    }

    //Handles the Commands
    private void HandleInput() {

        string[] properties = input.Split(' ');

        //Iterate over each command in the list and check if the command exists
        for(int i=0; i<commandList.Count; i++) {
            //Cast it to the base so that we can check multiple types of commands
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            //If it does we invoke it
            if (input.Contains(commandBase.commandId)) {

                if(commandList[i] as DebugCommand != null) {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<int> != null) {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
                else if (commandList[i] as DebugCommand<float> != null) {
                    (commandList[i] as DebugCommand<float>).Invoke(float.Parse(properties[1]));
                }
            }
        }
    }

}
