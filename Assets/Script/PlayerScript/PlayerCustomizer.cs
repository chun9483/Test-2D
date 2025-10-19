using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class PlayerCustomizer : MonoBehaviour
{
    [SerializeField] private PlayerBodyPartManager playerBodyPartManager;
    public readonly string[] bodyPartTypes = { "Hair", "Face", "Top", "Bottom", "Shoes" };

    public TMP_Text nameText;
    private string playerName;

    //for showing the player's outlook
    public GameObject playerModel; //this has body parts in its child
    private Transform playerModelTransform;
    //for get the childs(body parts)
    //can also set this on unity editor, 
    //but I just get this using getComponent on playerModel

    public Dictionary<string, List<Sprite>> allBodyPartSprites = new Dictionary<string, List<Sprite>>();

    private string currBodyPartType;

    //for UI -----------------------------------------------------
    //for generate body part options
    [SerializeField] private GameObject scrollView;
    [SerializeField] private GameObject viewport;
    [SerializeField] private GameObject emptyOptionsUI;
    [SerializeField] private GameObject emptySingleOption;
    private GameObject currOptionsUI; //curr body part options
    public Dictionary<string, GameObject> optionsUIs = new();


    void Start()
    {
        playerModelTransform = playerModel.GetComponent<Transform>();
        currBodyPartType = bodyPartTypes[0];
        loadPlayerPartSprites();
        generateOptionsUI();
        foreach (string type in bodyPartTypes)
        {
            changeBodyPart(type, playerBodyPartManager.bodyPartSprites[type]);
        }
    }

    public void loadPlayerPartSprites()
    {
        foreach (string type in bodyPartTypes)
        {
            List<Sprite> sprites = playerBodyPartManager.loadAllBodyPartSprites(type);
            allBodyPartSprites[type] = sprites;
            Debug.Log("load " + sprites.Count + " " + type);
        }
    }

    //Change body parts=============================================
    public void changeBodyPart(int i)
    {   //this one is for button
        //save the body part
        playerBodyPartManager.bodyPartSprites[currBodyPartType] = allBodyPartSprites[currBodyPartType][i];

        //find the hair part in the present playerModel and replace it
        Transform childTransform = playerModelTransform.Find(currBodyPartType);
        if (childTransform != null)
        {
            SpriteRenderer childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
            if (childSpriteRenderer != null)
            {
                childSpriteRenderer.sprite = allBodyPartSprites[currBodyPartType][i];
            }
        }
    }
    public void changeBodyPart(string type, Sprite sprite)
    {
        playerBodyPartManager.bodyPartSprites[type] = sprite;

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

    //UI===============================================================================
    //for generate body part options UI
    public void generateOptionsUI()
    {
        Vector3 startPos = new Vector3(150, -190, 0);
        //add body part options to each body part option UI
        foreach (string type in bodyPartTypes)
        {
            //for adjesting position of each options
            Vector3 currOptionsPos = startPos;
            int offsetX = 270;

            GameObject newOptionsUI = Instantiate(emptyOptionsUI);
            newOptionsUI.SetActive(true);
            newOptionsUI.name = type + "Options";

            //add body part options
            int i = 0; //i-th body part
            foreach (Sprite sprite in allBodyPartSprites[type])
            {
                GameObject newSingleOption = Instantiate(emptySingleOption);
                newSingleOption.SetActive(true);

                Image image = newSingleOption.GetComponentInChildren<Image>();
                if (image != null)
                    image.sprite = sprite;

                newSingleOption.transform.SetParent(newOptionsUI.transform, false);
                newSingleOption.transform.localPosition = currOptionsPos;

                Button button = newSingleOption.GetComponent<Button>();
                int index = i;
                button.onClick.AddListener(() => { changeBodyPart(index); });

                i++;
                currOptionsPos.x += offsetX;
            }
            optionsUIs[type] = newOptionsUI;
            newOptionsUI.transform.SetParent(viewport.transform, false);
            newOptionsUI.SetActive(false);
        }
        currOptionsUI = optionsUIs[bodyPartTypes[0]];
        currOptionsUI.SetActive(true);
        scrollView.GetComponent<ScrollRect>().content = currOptionsUI.GetComponent<RectTransform>();

        emptyOptionsUI.SetActive(false); //hide the emptyOptionsUI
    }

    //for changing curr option UI
    public void changeCurrOptionsUI(string type)
    {
        if (currOptionsUI != null) currOptionsUI.SetActive(false);

        currBodyPartType = type;
        currOptionsUI = optionsUIs[type];
        currOptionsUI.SetActive(true);
        scrollView.GetComponent<ScrollRect>().content = currOptionsUI.GetComponent<RectTransform>();

    }

    //Save the body part / naem info=====================================================
    public void comfirm()
    {
        if (nameText.text != "")
        {
            playerName = nameText.text;
        }
        else
        {
            playerName = "Nameless";
        }
        playerBodyPartManager.saveBodyParts();
    }
}
