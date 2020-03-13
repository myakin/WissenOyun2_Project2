using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnGenerator : MonoBehaviour {
    public string prefabName;
    private List<GameObject> generatedObjects = new List<GameObject>();

    public void GenerateColumns() {
        if (generatedObjects.Count>0) {
            DestroyGeneration();
        }

        int numOfgenerations = Random.Range(1,3);
        for (int i=0; i<numOfgenerations; i++) {
            GameObject generatedObject = Instantiate(Resources.Load(prefabName) as GameObject, transform);
            float generationX = GetGenerationPoint(generatedObject, GetGenerationBounds(generatedObject));
            generatedObject.transform.localPosition = new Vector3(
                generationX,
                generatedObject.transform.localPosition.y,
                generatedObject.transform.localPosition.z
            );
            generatedObjects.Add(generatedObject);
        }
    }
    public void DestroyGeneration() {
        for (int i=0; i<generatedObjects.Count; i++) {
            Destroy(generatedObjects[i]);
        }
        generatedObjects.Clear();
    }

    private float[] GetGenerationBounds(GameObject aTargetObject) {
        return new float[] { 
            aTargetObject.transform.position.x - aTargetObject.GetComponent<ObjectBoundsDefiner>().dummyLeft.transform.position.x,
            aTargetObject.GetComponent<ObjectBoundsDefiner>().dummyRight.transform.position.x - aTargetObject.transform.position.x
        };
    }

    private float GetGenerationPoint(GameObject aTargetObject, float[] targetObjBounds) {
        float myWidth = GetComponent<EndlessMovementManager>().joint.localPosition.x;
        float aLocalPositionX = Random.Range(0, myWidth);

        bool intersection = false;
        Debug.Log(aTargetObject.name + " " + targetObjBounds[0] + " " + targetObjBounds[1]);

        float[] objBoundPositions = new float[] { 
            aLocalPositionX - targetObjBounds[0],
            aLocalPositionX + targetObjBounds[1]  
        };

        for (int i=0; i<generatedObjects.Count; i++) {
            float[] currentObjBoundsDistances = GetGenerationBounds(generatedObjects[i]);
            float minPoint = generatedObjects[i].transform.localPosition.x - currentObjBoundsDistances[0];
            float maxPoint = generatedObjects[i].transform.localPosition.x + currentObjBoundsDistances[1];
            Debug.Log(generatedObjects[i].name + " " + minPoint + " " + maxPoint);

            if ( 
                (aLocalPositionX < generatedObjects[i].transform.localPosition.x && objBoundPositions[1] > minPoint) 
                || 
                (aLocalPositionX > generatedObjects[i].transform.localPosition.x && objBoundPositions[0] < maxPoint)
            ) {
                intersection=true;
            }
        }
        Debug.Log(intersection);
        if (intersection) {
            return GetGenerationPoint(aTargetObject, targetObjBounds);
        }
        
        return aLocalPositionX;
    }
    
}
