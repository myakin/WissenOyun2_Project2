using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBirdController : MonoBehaviour {
    public float movementRangePercent = 0.5f;
    public float moveSpeed = 1;
    public bool hasBullet;
    public float bulletFrequency = 1;
    public float maxYFromGround = 7.5f;
    public bool isDead;
    public Sprite deadSprite;

    private Vector2 maxPosY, minPosY, midPoint;
    private float movementDirection = 1; // 1->up, -1->down

    private IEnumerator fireBulletCoroutine;
    private bool listenForZeroVelocity;


    private void Start() {
        DefineLimits();
        if (hasBullet) {
            StarFiring();
        }
    }

    // 1<<10
    // LayerMask.GetMask("Environment")
    private void DefineLimits() {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, -Vector2.up, 100, 1<<10);
        // Debug.Log(hit2D.collider.name);
        if (hit2D) {
            Vector2 minPosYTemp = hit2D.point;
            Vector2 maxPosYTemp = minPosYTemp + Vector2.up * maxYFromGround;

            midPoint = minPosYTemp + Vector2.up * (7.5f/2f);

            minPosY = Vector2.Lerp(midPoint, minPosYTemp, movementRangePercent);
            maxPosY = Vector2.Lerp(midPoint, maxPosYTemp, movementRangePercent);

        }
    }

    
    private void Update() {
        if (!isDead) {
            transform.position += transform.up * moveSpeed * movementDirection;
            if (transform.position.y>maxPosY.y || transform.position.y<minPosY.y) {
                movementDirection *= -1;
            }
        }
        if (listenForZeroVelocity) {
            // Debug.Log(GetComponent<Rigidbody2D>().velocity.sqrMagnitude);
            if (GetComponent<Rigidbody2D>().velocity.sqrMagnitude==0) {
                listenForZeroVelocity = false;
                ObjectSpawnHandler.Instance.DespawnObject(gameObject);
            }
        }
    }

    public void StarFiring() {
        if (fireBulletCoroutine==null) {
            fireBulletCoroutine = BulletCreator();
        }
        StartCoroutine(fireBulletCoroutine);
    }

    public void StopFiring() {
        if (fireBulletCoroutine==null) {
            StopCoroutine(fireBulletCoroutine);
        }
    }

    private IEnumerator BulletCreator() {
        yield return new WaitForSeconds(1 / bulletFrequency);
        ObjectSpawnHandler.Instance.SpawnObject("EnemyBullet", transform.position + (-transform.right * 0.81f) + (-transform.up * 0.04f), Quaternion.identity);
        yield return BulletCreator();
    }

    public void Die() {
        if (!isDead) {
            isDead = true;

            // sprite degistirelim
            GetComponent<SpriteRenderer>().sprite = deadSprite;

            //1. SECENEK
            // patlama efekti
            ObjectSpawnHandler.Instance.SpawnObject("ExplositionSpriteAnimation", transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            // rigidbody ekleyelim
            gameObject.AddComponent<Rigidbody2D>();
            // velocity 0 oldugunda yok ol (despawn)
            

            //2. SECENEK
            // patlama efekti olusturalim
            // kusu da yok edelim
        }
        StartCoroutine(StartZeroVelocityListener());

    }

    private IEnumerator StartZeroVelocityListener() {
        yield return new WaitForSeconds(1);
        listenForZeroVelocity = true;
    }

    
    private void OnCollisionEnter2D(Collision2D other) {
        ObjectSpawnHandler.Instance.SpawnObject("CrashPuff_Particle System", transform.position, Quaternion.identity);
    }

}
