using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int ID;
    public string objectName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Glow(bool state) {
        switch (state) {
            case true:
                print("Turn on Glow");
                break;
            case false:
                print("Turn off Glow");
                break;
        }
    }
}
