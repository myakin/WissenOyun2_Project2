using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAwardManager : MonoBehaviour {
    public int awardAmount = 10;
    
    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log(other.tag);
        if (other.tag=="Player") {
            if (other.GetComponentInParent<PlayerControllerForManuelSetup>()) {
                other.GetComponentInParent<PlayerControllerForManuelSetup>().AddToScore(awardAmount);
            } else if (other.GetComponent<PlayerControllerForManuelSetup>()) {
                other.GetComponent<PlayerControllerForManuelSetup>().AddToScore(awardAmount);
            }
            ObjectSpawnHandler.Instance.SpawnObject("CoinCollect_Particle System", transform.position, Quaternion.identity);
            // Destroy(gameObject);
            ObjectSpawnHandler.Instance.DespawnObject(gameObject);
        }
    }
}
