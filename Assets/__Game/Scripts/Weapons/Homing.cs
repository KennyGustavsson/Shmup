using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Homing : MonoBehaviour{
	public HomingWeapon weapon;

	private Transform _transform;
	private Rigidbody _rigidbody;
	private Transform _target;
	private bool _targetLocked;
	private bool _canSearch = true;
	private AudioSource _audioSource;

	private void Awake(){
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
	}

	private void OnEnable(){
		_rigidbody.velocity = Vector3.zero;
		_transform.rotation = Quaternion.Euler(0,0,0);
		_target = default;
		_targetLocked = false;
		ObjectPool.Instance.ObjectPooler(weapon.shotEffectID, _transform.position, _transform.rotation);
		SearchForTarget();
		
		if (weapon.shootSound.Length <= 0) return;
		int i = Random.Range(0, weapon.shootSound.Length - 1);
		_audioSource.PlayOneShot(weapon.shootSound[i]);
	}

	private IEnumerator SearchDelay(){
		_canSearch = false;
		yield return new WaitForSeconds(weapon.searchNextTargetDelay);
		_canSearch = true;
	}
	
	private void Update(){
		_rigidbody.velocity = transform.forward * weapon.speed;
		
		if (!_targetLocked || !_target.gameObject.activeInHierarchy){
			_target = default;
			_targetLocked = false;
			if(!_canSearch) return;
			SearchForTarget();
		}
		else{
			Vector3 direction = _target.position - _transform.position;
			Vector3 lookRotation = Vector3.RotateTowards(_transform.forward, direction,
				weapon.rotationSpeed * Time.deltaTime, 0.0f);
			_transform.rotation = Quaternion.LookRotation(lookRotation);
		}
	}

	private void SearchForTarget(){
		//Scan for target
		var colliders = Physics.SphereCastAll(_transform.position, weapon.homingRadius,
			_transform.forward, Mathf.Infinity, weapon.enemyLayer);
		if (colliders.Length > 0){
			int i, j;
			int n = colliders.Length;

			for (j = 0; j < n; j++){
				for (i=j; i>0 && colliders[i].distance < colliders[i-1].distance; i--){
					var temp = colliders[i];
					colliders[i] = colliders[i - 1];
					colliders[i - 1] = colliders[i];
				}
			}

			for (int k = 0; k < colliders.Length; k++){
				if (colliders[k].transform.gameObject.activeInHierarchy && colliders[k].transform.position.z > 0 && colliders[k].transform.gameObject.layer == 9){
					_target = colliders[k].transform;
					_targetLocked = true;
					break;
				}
			}
			
			StartCoroutine(SearchDelay());
		}
	}
	
	private void OnTriggerEnter(Collider other){
		if(((1<<other.gameObject.layer) & weapon.enemyLayer) == 0) return;
		ObjectPool.Instance.ObjectPooler(weapon.hitEffectID, _transform.position, _transform.rotation);
		
		weapon.Hit(other.transform.position, other);
		_transform.position = new Vector3(0,0, -20);
		_transform.rotation = Quaternion.Euler(0,0,0);
		_target = default;
		_rigidbody.velocity = Vector3.zero;
		_targetLocked = false;
		gameObject.SetActive(false);
	}
}