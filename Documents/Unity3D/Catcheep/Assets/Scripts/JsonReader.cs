using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class JsonReader : MonoBehaviour
{
    private string jsonString;
    private static JsonData itemData;

    //Use this for initialization
    void Start()
    {
        print(getDataFromJson(getDataByIndex("sheeps", 0),"time"));
    }

    public static JsonData getJsonFile(string path)
    {
        string newPath = path.Replace(".json", "");
        TextAsset file = Resources.Load(newPath) as TextAsset;
        string jsonString = file.ToString();
        //string jsonString = File.ReadAllText(Application.dataPath + "/JSON files/" + path);
        return itemData = JsonMapper.ToObject(jsonString);
    }

    //path is original json,array is top of json file, needed neededURL is what u want from original json
    //this function gives back a needed json file
    public static JsonData getJsonByName(string path, string array, string neededURL)
    {
        JsonData itemData = getJsonFile(path);

        for (int i = 0; i < itemData[array].Count; i++)
        {
            if (itemData[array][i]["url"].ToString() == neededURL)
                return itemData[array][i]["url"];
        }
        return null;
    }

    public static String getDataFromJson(JsonData json, string attribute)
    {
          return itemData[attribute].ToString();
    }

    public static JsonData getDataByIndex(string array, int index)
    {
        itemData = getJsonFile("sheepsDataBase.json");
        if (itemData[array][index] != null) return itemData[array][index]["url"];
        return null;
    }

    public static void timeModifier(string url)
    {
        string json = getJsonByName("sheepsDataBase.json", "sheeps", url).ToString();
        json = getJsonFile(json).ToJson();
        
        Sheep mySheep = new Sheep();
        mySheep = JsonUtility.FromJson<Sheep>(json);
        mySheep.timeOfSell = DateTime.Now.ToString();
        JsonData temp = JsonMapper.ToJson(mySheep);
        File.WriteAllText(Application.dataPath + "/JSON files/" +"sheepy.json", temp.ToString());
    }
}

[Serializable]
public class Sheep
{
    public string name;
    public int time;
    public string timeOfSell;
}