using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class CharacterCreationMenu : MonoBehaviour
{
    [System.Serializable]
    public class CharacterPart
    {
        public string partName;
        public Texture2D preview;
        public string resourcePath;
    }

    [SerializeField] private Transform contentPanel; // ScrollView 的 Content
    [SerializeField] private GameObject partButtonPrefab; // 按鈕預製體
    [SerializeField] private Image characterPreview; // 角色預覽圖
    [SerializeField] private string resourcesPath = "CharacterParts"; // Resources 資料夾路徑
    
    private Dictionary<string, List<CharacterPart>> characterParts = new Dictionary<string, List<CharacterPart>>();
    private Dictionary<string, Texture2D> selectedParts = new Dictionary<string, Texture2D>();

    void Start()
    {
        LoadCharacterParts();
        CreateMenu();
    }

    // 從 Resources 資料夾加載角色部件
    void LoadCharacterParts()
    {
        // 假設資料夾結構：Resources/CharacterParts/Head/、Resources/CharacterParts/Body/ 等
        string[] partCategories = { "Head", "Body", "Legs", "Accessories" };

        foreach (string category in partCategories)
        {
            string path = $"{resourcesPath}/{category}";
            Texture2D[] textures = Resources.LoadAll<Texture2D>(path);
            
            if (textures.Length > 0)
            {
                List<CharacterPart> parts = new List<CharacterPart>();
                foreach (Texture2D texture in textures)
                {
                    parts.Add(new CharacterPart
                    {
                        partName = texture.name,
                        preview = texture,
                        resourcePath = path
                    });
                }
                characterParts[category] = parts;
            }
        }

        Debug.Log($"已加載 {characterParts.Count} 個角色部件類別");
    }

    // 動態建立選單 UI
    void CreateMenu()
    {
        foreach (var category in characterParts)
        {
            // 建立分類標題
            Text categoryTitle = new GameObject($"Title_{category.Key}").AddComponent<Text>();
            categoryTitle.text = category.Key;
            categoryTitle.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            categoryTitle.fontSize = 20;
            categoryTitle.fontStyle = FontStyle.Bold;
            categoryTitle.transform.SetParent(contentPanel);

            // 建立該分類的水平佈局群組
            GameObject horizontalLayout = new GameObject($"Layout_{category.Key}");
            horizontalLayout.transform.SetParent(contentPanel);
            HorizontalLayoutGroup hlg = horizontalLayout.AddComponent<HorizontalLayoutGroup>();
            hlg.childForceExpandWidth = false;
            hlg.childForceExpandHeight = false;

            // 為每個部件建立按鈕
            foreach (var part in category.Value)
            {
                CreatePartButton(part, category.Key, horizontalLayout.transform);
            }
        }
    }

    // 建立個別部件按鈕
    void CreatePartButton(CharacterPart part, string category, Transform parent)
    {
        GameObject buttonObj = Instantiate(partButtonPrefab, parent);
        buttonObj.name = part.partName;

        Button btn = buttonObj.GetComponent<Button>();
        Image btnImage = buttonObj.GetComponent<Image>();

        // 設定按鈕圖片
        if (part.preview != null)
        {
            btnImage.sprite = Sprite.Create(part.preview, 
                new Rect(0, 0, part.preview.width, part.preview.height), 
                Vector2.zero);
        }

        // 添加按鈕點擊事件
        btn.onClick.AddListener(() => SelectPart(category, part));

        // 設定按鈕大小提示
        LayoutElement layoutElement = buttonObj.AddComponent<LayoutElement>();
        layoutElement.preferredWidth = 100;
        layoutElement.preferredHeight = 100;
    }

    // 選擇角色部件
    void SelectPart(string category, CharacterPart part)
    {
        selectedParts[category] = part.preview;
        Debug.Log($"已選擇：{category} - {part.partName}");
        UpdatePreview();
    }

    // 更新預覽圖
    void UpdatePreview()
    {
        // 如果有選擇的頭部，顯示該預覽
        if (selectedParts.ContainsKey("Head") && selectedParts["Head"] != null)
        {
            Sprite previewSprite = Sprite.Create(selectedParts["Head"],
                new Rect(0, 0, selectedParts["Head"].width, selectedParts["Head"].height),
                Vector2.zero);
            characterPreview.sprite = previewSprite;
        }
    }

    // 確認角色建立
    public void ConfirmCharacter()
    {
        string selectedInfo = "已選擇的部件：\n";
        foreach (var part in selectedParts)
        {
            selectedInfo += $"{part.Key}: {part.Value.name}\n";
        }
        Debug.Log(selectedInfo);
        // 在這裡進行角色建立邏輯
    }

    // 重置選擇
    public void ResetSelection()
    {
        selectedParts.Clear();
        characterPreview.sprite = null;
        Debug.Log("已重置角色選擇");
    }
}