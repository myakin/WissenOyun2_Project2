using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleColumnManager : MonoBehaviour {
    public GameObject[] columns; // index 0 her zaman ust obje olsun, index 1 her zaman alt obje olsun
    public int offsetApplictionMethod; // 0: ustteki parcaya uygula, 1: alttaki parcaya uygula, 2: her ikisine de uygula
    public float maxOffset;
    
    private void Start() {
        offsetApplictionMethod = Random.Range(0,3);
        float offsetY = Random.Range(0, maxOffset);

        if (offsetApplictionMethod==0) {
            columns[0].transform.localPosition += (-columns[0].transform.up * offsetY);
        } else if (offsetApplictionMethod==1) {
            columns[1].transform.localPosition += (-columns[1].transform.up * offsetY);
        } else if (offsetApplictionMethod==2) {
            columns[0].transform.localPosition += (-columns[0].transform.up * offsetY);
            columns[1].transform.localPosition += (-columns[1].transform.up * offsetY);
        }
        
    }
}
