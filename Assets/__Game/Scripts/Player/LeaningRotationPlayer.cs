using UnityEngine;

public class LeaningRotationPlayer : MonoBehaviour{
	public float leaningTime = 30f;
	public float straightenUpTimeMultiplier = 1.3f;
	[Range(0f,1f)]public float leaningMax = 0.1f;
	
	private Transform _transform;
	private PlayerMovement _playerMovement;
	
	private void Awake(){
		_transform = transform;
		_playerMovement = GetComponentInParent<PlayerMovement>();
	}

	private void Update(){
		Quaternion rotation = _transform.rotation;
		if (_playerMovement.inputMove.x != 0){
			if (_playerMovement.inputMove.x < 0 && rotation.z < leaningMax){
				rotation.z += Quaternion.Euler(new Vector3(0f, 0f, Time.deltaTime * leaningTime)).z;
			}

			if (_playerMovement.inputMove.x > 0 && rotation.z > -leaningMax){
				rotation.z -= Quaternion.Euler(new Vector3(0f, 0f, Time.deltaTime * leaningTime)).z;
			}
		}
		else{
			if (rotation.z > 0){
				rotation.z -= Quaternion.Euler(new Vector3(0f, 0f,
					Time.deltaTime * leaningTime * straightenUpTimeMultiplier)).z;
			}
            
			if (rotation.z < 0){
				rotation.z += Quaternion.Euler(new Vector3(0f, 0f,
					Time.deltaTime * leaningTime * straightenUpTimeMultiplier)).z;
			}
		}
		_transform.rotation = rotation;
	}
}