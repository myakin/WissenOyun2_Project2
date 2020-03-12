using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMovementManager : MonoBehaviour {
    public Transform groundSet1, groundSet2;
    public Transform spawnPointDummy, movePointDummy;
    public float moveSpeed = 0.1f;

    private void Update() {
        groundSet1.transform.position = new Vector3(
            groundSet1.transform.position.x-moveSpeed,
            0,
            0);
        groundSet2.transform.position = new Vector3(
            groundSet2.transform.position.x-moveSpeed,
            0,
            0);

        if (groundSet1.transform.position.x < movePointDummy.position.x) {
            groundSet1.transform.position = spawnPointDummy.position;
        }
        if (groundSet2.transform.position.x < movePointDummy.position.x) {
            groundSet2.transform.position = spawnPointDummy.position;
        }
    }
}
