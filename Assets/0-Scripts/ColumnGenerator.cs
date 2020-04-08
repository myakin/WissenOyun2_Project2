using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnGenerator : MonoBehaviour {
    public string[] prefabNames;
    private List<GameObject> generatedObjects = new List<GameObject>();

    

    // recursive function yaklasimi++++++++++++++++++++++++++++++++++++++++
    // public void GenerateColumns() {
    //     if (generatedObjects.Count>0) {
    //         DestroyGeneration();
    //     }

    //     int numOfgenerations = Random.Range(1,3);
    //     for (int i=0; i<numOfgenerations; i++) {
    //     int chosenPrefabIndex = Random.Range(0, prefabNames.Length);
    //         GameObject generatedObject = Instantiate(Resources.Load(prefabNames[chosenPrefabIndex]) as GameObject, transform);
    //         float generationX = GetGenerationPoint(generatedObject, GetGenerationBounds(generatedObject));
    //         generatedObject.transform.localPosition = new Vector3(
    //             generationX,
    //             generatedObject.transform.localPosition.y,
    //             generatedObject.transform.localPosition.z
    //         );
    //         generatedObjects.Add(generatedObject);
    //     }
    // }
    // public void DestroyGeneration() {
    //     for (int i=0; i<generatedObjects.Count; i++) {
    //         Destroy(generatedObjects[i]);
    //     }
    //     generatedObjects.Clear();
    // }

    // private float[] GetGenerationBounds(GameObject aTargetObject) {
    //     return new float[] { 
    //         aTargetObject.transform.position.x - aTargetObject.GetComponent<ObjectBoundsDefiner>().dummyLeft.transform.position.x,
    //         aTargetObject.GetComponent<ObjectBoundsDefiner>().dummyRight.transform.position.x - aTargetObject.transform.position.x
    //     };
    // }

    // private float GetGenerationPoint(GameObject aTargetObject, float[] targetObjBounds) {
    //     float myWidth = GetComponent<EndlessMovementManager>().joint.localPosition.x;
    //     float aLocalPositionX = Random.Range(0, myWidth);

    //     bool intersection = false;
    //     Debug.Log(aTargetObject.name + " " + targetObjBounds[0] + " " + targetObjBounds[1]);

    //     float[] objBoundPositions = new float[] { 
    //         aLocalPositionX - targetObjBounds[0],
    //         aLocalPositionX + targetObjBounds[1]  
    //     };

    //     for (int i=0; i<generatedObjects.Count; i++) {
    //         float[] currentObjBoundsDistances = GetGenerationBounds(generatedObjects[i]);
    //         float minPoint = generatedObjects[i].transform.localPosition.x - currentObjBoundsDistances[0];
    //         float maxPoint = generatedObjects[i].transform.localPosition.x + currentObjBoundsDistances[1];
    //         Debug.Log(generatedObjects[i].name + " " + minPoint + " " + maxPoint);

    //         if ( 
    //             (aLocalPositionX < generatedObjects[i].transform.localPosition.x && objBoundPositions[1] > minPoint) 
    //             || 
    //             (aLocalPositionX > generatedObjects[i].transform.localPosition.x && objBoundPositions[0] < maxPoint)
    //         ) {
    //             intersection=true;
    //         }
    //     }
    //     Debug.Log(intersection);
    //     if (intersection) {
    //         return GetGenerationPoint(aTargetObject, targetObjBounds);
    //     }
        
    //     return aLocalPositionX;
    // }
    // recursive function yaklasimi---------------------------------------


    // grid yaklasimi degiskenleri+++++++++++++++++++++++++++++++++++++++++
    private List<Vector3> gridPositions = new List<Vector3>(); 
    private List<int> emptyGrid = new List<int>();
    private List<int> occupiedGrid = new List<int>();
    public GameObject gridStartDummyObject, gridEndDummyObject, columnMinHeightDummyObject, columnMaxHeightDummyObject;
    
    
    // grid yaklasimi fonksiyonlari
    private int GetGridIndex() {
        int returnValue = -1;
        int chosenIndex = emptyGrid.Count>0 ? Random.Range(0, emptyGrid.Count) : -1;
        occupiedGrid.Add(chosenIndex);
        returnValue = emptyGrid[chosenIndex];
        emptyGrid.Remove(chosenIndex);
        return returnValue;
    }
    private Vector3 GetGenerationPosition() {
        return gridPositions[GetGridIndex()];
    }
    private void GenerateGrid() {
        int numberOfGridCells = 9;
        float distance = (gridStartDummyObject.transform.position - gridEndDummyObject.transform.position).magnitude;
        float gridWidth = distance/numberOfGridCells;
        for (int i=0; i<numberOfGridCells; i++) {
            Vector3 gridLocalPos = gridStartDummyObject.transform.localPosition + (gridStartDummyObject.transform.right * (gridWidth * (i + 1))) + (-gridStartDummyObject.transform.right * (gridWidth/2));
            gridPositions.Add(gridLocalPos);
            emptyGrid.Add(i); // ilk olusturulurken tum gridler bos, grid numarasini emptyGrid listesine ekliyoruz

            // grid pozisyonlarini debug etmek icin fake kureler kullandik... gercek oyunda comment'e alin
            // GameObject debugObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // debugObject.transform.SetParent(transform);
            // debugObject.transform.localPosition = gridLocalPos;
        }
        // 0 grid (gridStartDummyObject.transform.localPosition + gridWidth * 1)/2
        // 1 grid (gridStartDummyObject.transform.localPosition + gridWidth * 2)/2
        // 2 grid (gridStartDummyObject.transform.localPosition + gridWidth * 3)/2
        // .
        // .
        // .
        // 9 grid (gridStartDummyObject.transform.localPosition + gridWidth * 10)/2
    }
    private void Start() {
        GenerateGrid();
    }
    public void ResetGrids() {
        for (int i=0; i<occupiedGrid.Count; i++) {
            emptyGrid.Add(occupiedGrid[i]);
        }
        occupiedGrid.Clear();
    }
    public void GenerateColumns() {
        if (generatedObjects.Count>0) {
            DestroyGeneration();
            ResetGrids();
        }

        int numOfgenerations = Random.Range(1,3);
        for (int i=0; i<numOfgenerations; i++) {
            int chosenPrefabIndex = Random.Range(0, prefabNames.Length);
            GameObject generatedObject = Instantiate(Resources.Load(prefabNames[chosenPrefabIndex]) as GameObject, transform);
            Vector3 generatedLocalPos = GetGenerationPosition();
            float valueY = Random.Range(columnMinHeightDummyObject.transform.localPosition.y, columnMaxHeightDummyObject.transform.localPosition.y);
            generatedObject.transform.localPosition = new Vector3(generatedLocalPos.x, valueY, generatedLocalPos.z);
            generatedObjects.Add(generatedObject);

        }
    }
    public void DestroyGeneration() {
        for (int i=0; i<generatedObjects.Count; i++) {
            Destroy(generatedObjects[i]);
        }
        generatedObjects.Clear();
    }
    // grid yaklasimi degiskenleri----------------------------------------------
    
    
}
