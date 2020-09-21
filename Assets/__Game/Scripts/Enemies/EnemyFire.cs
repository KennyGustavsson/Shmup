using System.Collections;
using UnityEngine;

public class EnemyFire : MonoBehaviour{
	public int bulletID = 1;
	public float fireRate = 1;

	private Transform _transform;

	private void Awake(){
		_transform = transform;
	}

	private void OnEnable(){
		StartCoroutine(Shooting());
	}

	private IEnumerator Shooting(){
		while (true){
			yield return new WaitForSeconds(fireRate);
			ObjectPool.Instance.ObjectPooler(bulletID, _transform.position + _transform.forward * 3, _transform.rotation);
		}
	}

	private void OnDisable(){
		StopCoroutine(Shooting());
	}
}