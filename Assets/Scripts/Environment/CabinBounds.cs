using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinBounds : MonoBehaviour
{
    Cabin cabin;
    public int cabinId;

    private void Start() {
        cabin = GetComponentInParent<Cabin>();
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        cabin.CabinEnter(collision.gameObject);


        if (collision.GetComponent<PlayerController>()) {
            CabinManager.Instance.CabinEnter(cabinId);
        }

        //Debug.Log(collision.gameObject.name + " entered " + gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.GetComponent<PlayerController>()) {
            CabinManager.Instance.CabinExit(cabinId);
        }

        //Debug.Log(collision.gameObject.name + " left " + gameObject.name);
    }

}
