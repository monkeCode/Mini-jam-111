using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProgressData
{
    [Serializable]
    public class SavableData
    {
        public int manaPoints;
        public List<int> unlockedItems;
    }
    
    public static SavableData Data { get; private set; }
    private static readonly string _savePath = Application.persistentDataPath + "/save.json";
    
    static GameProgressData()
    {
        Load();
    }

    public static void Load()
    {
        try
        {
            var json = System.IO.File.ReadAllText(_savePath);
            Data = JsonUtility.FromJson<SavableData>(json);
        }
        catch (Exception)
        {
            Data = new SavableData();
        }
    }
    
    public static void Save()
    {
        if(Data != null)
            System.IO.File.WriteAllText(_savePath, JsonUtility.ToJson(Data)  );
    }
}
