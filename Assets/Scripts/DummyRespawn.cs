using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [System.Serializable]
    public class DummySpawnInfo
    {
        public GameObject dummyPrefab; // De prefab van de dummy
        public Vector3 spawnLocation; // De spawnlocatie voor de dummy
    }

    public List<DummySpawnInfo> dummySpawnInfos; // Lijst van dummy prefab en spawnlocatie paren

    void Start()
    {
        // Spawn de initiële dummies
        foreach (var spawnInfo in dummySpawnInfos)
        {
            GameObject initialDummy = Instantiate(spawnInfo.dummyPrefab, spawnInfo.spawnLocation, spawnInfo.dummyPrefab.transform.rotation);
        }
    }
}
