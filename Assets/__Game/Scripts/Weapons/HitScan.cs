using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HitScan : MonoBehaviour{
	public HitScanWeapon weapon;

	private Transform _transform = default;
	private LineRenderer _lineRenderer = default;
	private AudioSource _audioSource;
	
	private void Awake(){
		_transform = transform;
		_lineRenderer = GetComponent<LineRenderer>();
		_audioSource = GetComponent<AudioSource>();
		_lineRenderer.enabled = false;
	}

	private void OnEnable(){
		weapon.Shoot(_transform);
		StartCoroutine(WeaponEffect());
	}
	
	private IEnumerator WeaponEffect(){
		_lineRenderer.enabled = true;
		
		if (weapon.shootSound.Length > 0){
			int i = Random.Range(0, weapon.shootSound.Length - 1);
			_audioSource.PlayOneShot(weapon.shootSound[i]);
		}
		
		yield return new WaitForSeconds(0.1f);
		_lineRenderer.enabled = false;
	}
}