using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour{
	public ProjectileWeapon weapon;
	public int deSpawnParticleID;

	private Transform _transform;
	private Rigidbody _rigidbody;
	private AudioSource _audioSource;
	
	private void Awake(){
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
	}

	private void OnEnable(){
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.AddForce(_transform.forward * weapon.speed, ForceMode.Impulse);
		ObjectPool.Instance.ObjectPooler(weapon.idShotEffect, _transform.position, _transform.rotation);

		if (weapon.shootSound.Length <= 0) return;
		int i = Random.Range(0, weapon.shootSound.Length - 1);
		_audioSource.PlayOneShot(weapon.shootSound[i]);
	}

	private void Update(){
		if (_transform.position.z > GameManager.Instance.playableArea.y){
			ObjectPool.Instance.ObjectPooler(deSpawnParticleID, _transform.position, _transform.rotation);
			_rigidbody.velocity = Vector3.zero;
			gameObject.SetActive(false);
		}
		else if (_transform.position.z < GameManager.Instance.playableArea.z){
			_rigidbody.velocity = Vector3.zero;
			gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other){
		if(((1<<other.gameObject.layer) & weapon.damageableLayer) == 0) return;
		
		ObjectPool.Instance.ObjectPooler(weapon.idHitEffect, _transform.position, _transform.rotation);
		
		if (((1<<other.gameObject.layer) & weapon.damageableLayer) != 0){
			weapon.Hit(other.transform.position, other);
		}
		_transform.position = new Vector3(0,0, -20);
		gameObject.SetActive(false);
	}
}