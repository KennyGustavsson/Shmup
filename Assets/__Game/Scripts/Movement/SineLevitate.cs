using UnityEngine;

public class SineLevitate : MonoBehaviour{
	[Header("Rotation")]
	public Vector3 sineRotationStrength = Vector3.one;
	public Vector3 sineRotationRadius = Vector3.one;
	
	[Header("Position")]
	public Vector3 sinePositionStrength = Vector3.one;
	public Vector3 sinePositionRadius = Vector3.one;
	
	private Transform _transform;
	private Vector3 _startRot;
	private Vector3 _startPos;

	private void Awake(){
		_transform = transform;
		_startRot = _transform.rotation.eulerAngles;
		_startPos = _transform.position;
	}

	private void FixedUpdate(){
		float time = Time.time;
		
		_transform.rotation = Quaternion.Euler(_startRot + new Vector3(Mathf.Sin(time * sineRotationStrength.x) * sineRotationRadius.x,
			Mathf.Sin(time * sineRotationStrength.y) * sineRotationRadius.y,
			Mathf.Sin(time * sineRotationStrength.z)* sineRotationRadius.z));

		_transform.position = _startPos + new Vector3(Mathf.Sin(time * sinePositionStrength.x) * sinePositionRadius.x,
			Mathf.Sin(time * sinePositionStrength.y) * sinePositionRadius.y,
			Mathf.Sin(time * sinePositionStrength.z)) * sinePositionRadius.z;
	}
}