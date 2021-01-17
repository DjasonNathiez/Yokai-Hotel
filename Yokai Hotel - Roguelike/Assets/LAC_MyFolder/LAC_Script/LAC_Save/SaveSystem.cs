﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem 
{
    public static void SaveProgress( KF_Unlockables unlockables, KF_LevelManager lvlM)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/progress.lose";

        FileStream stream = new FileStream(path,FileMode.Create);
        ProgressData data = new ProgressData(unlockables, lvlM);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    public static ProgressData LoadProgress()
    {
        string path = Application.persistentDataPath + "/progress.lose";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ProgressData data = formatter.Deserialize(stream) as ProgressData;
            stream.Close();
            //Debug.Log("Save file not found in : " + path);
            return data;
            
        }
        else
        {
            Debug.Log("Save file not found in : "+path);
            return null;
        }
    }
    public static bool SaveExist()
    {
        string path = Application.persistentDataPath + "/progress.lose";
        return (File.Exists(path));
    }

    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "/progress.lose";
        if (File.Exists(path))
            File.Delete(path);
        else
            Debug.Log("File already delete");
    }
 
}
