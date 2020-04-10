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
    private List<GameObject> generatedPuffEffects = new List<GameObject>();


    private void Start() {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        if (SaveLoadManager.Instance.CheckSavedGame()) { // load previous game
            ResumeLastSave();
        } else { // new game
            InitiatePlayer();
        }
    }

    public void InitiatePlayer() {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        ClearPuffEffects();

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
        ClearPuffEffects();

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

    private void ClearPuffEffects() {
        for (int i=0; i<generatedPuffEffects.Count; i++) {
            Destroy(generatedPuffEffects[i]);
        }
        generatedPuffEffects.Clear();
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
        GameObject puffEffectGameObject = Instantiate(Resources.Load("CrashPuff_Particle System") as GameObject, transform.position, Quaternion.identity);
        generatedPuffEffects.Add(puffEffectGameObject);
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

    public int GetScore() {
        return score;
    }
    public void SetScore(int aScore) {
        score = aScore;
        GameObject.FindGameObjectWithTag("UIInterface").GetComponent<UIManager>().UpdateScoreText(score.ToString());
    }
}
