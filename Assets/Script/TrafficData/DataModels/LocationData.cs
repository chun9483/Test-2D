using UnityEngine;

[System.Serializable]
public class LocationData
{
    public string locationID; // 據點唯一識別碼 (例如: "A-001")
    [Range(0, 100)]
    public int currentPopulation; // 當前人數
    public Transform assignmentPoint; // 任務生成/顯示的實際位置

    public LocationData(string id, Transform point, int initialPop)
    {
        locationID = id;
        assignmentPoint = point;
        currentPopulation = initialPop;
    }
}