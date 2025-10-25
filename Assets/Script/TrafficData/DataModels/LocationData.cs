using UnityEngine;

[System.Serializable]
public class LocationData
{
    public string locationID; // ���I�ߤ@�ѧO�X (�Ҧp: "A-001")
    [Range(0, 100)]
    public int currentPopulation; // ��e�H��
    public Transform assignmentPoint; // ���ȥͦ�/��ܪ���ڦ�m

    public LocationData(string id, Transform point, int initialPop)
    {
        locationID = id;
        assignmentPoint = point;
        currentPopulation = initialPop;
    }
}