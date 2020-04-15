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
        if (SaveLoadManager.Instance.CheckSavedGame()) { // load previous game
            ResumeLastSave();
        } else { // new game
            StartNewGame();
        }
    }

    public void StartNewGame() {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        ObjectSpawnHandler.Instance.ClearEffects();
        ObjectSpawnHandler.Instance.ReSpawnCoins();
        SoundManager.Instance.ResetSounds();

        transform.position = initialPosition;
        transform.rotation = initialRotation;
        lastPositionX = initialPosition.x;
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * rightMovementMagnitude, ForceMode2D.Impulse);
        score = 0;
        
        GetComponent<Animator>().SetBool("isDead", false);
        // playerSpriteRenderer.sprite = liveSprite;
        
        isDead = false;

        if (displacementScoreCoroutine==null) {
            displacementScoreCoroutine = SetDisplacementScore();
            StartCoroutine(displacementScoreCoroutine);
        }

        
    }

    public void ResumeLastSave() {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        ObjectSpawnHandler.Instance.ClearEffects();
        SoundManager.Instance.ResetSounds();

        if (SaveLoadManager.Instance.LoadGame()) {
            GetComponent<Animator>().SetBool("isDead", false);
            // playerSpriteRenderer.sprite = liveSprite;
            
            isDead = false;

            if (displacementScoreCoroutine==null) {
                displacementScoreCoroutine = SetDisplacementScore();
                StartCoroutine(displacementScoreCoroutine);
            }
        } else {
            Debug.Log("Error: save file counld not be found");
        }
    }

    


    
    private void Update() {
        if (!isDead && Input.GetMouseButtonDown(0)) {
            ApplyAntiGravityForce();
        }
        if (!isDead && Input.GetMouseButtonDown(1)) {
            Fire();
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
        ObjectSpawnHandler.Instance.SpawnObject("CrashPuff_Particle System", transform.position, Quaternion.identity);
        isDead = true;
        GameObject.FindGameObjectWithTag("UIInterface").GetComponent<UIManager>().ActivteMainMenu();
        SoundManager.Instance.PlaySoundsOnFail();
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

    public int GetScore() {
        return score;
    }
    public void SetScore(int aScore) {
        score = aScore;
        GameObject.FindGameObjectWithTag("UIInterface").GetComponent<UIManager>().UpdateScoreText(score.ToString());
    }
    private void Fire() {
        ObjectSpawnHandler.Instance.SpawnObject("PlayerBullet", transform.position + (transform.right * 1f) + (-transform.up * 0.04f), Quaternion.identity);
    }
}
