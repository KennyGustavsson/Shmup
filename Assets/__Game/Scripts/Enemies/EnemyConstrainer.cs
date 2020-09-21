using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyConstrainer : MonoBehaviour{
    private Transform _transform;
    private Health _health;
    
    private void Awake(){
        _transform = transform;
        _health = GetComponent<Health>();
    }

    private void Update(){
        if(_health.isEnqued) return;
        
        // Z
        if (_transform.position.z < EnemySpawner.Instance.despawnLocation.z){
            if(_health.isEnqued) return;
            _health.isEnqued = true;
            EnemySpawner.Instance.EnqueEnemy(gameObject);
            gameObject.SetActive(false);
        }
        
        // X
        else if(_transform.position.x > EnemySpawner.Instance.despawnLocation.x ||
                _transform.position.x < -EnemySpawner.Instance.despawnLocation.x){
            if(_health.isEnqued) return;
            _health.isEnqued = true;
            EnemySpawner.Instance.EnqueEnemy(gameObject);
            gameObject.SetActive(false);
        }
        
        // Y
        else if(_transform.position.y > EnemySpawner.Instance.despawnLocation.y ||
                _transform.position.y < -EnemySpawner.Instance.despawnLocation.y){
            if(_health.isEnqued) return;
            _health.isEnqued = true;
            EnemySpawner.Instance.EnqueEnemy(gameObject);
            gameObject.SetActive(false);
        }
    }
}