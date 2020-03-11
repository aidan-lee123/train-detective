using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public GameObject arrow;
    public GameObject link;
    public Collider2D bounds;

    private void OnTriggerStay2D(Collider2D collision) {
        if (Input.GetKeyDown(KeyCode.E)) {

            Debug.Log("E Pressed");
            MoveCharacter(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DisplayArrow();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        HideArrow();
    }

    public void DisplayArrow() {
        arrow.SetActive(true);
    }

    public void HideArrow() {
        arrow.SetActive(false);
    }

    public void MoveCharacter(GameObject character) {
        character.transform.position = new Vector2(link.transform.position.x, character.transform.position.y);
        //CameraController.Instance.DisableConfines();

        //StartCoroutine(DelayCinemachine(0.08f));

    }

    IEnumerator DelayCinemachine(float seconds) {
        yield return new WaitForSeconds(seconds);
        CameraController.Instance.EnableConfines(bounds);
    }
}
