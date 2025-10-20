using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerBodyPartManager : MonoBehaviour
{
    private readonly string bodyPartSpriteDir = "PlayerBodyParts";
    private string playerInfoFileDir;
    private readonly string bodyPartFileName = "PlayerBodyParts.json";

    public readonly string[] bodyPartTypes = { "Hair", "Face", "Top", "Bottom", "Shoes" };

    public Dictionary<string, string> bodyPartNames = new Dictionary<string, string>();
    public Dictionary<string, Sprite> bodyPartSprites = new Dictionary<string, Sprite>();

    public GameObject playerModel;

    void Awake()
    {
        playerInfoFileDir = Application.persistentDataPath + "/PlayerInfo";
         
        if (File.Exists(playerInfoFileDir + "/" + bodyPartFileName))
        {
            string json = File.ReadAllText(playerInfoFileDir + "/" + bodyPartFileName);
            bodyPartNames = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Debug.Log(json);
        }
        
        foreach (string type in bodyPartTypes)
        {
            if (!bodyPartNames.ContainsKey(type))
            {
                bodyPartNames.Add(type, type + "_0"); //default
            }

            Sprite sprite = loadBodyPartSprit(type, bodyPartNames[type]);
            if (sprite != null)
            {
                bodyPartSprites[type] = sprite;
            }
            else
            {
                bodyPartNames[type] = type + "_0"; //default
                bodyPartSprites[type] = loadBodyPartSprit(type, bodyPartNames[type]);
            }

            if(playerModel != null)
            {
                Transform playerModelTransform = playerModel.GetComponent<Transform>();
                
                //find the hair part in the present playerModel and replace it
                Transform childTransform = playerModelTransform.Find(type);
                if (childTransform != null)
                {
                    SpriteRenderer childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
                    if (childSpriteRenderer != null)
                    {
                        childSpriteRenderer.sprite = sprite;
                    }
                }
            }
        }
    }


    //save and load =============================================================================
    public Sprite loadBodyPartSprit(string type, string name)
    {
        string path = bodyPartSpriteDir + "/" + type + "/" + name;
        Sprite sprite = Resources.Load<Sprite>(path);
        return sprite;
    }

    public List<Sprite> loadAllBodyPartSprites(string type)
    {
        return Resources.LoadAll<Sprite>(bodyPartSpriteDir + "/" + type).ToList();
    }

    public void saveBodyParts()
    {

        Debug.Log(playerInfoFileDir);
        bool isFileExist = Directory.Exists(playerInfoFileDir);
        if (!isFileExist)
        {
            Directory.CreateDirectory(playerInfoFileDir);
            Debug.Log("create");
        }
        else
        {
            Debug.Log("Exists");
        }

        foreach (string type in bodyPartTypes)
        {
            bodyPartNames[type] = bodyPartSprites[type].name;
            Debug.Log(bodyPartNames[type]);
        }

        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Application.persistentDataPath + "/" + playerInfoFilePath + "/PlayerBodyParts");
        string json = JsonConvert.SerializeObject(bodyPartNames);
        Debug.Log(json);
        File.WriteAllText(playerInfoFileDir + "/" + bodyPartFileName, json);
        //bf.Serialize(file, json);
        //file.Close();
    }
}
