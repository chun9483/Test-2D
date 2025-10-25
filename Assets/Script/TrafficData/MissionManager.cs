using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    public GameObject missionMarkerPrefab; // 接收任務標記 Prefab
    // 用字典追蹤當前啟動的任務，鍵為據點 ID
    private Dictionary<string, GameObject> activeMissions = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckAndDeployMission()
    {
        if (DataSimulator.Instance == null) return; // 安全檢查

        // 1. 識別人少的據點
        var lowPopLocations = DataSimulator.Instance.allLocations
            // 篩選人數少於閾值的據點
            .Where(loc => loc.currentPopulation < DataSimulator.Instance.lowPopulationThreshold)
            .ToList();

        Debug.Log($"[MissionManager] Found {lowPopLocations.Count} low population locations.");

        // 2. 清除不再符合條件的舊任務 (脫離低人流的據點)
        var locationsToRemove = activeMissions.Keys
            // 找出在 activeMissions 中，但不在當前 lowPopLocations 列表中的 ID
            .Except(lowPopLocations.Select(l => l.locationID))
            .ToList();

        foreach (var id in locationsToRemove)
        {
            Destroy(activeMissions[id]);
            activeMissions.Remove(id);
            Debug.Log($"[MissionManager] Mission REMOVED from {id} (Population normalized).");
        }

        // 3. 在新符合條件的據點推送/生成任務
        foreach (var location in lowPopLocations)
        {
            // 檢查該據點是否已經有任務
            if (!activeMissions.ContainsKey(location.locationID))
            {
                if (missionMarkerPrefab == null)
                {
                    Debug.LogError("MissionManager: missionMarkerPrefab is NOT set in the Inspector!");
                    continue;
                }

                // **任務生成/推送**
                // 實例化標記並將其位置設為據點的位置
                GameObject marker = Instantiate(missionMarkerPrefab, location.assignmentPoint.position, Quaternion.identity);
                activeMissions.Add(location.locationID, marker);

                Debug.Log($"*** Mission PUSHED to {location.locationID} (Pop: {location.currentPopulation}) ***");
            }
        }
    }
}