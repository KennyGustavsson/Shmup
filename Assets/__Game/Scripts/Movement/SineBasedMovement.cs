using UnityEngine;

public class SineBasedMovement : MonoBehaviour{
    public float sinSpeed = 5f;
    public float sinRadius = 5f;
    public float forwardSpeed = 1f;
    public float posLimit = 5f;
    public float scalingSpeed = 1f;
    
    private Vector3 _startPos;
    private Transform _transform;
    private float _zPos = 0f;
    private Vector3 _originalScale;
    
    private void Awake(){
        _transform = transform;
        _originalScale = _transform.localScale;
    }

    private void OnEnable(){
        _zPos = 0f;
        _transform.localScale = Vector3.zero;
        _startPos = _transform.position;
    }
    
    private void FixedUpdate(){
        transform.localScale = Vector3.Lerp (transform.localScale, _originalScale, scalingSpeed * Time.deltaTime);
        
        _transform.position = _startPos + new Vector3(Mathf.Sin(Time.time * sinSpeed) * sinRadius, 0, -_zPos);
        _zPos += 1 * forwardSpeed * GameManager.Instance.speedMultiplier * Time.deltaTime;

        
        if (_transform.position.x > posLimit){
            _transform.position = new Vector3(posLimit, 0, _transform.position.z);
        }

        if (_transform.position.x < -posLimit){
            _transform.position = new Vector3(-posLimit, 0, _transform.position.z);
        }
    }
}