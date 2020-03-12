using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float forceMagnitude = 100f;

    
    private void Update() {
        
        if (Input.GetMouseButtonDown(0)) {
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
}
