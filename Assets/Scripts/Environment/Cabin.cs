using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabin : MonoBehaviour
{

    public List<GameObject> _actors = new List<GameObject>();
    public GameObject cabinSprite;

    private void OnTriggerEnter2D(Collider2D collision) {

        cabinSprite.SetActive(false);
        _actors.Add(collision.gameObject);
        Debug.Log(collision.gameObject.name + " entered " + gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision) {

        cabinSprite.SetActive(true);
        _actors.Remove(collision.gameObject);
        Debug.Log(collision.gameObject.name + " left " + gameObject.name);
    }


    // Update is called once per frame
    void Awake()
    {
        cabinSprite.SetActive(true);
    }
}
