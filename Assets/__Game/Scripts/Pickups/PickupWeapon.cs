using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class PickupWeapon : MonoBehaviour{
	public int weaponID;
	public float fireRate;
	public bool multiFire;
	public LayerMask triggerLayer;
	
	[Header("Audio")]
	public AudioClip[] pickUpSound;

	[NonSerialized] public bool isPickedUp;
	
	private AudioSource _audioSource;

	private void Awake(){
		_audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider other){
		if (((1<<other.gameObject.layer) &triggerLayer) != 0){
			PickUpWeapon(weaponID);
		}
	}

	private void PickUpWeapon(int id){
		isPickedUp = true;
		GameEvents.current.PickUpWeapon(id, fireRate, multiFire);
		if (pickUpSound.Length > 0){
			int i = Random.Range(0, pickUpSound.Length - 1);
			_audioSource.PlayOneShot(pickUpSound[i]);
			StartCoroutine(Deactivate(pickUpSound[i].length));
		}
		else{
			gameObject.SetActive(false);
		}
	}
	
	private IEnumerator Deactivate(float time){
		yield return new WaitForSeconds(time);
		gameObject.SetActive(false);
	}
}