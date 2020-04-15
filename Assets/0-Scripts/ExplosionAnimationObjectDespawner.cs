using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimationObjectDespawner : MonoBehaviour {
    public void DespawnExplosion() {
        ObjectSpawnHandler.Instance.DespawnObject(gameObject);
    }
}
