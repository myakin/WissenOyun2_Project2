using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMovementManager : MonoBehaviour {
    public bool executeScript;
    public string prefabName;
    public float movePoint;
    public float moveSpeed = 0.1f;
    public Transform joint;
    public GameObject pairObject;

    private void Start() {
        if (executeScript) {
            pairObject = Instantiate(Resources.Load(prefabName) as GameObject, joint);
            pairObject.transform.localPosition = Vector3.zero;
        }

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        if (executeScript) {
            //transform.position = new Vector3(transform.position.x-moveSpeed, 0, 0);
            transform.position += -transform.right * moveSpeed;

            if (transform.position.x<=movePoint) {
                pairObject.transform.SetParent(null);
                transform.SetParent(pairObject.GetComponent<EndlessMovementManager>().joint);
                transform.localPosition=Vector3.zero;
                pairObject.GetComponent<EndlessMovementManager>().pairObject = gameObject;
                if (GetComponent<ColumnGenerator>()) {
                    GetComponent<ColumnGenerator>().GenerateColumns();
                }
                pairObject.GetComponent<EndlessMovementManager>().executeScript=true;
                executeScript=false;
            }
        }
    }
}
