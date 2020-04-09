using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerForManuelSetup : MonoBehaviour {
    public float forceMagnitude = 100f;
    public float rightMovementMagnitude = 5f;
    public bool isDead;
    public float displacementScoreCoefficient = 1f;
    // public Sprite deadSprite;
    // public Sprite liveSprite;
    // public SpriteRenderer playerSpriteRenderer;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private int score;
    private float lastPositionX;
    private IEnumerator displacementScoreCoroutine;


    private void Start() {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        InitiatePlayer();
    }

    public void InitiatePlayer() {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        lastPositionX = initialPosition.x;
        GetComponent<Animator>().SetBool("isDead", false);
        // playerSpriteRenderer.sprite = liveSprite;
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * rightMovementMagnitude, ForceMode2D.Impulse);
        isDead = false;
        score = 0;
        if (displacementScoreCoroutine==null) {
            displacementScoreCoroutine = SetDisplacementScore();
            StartCoroutine(displacementScoreCoroutine);
        }
        
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

    public void AddToScore(int anIncrement) {
        score+=anIncrement;
        GameObject.FindGameObjectWithTag("UIInterface").GetComponent<UIManager>().UpdateScoreText(score.ToString());
    }

    private int CalculateDisplacementScore() {
        int increment = (int)((transform.position.x - lastPositionX) * displacementScoreCoefficient);
        lastPositionX = transform.position.x;
        return increment;
    }

    private IEnumerator SetDisplacementScore() {
        yield return new WaitForSeconds(10f);
        if (isDead) {
            StopCoroutine(displacementScoreCoroutine);
            displacementScoreCoroutine = null;
        } else {
            int increment = CalculateDisplacementScore();
            AddToScore(increment);
            yield return SetDisplacementScore();
        }
    }
}
