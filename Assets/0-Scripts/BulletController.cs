using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public float speed = 0.1f;
    public float range = 20f;
    public int bulletDirection = -1;

    private Vector3 initialPos;
    private bool hasDespawnRequested;

    private void OnEnable() {
        initialPos = transform.position;
        hasDespawnRequested = false;
    }
    
    private void Update() {
        transform.position +=  bulletDirection * transform.right * speed;
        if ((transform.position - initialPos).sqrMagnitude > range * range) {
            if (!hasDespawnRequested) {
                hasDespawnRequested = true;
                Despawn();
            }
        }
    }

    private void Despawn() {
        ObjectSpawnHandler.Instance.DespawnObject(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // Debug.Log(other.collider.tag);
        if (other.collider.tag == "Enemy") {
            other.collider.GetComponent<EnemyBirdController>().Die();
            Despawn();
        } else if (other.collider.tag == "Environment") {
            ObjectSpawnHandler.Instance.SpawnObject("ExplositionSpriteAnimationSmall", transform.position, Quaternion.identity);
            Despawn();
        }
    }
}
