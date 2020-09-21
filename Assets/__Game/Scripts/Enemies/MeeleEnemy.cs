using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(LineRenderer))]
public class MeeleEnemy : MonoBehaviour{
	[Header("Damage")]
	public int damage;
	public int hitEffect;
	public LayerMask damageableLayer;
	
	[Header("Audio")]
	public AudioClip warningSound;
	public AudioMixerGroup audioMixer;
	public float audioVolume = 0.2f;
	
	private Transform _transform;
	private LineRenderer _lineRenderer;
	private AudioSource _audioSource;
	
	private void Awake(){
		_transform = transform;
		_lineRenderer = GetComponent<LineRenderer>();
		_audioSource = gameObject.AddComponent<AudioSource>();
		_audioSource.outputAudioMixerGroup = audioMixer;
		_audioSource.volume = audioVolume;
	}

	private void OnEnable(){
		_lineRenderer.enabled = true;
		_audioSource.PlayOneShot(warningSound);
	}

	private void OnDisable(){
		_lineRenderer.enabled = false;
	}

	private void OnTriggerEnter(Collider other){
		if(((1<<other.gameObject.layer) & damageableLayer) == 0) return;
		var transform1 = _transform;
		ObjectPool.Instance.ObjectPooler(hitEffect, transform1.position, transform1.rotation);
		other.transform.GetComponent<Health>().Damage(damage);
	}
}