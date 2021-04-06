using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInventoryTitle : MonoBehaviour
{
    public string Text { get; set; }
    private TMP_Text tmp;

    // Start is called before the first frame update
    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
        HideText(true);
        Text = "";
    }

    public void UpdateText(string text) {
        Text = text;
        tmp.text = text;
    }


    public void HideText(bool state) {
        RectTransform rect = GetComponent<RectTransform>();

        if (state) {
            Text = "";
            rect.localScale = new Vector3(0, 0, 0);
        }
        else {
            rect.localScale = new Vector3(1, 1, 1);
        }
    }
}
