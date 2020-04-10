using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {
    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            SaveLoadManager.Instance.SaveGame();
        }
    }   
}
