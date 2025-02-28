using System.IO;
using BootStrapScripts;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    public class SaveSystem : DoNotDestroyOnLoad
    {
    private const string FileName = "BlobMaze.json";
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, FileName);

    public static SaveFileData SaveFileData { get; private set; }
    public static bool SaveDataFileExists => File.Exists(SaveFilePath);
    public static SaveSystem Instance { get; private set; }
    
    private void OnEnable() => Instance = this;
    private void OnDisable() => Instance = Instance == this ? null : Instance;
    public static void SaveProgression(SaveFileData saveFileData)
    {
        if (SaveDataFileExists) File.Delete(SaveFilePath);
        string j = JsonConvert.SerializeObject(saveFileData, Formatting.Indented, new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        File.WriteAllText(SaveFilePath, j);
    }

    public static void LoadProgression() { SaveFileData = File.Exists(SaveFilePath) ? JsonConvert.DeserializeObject<SaveFileData>(File.ReadAllText(SaveFilePath)) : new SaveFileData(); }
    }
}

