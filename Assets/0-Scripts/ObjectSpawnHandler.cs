using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnHandler : MonoBehaviour {
    public static ObjectSpawnHandler Instance;

    private Dictionary<string, List<GameObject>> objectPool = new Dictionary<string, List<GameObject>>(); // sahnede aktif olmayanlar
    private Dictionary<string, List<GameObject>> activeObjectsInScene = new Dictionary<string, List<GameObject>>(); // sahnede aktif olanlar


    private void Awake() {
        if (ObjectSpawnHandler.Instance==null) {
            ObjectSpawnHandler.Instance = this;
        } else {
            if (ObjectSpawnHandler.Instance!=this) {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnObject(string nameOfObjectToSpawn, Vector3 aWorldPosition, Quaternion aWorldRotation, bool isEffect = false) {
        if (objectPool.ContainsKey(nameOfObjectToSpawn) && objectPool[nameOfObjectToSpawn].Count>0) {
            GameObject objToSpawn = objectPool[nameOfObjectToSpawn][0];
            objectPool[nameOfObjectToSpawn].RemoveAt(0);

            objToSpawn.transform.position = aWorldPosition;
            objToSpawn.transform.rotation = aWorldRotation;
            objToSpawn.SetActive(true);

            if (!activeObjectsInScene.ContainsKey(nameOfObjectToSpawn)) {
                List<GameObject> newGameObjectPool = new List<GameObject>();
                activeObjectsInScene.Add(nameOfObjectToSpawn, newGameObjectPool);
            }
            activeObjectsInScene[nameOfObjectToSpawn].Add(objToSpawn);
            
        } else {
            GameObject spawnedObject = Instantiate(Resources.Load(nameOfObjectToSpawn) as GameObject, aWorldPosition, aWorldRotation);
            if (!activeObjectsInScene.ContainsKey(nameOfObjectToSpawn)) {
                List<GameObject> newGameObjectPool = new List<GameObject>();
                activeObjectsInScene.Add(nameOfObjectToSpawn, newGameObjectPool);
            }
            activeObjectsInScene[nameOfObjectToSpawn].Add(spawnedObject);

        }
    }

    public void DespawnObject(GameObject anObject) {
        anObject.SetActive(false);
        
        string nameOfGameObject = anObject.name;
        if (nameOfGameObject.Contains("(Clone)")) {
            nameOfGameObject = nameOfGameObject.Substring(0, nameOfGameObject.Length-7);
        }

        if (activeObjectsInScene.ContainsKey(nameOfGameObject)) {
            for (int i=0; i<activeObjectsInScene[nameOfGameObject].Count; i++) {
                if (activeObjectsInScene[nameOfGameObject][i] == anObject) {
                    activeObjectsInScene[nameOfGameObject].Remove(anObject);
                    break;
                }
            }
        }

        if (objectPool.ContainsKey(nameOfGameObject)) {
            objectPool[nameOfGameObject].Add(anObject);
        } else {
            List<GameObject> newGameObjectPool = new List<GameObject>();
            newGameObjectPool.Add(anObject);
            objectPool.Add(nameOfGameObject, newGameObjectPool);
        }
    }

    // ekler
    public void ClearEffects() {
        foreach (KeyValuePair<string, List<GameObject>> entry in activeObjectsInScene) {
            if (entry.Key.Contains("Particle System")) {
                for (int i=entry.Value.Count-1; i>=0; i--) {
                    DespawnObject(entry.Value[i]);
                    // Debug.Log("Moving object to pool at i="+i+" name="+entry.Value[i].name);
                }
            }
        }
    }

    public void ReSpawnCoins() {
        foreach(KeyValuePair<string, List<GameObject>> entry in objectPool) {
            if (objectPool.ContainsKey("CoinSprite")) {
                for (int i = objectPool["CoinSprite"].Count-1; i>=0; i--) {
                    objectPool["CoinSprite"][i].SetActive(true);
                    
                    if (!activeObjectsInScene.ContainsKey("CoinSprite")) {
                        List<GameObject> newList = new List<GameObject>();
                        activeObjectsInScene.Add("CoinSprite", newList);
                    }
                    activeObjectsInScene["CoinSprite"].Add(objectPool["CoinSprite"][i]);

                    objectPool["CoinSprite"].RemoveAt(i);
                }
            } 
        }
    }
}
// List 1 -> 0 1 2 3 4 buraan list2ye eklemek istiyorum
// List 2 -> 
// List 1in loopu 0'dan baslarsa
// 0 i List 1den kaldirdim. Artik list 1 -> 1 2 3 4
// 0 i List 2ye ekledim.
// List 1in index 1deki elemani kimdir?
// 2
// oysa benim List 1i loop eden for loopuma gore index 1deki elemanin 1 olmasi gerekiyordu (en basta)

// List 1 -> 0 1 2 3 4 buraan list2ye eklemek istiyorum
// List 2 -> 
// List 1in loopu Count-1'den baslarsa
// 4u List 2'ye aktardim. List 1 -> 0 1 2 3
// benim su anki indexNo'm (4un index nosu): 4
// benim sonraki indexNo.m ne? 3
// 3 hala listede var mi ve bastakiyle ayni eleman mi? evet 
