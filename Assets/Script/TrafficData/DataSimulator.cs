using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataSimulator : MonoBehaviour
{
    // --- �~���s���P��� ---
    public Transform[] initialLocationPoints; // �������I����
    public static DataSimulator Instance;

    // --- �����ƾ� ---
    [HideInInspector] // ���b Inspector ��ܡA�� Initialize ��R
    public List<LocationData> allLocations = new List<LocationData>();
    private float timer;

    // --- �Ѽƽվ�� (�i�b Inspector ���t�m) ---
    public float updateInterval = 2.0f;           // �C�j 2 ���s�ƾ�
    public int lowPopulationThreshold = 20;       // �H�֪��w�q
    public int minPopulation = 5;                 // �H�ƤU��
    public int maxPopulation = 40;                // �H�ƤW��
    public int populationChangeRange = 5;        // �C����s�H���ܰʪ��d�� (+/- 10)


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // �T�O����b���������ɤ��|�Q�P�� (�i��)
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Initialize(); // �C���ҰʮɩI�s��l��
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
            // �T�O MissionManager �s�b�~�I�s
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
            // �H���i��
            int change = Random.Range(-populationChangeRange, populationChangeRange + 1);
            location.currentPopulation += change;

            // ����H�Ʀb�]�w�d��
            location.currentPopulation = Mathf.Clamp(location.currentPopulation, minPopulation, maxPopulation);

            // ��X���A�A��K�ո�
            string status = (location.currentPopulation < lowPopulationThreshold) ? "LOW POP" : "NORMAL";
            Debug.Log($"[Simulator] ID: {location.locationID}, Pop: {location.currentPopulation}, Status: {status}");
        }
    }
}