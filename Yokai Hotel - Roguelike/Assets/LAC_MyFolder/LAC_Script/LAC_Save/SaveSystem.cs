using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem 
{
    public static void SaveProgress( KF_Unlockables unlockables)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/progress.lose";

        FileStream stream = new FileStream(path,FileMode.Create);
        ProgressData data = new ProgressData(unlockables);

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

            ProgressData data = (ProgressData)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in : "+path);
            return null;
        }
    }
}
