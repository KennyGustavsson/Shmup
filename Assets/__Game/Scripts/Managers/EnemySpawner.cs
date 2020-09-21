using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour{
    public static EnemySpawner Instance;
    
    public Vector3 spawnLocation;
    public Vector3 despawnLocation;
    public float maxSpawnOffset;
    public float spawnInterval;
    public AnimationCurve difficultyCurve;
    public GameObject[] enemies;

    private float _timer;
    private float _curveTimer;
    private float _spawnTime;
    private Queue<GameObject> _enemyQueue;

    private void Awake(){
        if (Instance == null) Instance = this;
        else{
            Destroy(gameObject);
            return;
        }

        _enemyQueue = new Queue<GameObject>(enemies.Length);
        foreach (var enemy in enemies){
            var enemyObject = Instantiate(enemy, transform, true);
            enemyObject.SetActive(false);
            _enemyQueue.Enqueue(enemyObject);
        }
    }

    private void Update(){
        spawnInterval = difficultyCurve.Evaluate(_curveTimer);

        float deltaTime = Time.deltaTime;
        _timer += deltaTime;
        _curveTimer += deltaTime;
        if (!(_timer > spawnInterval)) return;
        _timer = 0;
        SpawnEnemy();
    }

    public void EnqueEnemy(GameObject enemy){
        _enemyQueue.Enqueue(enemy);
    }
    
    private void SpawnEnemy(){
        if(_enemyQueue.Count <= 0) return;
        GameObject enemy = _enemyQueue.Dequeue();
        enemy.SetActive(false);
        
        enemy.transform.rotation = Quaternion.Euler(0,180,0);
        enemy.transform.position = new Vector3(Random.Range(-maxSpawnOffset, maxSpawnOffset) + spawnLocation.x,
            spawnLocation.y, spawnLocation.z);
        enemy.SetActive(true);
    }
}