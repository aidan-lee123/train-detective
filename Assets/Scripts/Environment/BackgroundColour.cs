using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BackgroundColour : MonoBehaviour
{
    public Color spriteColour;

    [Range(0.0f, 1.1f)]
    public float Hue;

    [Range(0.0f, 1.0f)]
    public float Saturation;

    [Range(0.0f, 1.0f)]
    public float Value = 1f;

    private Transform[] children;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        spriteColour = new Color(spriteColour.r, spriteColour.g, spriteColour.b);
        spriteColour = Color.HSVToRGB( Hue, Saturation, Value);


        foreach (Transform t in transform) {
            children = t.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform c in children) {
                c.GetComponent<SpriteRenderer>().color = spriteColour;
            }
        }
    }




}
