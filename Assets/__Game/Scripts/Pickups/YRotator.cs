using UnityEngine;

public class YRotator : MonoBehaviour{
	public float speed = 1f;
	
	private Transform _transform;
	private float _yRotation = 0;
	
	private void Awake(){
		_transform = transform;
	}

	private void Update(){
		_yRotation += Time.deltaTime * speed;
		_transform.rotation = Quaternion.Euler(0,_yRotation,0);
	}
}