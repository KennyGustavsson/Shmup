using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForwardMovement : MonoBehaviour{
	public float speed;
	public float scalingSpeed = 1f;
	
	private Transform _transform;
	private Rigidbody _rigidbody;
	private Vector3 _originalScale;
	
	private void Awake(){
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody>();
		_originalScale = _transform.localScale;
	}

	private void OnEnable(){
		_transform.localScale = Vector3.zero;
	}

	private void FixedUpdate(){
		transform.localScale = Vector3.Lerp (transform.localScale, _originalScale, scalingSpeed * Time.deltaTime);
		
		var position = _transform.position;
		_rigidbody.MovePosition(position + (_transform.forward * (speed * Time.deltaTime)));
	}
}