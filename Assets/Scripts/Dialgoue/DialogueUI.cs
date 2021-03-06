﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using Yarn.Unity;

namespace Yarn.Unity.Example {
    /// Displays dialogue lines to the player, and sends
    /// user choices back to the dialogue system.

    /** Note that this is just one way of presenting the
     * dialogue to the user. The only hard requirement
     * is that you provide the RunLine, RunOptions, RunCommand
     * and DialogueComplete coroutines; what they do is up to you.
     */
    public class DialogueUI : DialogueUIBehaviour {

        //NPC Container Gameobject
        public GameObject npcContainer;

        /// The object that contains the dialogue and the options.
        /** This object will be enabled when conversation starts, and 
         * disabled when it ends.
         */
        public GameObject dialogueContainer;

        //
        public Dictionary<string, GameObject> characters = new Dictionary<string, GameObject>();

        /// The UI element that displays lines
        public Text lineText;

        /// A UI element that appears after lines have finished appearing
        public GameObject continuePrompt;

        /// A delegate (ie a function-stored-in-a-variable) that
        /// we call to tell the dialogue system about what option
        /// the user selected
        private Yarn.OptionChooser SetSelectedOption;

        /// How quickly to show the text, in seconds per character
        [Tooltip("How quickly to show the text, in seconds per character")]
        public float textSpeed = 0.025f;

        /// The buttons that let the user choose an option
        public List<Button> optionButtons;
        public GameObject optionsIcon;

        /// Make it possible to temporarily disable the controls when
        /// dialogue is active and to restore them when dialogue ends
        public RectTransform gameControlsContainer;

        public float containerHeight;

        public GameObject Player;

        void Awake() {
            // Start by hiding the container, line and option buttons
            if (dialogueContainer != null)
                dialogueContainer.SetActive(false);

            lineText.gameObject.SetActive(false);

            foreach (var button in optionButtons) {
                button.gameObject.SetActive(false);
            }

            // Hide the continue prompt if it exists
            if (continuePrompt != null)
                continuePrompt.SetActive(false);

            FindNPCs(npcContainer);

            //foreach(GameObject )
        }

        //Find all Gameobjects with tag "NPC" and add it to the dictionary
        public void FindNPCs(GameObject root) {
            string remove = "NPC_";
            GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
            foreach (GameObject g in npcs) {

                var stringBuilder = new StringBuilder();

                    foreach(char c in g.name) {
                        stringBuilder.Append(c);
                    }

                    string charName = stringBuilder.ToString().Replace(remove, "");
                    Debug.Log(charName);
                    characters.Add(charName, g); ;
                }
            
        }


        // TODO Rewrite Parsing of string so that we can get in some commands
        public string ParseText(string line) {
            string newLine = "";
            string command = "";
            newLine = line;

            //Set the lineText Color to white at the beginning of each line
            lineText.color = Color.white;

            //If the line contains a tag then find out what it is
            if (newLine.Contains("[") && newLine.Contains("]")) {
                int endTag = newLine.IndexOf("]");
                int beginTag = newLine.IndexOf("[");

                //put the command between the two tag chars into a string
                command = newLine.Substring(beginTag + 1, endTag - beginTag - 1);

                //Switch depending on command
                switch (command) {
                    case "Blue":
                        //Remove the command from the line
                        newLine = newLine.Remove(beginTag, endTag - beginTag + 1);
                        lineText.color = Color.blue;
                        break;

                    case "Yellow":
                        newLine = newLine.Remove(beginTag, endTag - beginTag + 1);
                        lineText.color = Color.yellow;
                        break;
                }
                

            }

            //TODO
            foreach (char c in newLine) { 
                if (c.Equals(':')) {
                  //  Debug.Log(stringBuilder.ToString() + " is speaking");
                   // CheckName(stringBuilder.ToString());
                }
            }

            //return the new altered string
            return newLine;
        }   
        
