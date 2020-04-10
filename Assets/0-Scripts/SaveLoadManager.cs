using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour {
    public static SaveLoadManager Instance; 
    private string checkpointFilePath;

    public int playerScore;

    private void Awake() {
        if (SaveLoadManager.Instance == null) { // sistemde bir SaveLoadManager yok, ilk atanan ben olayim
            SaveLoadManager.Instance = this;
            DontDestroyOnLoad(gameObject); // beni asla yok etme
        } else { // Sistemde daha onceden atanmis bir SaveLoadManager var
            if (SaveLoadManager.Instance != this) { // o SaveLoadManager ben miyim? Degilsem, kendimi feda edeyim ki, bilgiyi tasiyan esas SaveLoadManager kurtulsun
                Destroy(gameObject);
            }
        }
        checkpointFilePath = Application.persistentDataPath + "/checkpointSave.cps";
    }

    // int counter = 0;
    public void SaveGame() {
        // Debug.Log("Saved "+counter);
        // counter++;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(checkpointFilePath);

        bf.Serialize(fs, player.transform.position.x);
        bf.Serialize(fs, player.transform.position.y);
        bf.Serialize(fs, player.transform.position.z);

        bf.Serialize(fs, player.transform.rotation.x);
        bf.Serialize(fs, player.transform.rotation.y);
        bf.Serialize(fs, player.transform.rotation.z);
        bf.Serialize(fs, player.transform.rotation.w);

        Vector2 currentVelocity = player.GetComponent<Rigidbody2D>().velocity;
        bf.Serialize(fs, currentVelocity.x);
        bf.Serialize(fs, currentVelocity.y);

        bf.Serialize(fs, player.GetComponent<Rigidbody2D>().angularVelocity);

        bf.Serialize(fs, player.GetComponent<PlayerControllerForManuelSetup>().GetScore());

        fs.Close();

    }  

    public bool CheckSavedGame() {
        return File.Exists(checkpointFilePath);
    }

    public bool LoadGame() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(checkpointFilePath)) {
            FileStream fs = File.Open(checkpointFilePath, FileMode.Open);

            player.transform.position = new Vector3( (float)bf.Deserialize(fs) , (float)bf.Deserialize(fs), (float)bf.Deserialize(fs));
            player.transform.rotation = new Quaternion((float)bf.Deserialize(fs), (float)bf.Deserialize(fs), (float)bf.Deserialize(fs), (float)bf.Deserialize(fs));
            
            player.GetComponent<Rigidbody2D>().velocity = new Vector2((float)bf.Deserialize(fs), (float)bf.Deserialize(fs));
            player.GetComponent<Rigidbody2D>().angularVelocity = (float)bf.Deserialize(fs);

            player.GetComponent<PlayerControllerForManuelSetup>().SetScore((int)bf.Deserialize(fs));

            fs.Close();

            return true;
        }
        return false;
    } 


}
