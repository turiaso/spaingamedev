using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Prefab_Policia;
    public GameObject Prefab_Preso;
    public GameObject SpawnPoints;
    public Transform PresosPadre;

    private float timer, timerMax;
    void Start()
    {
        timerMax = 10f;
        timer = 0;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            GenerarPreso();
            timer -= timerMax;
        }
    }

    private void GenerarPreso()
    {
        //Debug.Log("Generando Preso");
        int sp = Random.Range(0, SpawnPoints.transform.childCount);
        Vector3 spawnPosition = SpawnPoints.transform.GetChild(sp).position;
        spawnPosition.y += 0.5f;
        spawnPosition.x += Random.Range(-5, 6);
        spawnPosition.z += Random.Range(-5, 6);
        Instantiate(Prefab_Preso, spawnPosition, Quaternion.identity, PresosPadre);
    }
}
