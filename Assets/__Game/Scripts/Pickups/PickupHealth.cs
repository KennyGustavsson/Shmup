using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickupHealth : MonoBehaviour{
	public int healthAmount;
	public int whoToHealID;
	public LayerMask triggerLayer;

	public AudioClip[] pickUpSound;
	private AudioSource _audioSource;

	private void Awake(){
		_audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider other){
		if (((1<<other.gameObject.layer) &triggerLayer) != 0){
			PickUpHealth(whoToHealID, healthAmount);
		}
	}

	private void PickUpHealth(int id, int heal){
		GameEvents.current.PickUpHealth(id, heal);
		PickupSpawner.Instance.EnquePickup(gameObject);
		
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