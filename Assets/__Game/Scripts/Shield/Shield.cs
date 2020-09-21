using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Health))]
public class Shield : MonoBehaviour{
	public ScriptableObject scriptableObject;
	public GameObject shieldObj;
	
	[Header("Audio")]
	public AudioClip activateShield;
	public AudioMixerGroup mixerGroup;
	public float audioVolume = 0.15f;
	
	[NonSerialized] public bool canShield = true;
	
	private IShield _shield;
	private AudioSource _audioSource;

	private void Awake(){
		if (scriptableObject is IShield shield) _shield = shield;
		_shield.SetVariables(GetComponent<Health>(), shieldObj);

		_audioSource = gameObject.AddComponent<AudioSource>();
		_audioSource.outputAudioMixerGroup = mixerGroup;
		_audioSource.volume = audioVolume;
	}

	private void OnEnable(){
		GameEvents.current.ONActivateShield += OnShieldActivate;
	}

	private void OnDisable(){
		GameEvents.current.ONActivateShield -= OnShieldActivate;
	}

	private void OnShieldActivate(){
		if(!canShield || GameManager.Instance.isPaused || GameManager.Instance.playerHealth <= 0) return;
 		_audioSource.PlayOneShot(activateShield);
		StartCoroutine(_shield.Shield(this, shieldObj));
	}
}