        //Check if the name before the ':' is a character and move the dialogue box to them
        public void CheckName(string name) {
            foreach(KeyValuePair<string, GameObject> charName in characters)

            if (name.Equals(charName.Key)) {
                    dialogueContainer.transform.position = new Vector2(charName.Value.transform.position.x, (charName.Value.transform.position.y + containerHeight));
            }
        }

        /// Show a line of dialogue, gradually
        public override IEnumerator RunLine(Line line) {
            // Show the text
            lineText.gameObject.SetActive(true);

            if (textSpeed > 0.0f) {
                string newLine = ParseText(line.text);

                // Display the line one character at a time
                var stringBuilder = new StringBuilder();



                //Check what the 
                foreach (char c in newLine) {
                    //If the Character equals ':' send the name before that char to the CheckName() function
                    if (c.Equals(':')){
                        Debug.Log(stringBuilder.ToString() + " is speaking");
                        CheckName(stringBuilder.ToString());
                    }


                    stringBuilder.Append(c);
                    lineText.text = stringBuilder.ToString();

                    yield return new WaitForSeconds(textSpeed);
                }


            }
            else {
                // Display the line immediately if textSpeed == 0
                lineText.text = line.text;
            }

            // Show the 'press any key' prompt when done, if we have one
            if (continuePrompt != null)
                continuePrompt.SetActive(true);

            // Wait for any user input
            while (Input.anyKeyDown == false) {
                yield return null;
            }

            // Hide the text and prompt
            lineText.gameObject.SetActive(false);

            if (continuePrompt != null)
                continuePrompt.SetActive(false);

        }

        /// Show a list of options, and wait for the player to make a selection.
        public override IEnumerator RunOptions(Options optionsCollection,
                                                OptionChooser optionChooser) {

            dialogueContainer.GetComponent<Image>().enabled = false;
            // Do a little bit of safety checking
            if (optionsCollection.options.Count > optionButtons.Count) {
                Debug.LogWarning("There are more options to present than there are" +
                                 "buttons to present them in. This will cause problems.");
            }

            // Display each option in a button, and make it visible
            int i = 0;
            foreach (var optionString in optionsCollection.options) {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<Text>().text = optionString;
                i++;
            }

            // Record that we're using it
            SetSelectedOption = optionChooser;
            

            // Wait until the chooser has been used and then removed (see SetOption below)
            while (SetSelectedOption != null) {
                yield return null;
            }

            // Hide all the buttons
            foreach (var button in optionButtons) {
                button.gameObject.SetActive(false);
            }
        }

        /// Called by buttons to make a selection.
        public void SetOption(int selectedOption) {

            // Call the delegate to tell the dialogue system that we've
            // selected an option.
            SetSelectedOption(selectedOption);


            // Now remove the delegate so that the loop in RunOptions will exit
            SetSelectedOption = null;

            dialogueContainer.GetComponent<Image>().enabled = true;
        }

        /// Run an internal command.
        public override IEnumerator RunCommand(Command command) {
            // "Perform" the command
            Debug.Log("Command: " + command.text);

            yield break;
        }

        /// Called when the dialogue system has started running.
        public override IEnumerator DialogueStarted() {
            Debug.Log("Dialogue starting!");

            // Enable the dialogue controls.
            if (dialogueContainer != null)
                dialogueContainer.SetActive(true);

            // Hide the game controls.
            if (gameControlsContainer != null) {
                gameControlsContainer.gameObject.SetActive(false);
                
            }

            Player.GetComponent<PlayerController>().enabled = false;

            yield break;
        }

        /// Called when the dialogue system has finished running.
        public override IEnumerator DialogueComplete() {
            Debug.Log("Complete!");

            // Hide the dialogue interface.
            if (dialogueContainer != null)
                dialogueContainer.SetActive(false);

            // Show the game controls.
            if (gameControlsContainer != null) {
                gameControlsContainer.gameObject.SetActive(true);
            }

            Player.GetComponent<PlayerController>().enabled = true;

            yield break;
        }

    }

}
