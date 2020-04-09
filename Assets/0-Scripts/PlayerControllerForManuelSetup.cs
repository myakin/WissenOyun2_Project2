using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerForManuelSetup : MonoBehaviour {
    public float forceMagnitude = 100f;
    public float rightMovementMagnitude = 5f;
    public bool isDead;
    // public Sprite deadSprite;
    // public Sprite liveSprite;
    // public SpriteRenderer playerSpriteRenderer;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start() {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        InitiatePlayer();
    }

    public void InitiatePlayer() {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        GetComponent<Animator>().SetBool("isDead", false);
        // playerSpriteRenderer.sprite = liveSprite;
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * rightMovementMagnitude, ForceMode2D.Impulse);
        isDead = false;
    }
    
    private void Update() {
        if (!isDead && Input.GetMouseButtonDown(0)) {
            ApplyAntiGravityForce();
        }

        //Touch tou = Input.GetTouch(0);
        //if (tou.phase == TouchPhase.Began) {
        //    ApplyAntiGravityForce();
        //}  
    }

    private void ApplyAntiGravityForce() {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * forceMagnitude, ForceMode2D.Impulse);
    }

    

    private void OnCollisionEnter2D(Collision2D other) {
        Die();
    }

    private void Die() {
        GetComponent<Animator>().SetBool("isDead", true);
        // playerSpriteRenderer.sprite = deadSprite;
        isDead = true;
        GameObject.FindGameObjectWithTag("UIInterface").GetComponent<UIManager>().ActivteMainMenu();
    }
}
