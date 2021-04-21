using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using Yarn.Unity;
using Yarn;
using TMPro;
using UnityEngine.Events;

/// Displays dialogue lines to the player, and sends
/// user choices back to the dialogue system.

/** Note that this is just one way of presenting the
* dialogue to the user. The only hard requirement
* is that you provide the RunLine, RunOptions, RunCommand
* and DialogueComplete coroutines; what they do is up to you.
*/
public enum Emotion { happy, sad, suprised, angry };

[System.Serializable] public class EmotionEvent : UnityEvent<Emotion> { }

[System.Serializable] public class ActionEvent : UnityEvent<string> { }

[System.Serializable] public class TextRevealEvent : UnityEvent<char> { }

[System.Serializable] public class DialogueEvent : UnityEvent { }

public class DialogueUILog : DialogueUIBehaviour {

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
    //public Text lineText;

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

    public Transform textContainer;
    public TMP_FontAsset textMeshFont;
    public List<GameObject> convoLines = new List<GameObject>();

    [SerializeField] private float speed = 10;
    [SerializeField] public float fontSize = 12;
    public EmotionEvent onEmotionChange;
    public ActionEvent onAction;
    public TextRevealEvent onTextReveal;
    public DialogueEvent onDialogueFinish;

    int lineNum = 0;

    void Awake() {
        // Start by hiding the container, line and option buttons
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);

        //lineText.gameObject.SetActive(false);

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

            foreach (char c in g.name) {
                stringBuilder.Append(c);
            }

            string charName = stringBuilder.ToString().Replace(remove, "");
            Debug.Log(charName);
            characters.Add(charName, g); ;
        }

    }

    // TODO Rewrite Parsing of string so that we can get in some commands
    public string ParseText(string line) {


        string newLine = string.Empty;

        //Old Parser
        //string command = "";

        // split the whole text into parts based off the <> tags 
        // even numbers in the array are text, odd numbers are tags
        string[] subTexts = line.Split('<', '>');

        // textmeshpro still needs to parse its built-in tags, so we only include noncustom tags
        string displayText = "";
        //Run through each subtext
        for (int i = 0; i < subTexts.Length; i++) {
            // If the number is even then we add it to the display text as text
            if (i % 2 == 0) {
                displayText += subTexts[i];
            }
            //If it is not a custom tag either then we add it in as a regular TMP tag
            else if (!isCustomTag(subTexts[i].Replace(" ", ""))) {
                displayText += $"<{subTexts[i]}>";
            }
        }

        //Check if the string we are using is a custom tag
        bool isCustomTag(string tag) {

            return tag.StartsWith("speed=") || tag.StartsWith("pause=") || tag.StartsWith("emotion=") || tag.StartsWith("action=");
        }

        //Create our Text Game object
        GameObject text = CreateText();
        convoLines.Add(text);
        //Get the Text Mesh Pro component
        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();
        tmp.font = textMeshFont;

        // send that string to textmeshpro and hide all of it, then start reading
        tmp.text = displayText;
        tmp.maxVisibleCharacters = 0;
        StartCoroutine(Read());

        IEnumerator Read() {
            int subCounter = 0;
            int visibileCounter = 0;

            //while the amount of sub texts we've read is less than the total
            while (subCounter < subTexts.Length) {

                //if the sub counter is odd then we evaluate the tag
                if (subCounter % 2 == 1) {
                    yield return EvaluateTag(subTexts[subCounter].Replace(" ", ""));
                }
                //If it is not then we read it out and use any of the tags weve got
                else {
                    while (visibileCounter < subTexts[subCounter].Length) {
                        onTextReveal.Invoke(subTexts[subCounter][visibileCounter]);
                        visibileCounter++;
                        tmp.maxVisibleCharacters++;
                        yield return new WaitForSeconds(1f / speed);
                    }

                    visibileCounter = 0;
                }
                subCounter++;
            }

            yield return null;

            //TODO move to other classes?
            // Evaluate each of our tags
            WaitForSeconds EvaluateTag(string tag) {
                if (tag.Length > 0) {
                    if (tag.StartsWith("speed=")) {
                        speed = float.Parse(tag.Split('=')[1]);
                    }
                    else if (tag.StartsWith("pause=")) {
                        return new WaitForSeconds(float.Parse(tag.Split('=')[1]));
                    }

                    else if (tag.StartsWith("emotion=")) {
                        onEmotionChange.Invoke((Emotion)System.Enum.Parse(typeof(Emotion), tag.Split('=')[1]));
                    }
                    else if (tag.StartsWith("action=")) {
                        onAction.Invoke(tag.Split('=')[1]);
                    }
                }

                return null;
            }

            onDialogueFinish.Invoke();
        }
        return newLine;


    }

    //Check if the name before the ':' is a character and move the dialogue box to them
    public void CheckName(string name) {
        foreach (KeyValuePair<string, GameObject> charName in characters)

            if (name.Equals(charName.Key)) {
                dialogueContainer.transform.position = new Vector2(charName.Value.transform.position.x, (charName.Value.transform.position.y + containerHeight));
            }
    }


    public GameObject CreateText() {
        GameObject newObject = new GameObject();


        newObject.transform.SetParent(textContainer);
        newObject.transform.SetAsLastSibling();

        newObject.name = "Line Number: " + lineNum;

        TextMeshProUGUI text = newObject.AddComponent<TextMeshProUGUI>();
        text.font = Resources.Load("UI/Fonts/Timeless SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
        text.fontSize = fontSize;


        LayoutElement layout = newObject.AddComponent<LayoutElement>();
        layout.minHeight = 20;
        layout.minWidth = 200;


        lineNum++;
        return newObject;


    }

    /// Show a line of dialogue, gradually
    public override IEnumerator RunLine(Line line) {
        // Show the text
        //lineText.gameObject.SetActive(true);
        speed = 20;
        Debug.Log(line.text);
        ParseText(line.text);

        // Show the 'press any key' prompt when done, if we have one
        if (continuePrompt != null)
            continuePrompt.SetActive(true);

        // Wait for any user input
        while (Input.anyKeyDown == false) {
            speed = 1000;
            yield return null;
        }

        // Hide the text and prompt
        //lineText.gameObject.SetActive(false);

        if (continuePrompt != null)
            continuePrompt.SetActive(false);

    }

    /// Show a list of options, and wait for the player to make a selection.
    public override IEnumerator RunOptions(Options optionsCollection,
                                            OptionChooser optionChooser) {


        // Do a little bit of safety checking
        if (optionsCollection.options.Count > optionButtons.Count) {
            Debug.LogWarning("There are more options to present than there are" +
                                "buttons to present them in. This will cause problems.");
        }

        // Display each option in a button, and make it visible
        int i = 0;
        foreach (var optionString in optionsCollection.options) {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = optionString;
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
        Player.GetComponent<Animator>().SetFloat("Speed", 0);
        //Player.GetComponent<PlayerController>().Move(0);
        // Enable the dialogue controls.
        if (dialogueContainer != null)
            dialogueContainer.SetActive(true);

        // Hide the game controls.
        if (gameControlsContainer != null) {
            gameControlsContainer.gameObject.SetActive(false);

        }

        Player.GetComponent<PlayerController>().enabled = false;
        //Player.transform.position = DialoguePosition;

        yield break;
    }

    /// Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete() {
        Debug.Log("Complete!");

        foreach(GameObject line in convoLines) {
            Destroy(line);
        }

        convoLines.Clear();

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


