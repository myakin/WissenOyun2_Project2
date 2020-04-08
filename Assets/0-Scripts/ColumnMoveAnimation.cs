using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnMoveAnimation : MonoBehaviour {
    public float offsetY = 2f;
    public float duration = 1.5f;
    private Vector3 initialLocalPos;
    private Vector3 targetLocalPos;


    private void Start() {
        initialLocalPos = transform.localPosition;
        targetLocalPos = initialLocalPos + (-transform.up * offsetY);
        StartCoroutine(AnimateDecompress());
    }

    private IEnumerator AnimateDecompress() {
        float timer = 0;
        while (timer<duration) {
            transform.localPosition = Vector3.Lerp(initialLocalPos, targetLocalPos, timer/duration);
            timer+=Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetLocalPos;
        StartCoroutine(AnimateCompress());
    }
    private IEnumerator AnimateCompress() {
        float timer = 0;
        while (timer<duration) {
            transform.localPosition = Vector3.Lerp(targetLocalPos, initialLocalPos, timer/duration);
            timer+=Time.deltaTime;
            yield return null;
        }
        transform.localPosition = initialLocalPos;
        StartCoroutine(AnimateDecompress());
    }
}
