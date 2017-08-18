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
	void Start ()
	{
	    jsonString = File.ReadAllText(Application.dataPath + "/JSON files/SheepsDataBase.json");
	    itemData = JsonMapper.ToObject(jsonString);
	}

    public static JsonData getDataByName(string array, string name)
    {
        for (int i = 0; i < itemData[array].Count; i++)
        {
            if (itemData[array][i]["name"].ToString() == name)
                return itemData[array][i];
        }
        return null;
    }

    public static JsonData getDataByIndex(string array, int index)
    {
        if (itemData[array][index] != null) return itemData[array][index];
        return null;
    }
}
