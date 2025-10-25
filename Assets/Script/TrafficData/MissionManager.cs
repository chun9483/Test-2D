using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    public GameObject missionMarkerPrefab; // �������ȼаO Prefab
    // �Φr��l�ܷ�e�Ұʪ����ȡA�䬰���I ID
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
        if (DataSimulator.Instance == null) return; // �w���ˬd

        // 1. �ѧO�H�֪����I
        var lowPopLocations = DataSimulator.Instance.allLocations
            // �z��H�Ƥ֩��H�Ȫ����I
            .Where(loc => loc.currentPopulation < DataSimulator.Instance.lowPopulationThreshold)
            .ToList();

        Debug.Log($"[MissionManager] Found {lowPopLocations.Count} low population locations.");

        // 2. �M�����A�ŦX�����¥��� (�����C�H�y�����I)
        var locationsToRemove = activeMissions.Keys
            // ��X�b activeMissions ���A�����b��e lowPopLocations �C���� ID
            .Except(lowPopLocations.Select(l => l.locationID))
            .ToList();

        foreach (var id in locationsToRemove)
        {
            Destroy(activeMissions[id]);
            activeMissions.Remove(id);
            Debug.Log($"[MissionManager] Mission REMOVED from {id} (Population normalized).");
        }

        // 3. �b�s�ŦX���󪺾��I���e/�ͦ�����
        foreach (var location in lowPopLocations)
        {
            // �ˬd�Ӿ��I�O�_�w�g������
            if (!activeMissions.ContainsKey(location.locationID))
            {
                if (missionMarkerPrefab == null)
                {
                    Debug.LogError("MissionManager: missionMarkerPrefab is NOT set in the Inspector!");
                    continue;
                }

                // **���ȥͦ�/���e**
                // ��ҤƼаO�ñN���m�]�����I����m
                GameObject marker = Instantiate(missionMarkerPrefab, location.assignmentPoint.position, Quaternion.identity);
                activeMissions.Add(location.locationID, marker);

                Debug.Log($"*** Mission PUSHED to {location.locationID} (Pop: {location.currentPopulation}) ***");
            }
        }
    }
}