using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerController : MonoBehaviour {

    public GameObject[] arrows;

    private Rigidbody2D _rigidBody;
    [SerializeField] private bool _facingRight = true;

    private Vector3 _velocity = Vector3.zero;
    private float _movementSmoothing = 0.5f;

    private float _rayLength = 1f;
    private float _rayOffset = 0.5f;
    private int _rayLayerMask;

    public float interactionRadius = 2.0f;


    private void Awake() {

        _rigidBody = GetComponent<Rigidbody2D>();
        _rayLayerMask = 8;
    }

    public void Move(float move) {

        Vector3 targetVelocity = new Vector2(move * 10f, _rigidBody.velocity.y);

        _rigidBody.velocity = Vector3.SmoothDamp(_rigidBody.velocity, targetVelocity, ref _velocity, _movementSmoothing);

        if (move > 0 && !_facingRight) {
            Flip();
        }
        else if (move < 0 && _facingRight) {
            Flip();
        }
    }

    private void Flip() {
        _facingRight = !_facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public GameObject CheckForward() {

        Vector2 direction = new Vector2(1, 0);
        if (!_facingRight) {
            direction *= -1;

        }
        float directionOffset = _rayOffset * direction.x;

        Vector2 startingPosition = new Vector2(transform.position.x + directionOffset, transform.position.y);

        RaycastHit2D hit = Physics2D.Raycast(startingPosition, direction, _rayLength, _rayLayerMask);

        if (hit.collider) {
            Debug.DrawRay(startingPosition, direction, Color.red);
            return hit.collider.gameObject;
        }

        Debug.DrawRay(startingPosition, direction, Color.white);
        return null;
    }

    public GameObject CheckBack() {
        Vector2 direction = new Vector2(-1, 0);
        if (!_facingRight) {
            direction *= -1;

        }
        float directionOffset = _rayOffset * direction.x;

        Vector2 startingPosition = new Vector2(transform.position.x + directionOffset, transform.position.y);

        RaycastHit2D hit = Physics2D.Raycast(startingPosition, direction, _rayLength, _rayLayerMask);

        if (hit.collider) {
            Debug.DrawRay(startingPosition, direction, Color.red);
            return hit.collider.gameObject;
        }

        Debug.DrawRay(startingPosition, direction, Color.white);
        return null;
    }

    public void CheckForNearbyNPC() {
        var allParticipants = new List<NPCInfo>(FindObjectsOfType<NPCInfo>());
        var target = allParticipants.Find(delegate (NPCInfo p) {
            return string.IsNullOrEmpty(p.talkToNode) == false && // has a conversation node?
            (p.transform.position - this.transform.position)// is in range?
            .magnitude <= interactionRadius;
        });
        if (target != null) {
            // Kick off the dialogue at this node.
            FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);
        }
    }

}
