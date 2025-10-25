using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataSimulator : MonoBehaviour
{
    // --- 外部連接與單例 ---
    public Transform[] initialLocationPoints; // 接收據點物件
    public static DataSimulator Instance;

    // --- 內部數據 ---
    [HideInInspector] // 不在 Inspector 顯示，由 Initialize 填充
    public List<LocationData> allLocations = new List<LocationData>();
    private float timer;

    // --- 參數調整區 (可在 Inspector 中配置) ---
    public float updateInterval = 2.0f;           // 每隔 2 秒更新數據
    public int lowPopulationThreshold = 20;       // 人少的定義
    public int minPopulation = 5;                 // 人數下限
    public int maxPopulation = 40;                // 人數上限
    public int populationChangeRange = 5;        // 每次更新人數變動的範圍 (+/- 10)


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 確保物件在場景切換時不會被銷毀 (可選)
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Initialize(); // 遊戲啟動時呼叫初始化
    }

    public void Initialize()
    {
        if (initialLocationPoints.Length == 0)
        {
            Debug.LogError("DataSimulator: initialLocationPoints is empty! Please drag location objects into the array in the Inspector.");
            return;
        }

        for (int i = 0; i < initialLocationPoints.Length; i++)
        {
            Transform point = initialLocationPoints[i];
            int initialPop = Random.Range(minPopulation, maxPopulation);
            allLocations.Add(new LocationData($"LOC-{i:000}", point, initialPop));
        }

        Debug.Log($"[DataSimulator] Successfully initialized {allLocations.Count} locations.");
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            UpdateAllPopulations();
            // 確保 MissionManager 存在才呼叫
            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.CheckAndDeployMission();
            }
            timer = 0f;
        }
    }

    void UpdateAllPopulations()
    {
        foreach (var location in allLocations)
        {
            // 隨機波動
            int change = Random.Range(-populationChangeRange, populationChangeRange + 1);
            location.currentPopulation += change;

            // 限制人數在設定範圍內
            location.currentPopulation = Mathf.Clamp(location.currentPopulation, minPopulation, maxPopulation);

            // 輸出狀態，方便調試
            string status = (location.currentPopulation < lowPopulationThreshold) ? "LOW POP" : "NORMAL";
            Debug.Log($"[Simulator] ID: {location.locationID}, Pop: {location.currentPopulation}, Status: {status}");
        }
    }
}