using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour{
	public static PickupSpawner Instance;
	
	public Vector3 spawnLocation;
	public Vector3 deSpawnLocation;
	public float maxSpawnOffset;
	public float spawnInterval;
	public GameObject[] pickups;

	private float _timer;
	private Queue<GameObject> _pickupQueue;

	private void Awake(){
		if (Instance) Destroy(gameObject);
		else Instance = this;
		
		_pickupQueue = new Queue<GameObject>(pickups.Length);
		foreach (var pickup in pickups){
			var pickupObject = Instantiate(pickup, transform, true);
			pickupObject.SetActive(false);
			_pickupQueue.Enqueue(pickupObject);
		}
	}

	private void Update(){
		_timer += Time.deltaTime;
		if (_timer > spawnInterval){
			_timer = 0;
			SpawnPickup();
		}
	}

	public void EnquePickup(GameObject pickup){
		_pickupQueue.Enqueue(pickup);
	}

	private void SpawnPickup(){
		if (_pickupQueue.Count == 0) return;
		GameObject pickup = _pickupQueue.Dequeue();
		pickup.SetActive(false);
		
		pickup.transform.rotation = Quaternion.Euler(0,180,0);
		pickup.transform.position = new Vector3(Random.Range(-maxSpawnOffset, maxSpawnOffset) + spawnLocation.x,
			spawnLocation.y, spawnLocation.z);
		pickup.SetActive(true);
	}
}
