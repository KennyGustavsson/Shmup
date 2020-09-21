using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicIntesity : MonoBehaviour{
	[Header("Audio")] 
	public float pitchChangeMultiplier = 0.001f;
	public float maxPitch = 1.3f;
	
	private float _audioPitch = 1f;
	private AudioSource _audioSource;

	private void Awake(){
		_audioSource = GetComponent<AudioSource>();
	}

	private void Update(){
		if (!(_audioPitch < maxPitch)) return;
		_audioPitch += Time.deltaTime * pitchChangeMultiplier;
		_audioSource.pitch = _audioPitch;
	}

	public void StopMusic(){
		_audioSource.Stop();
	}
}