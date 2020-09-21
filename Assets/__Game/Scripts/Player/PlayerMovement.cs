using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour{
    [Header("Basic Movement")]
    public float keyboardMovementSpeed = 10f;
    public float controllerMovementSpeed = 11f;
    [Range(0f,1f)]public float travelSpeed = 0.25f;
    public float zHorizontalSpeedMultiplier = 0.1f;

    [NonSerialized] public Vector2 inputMove;

    private Transform _transform;
    private Rigidbody _rigidbody;
    private float _zHorizontalSpeedMultiplier;

    private void Awake(){
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update(){
        var position = _transform.position;
        PlayerBoundaries(position);
        _zHorizontalSpeedMultiplier = 1 + (position.z * zHorizontalSpeedMultiplier);
    }

    private void FixedUpdate(){
        var position = _transform.position;

        if (ControllerDetection.instance.usingController){
            _rigidbody.MovePosition((position + 
                (Vector3.right * (inputMove.x * controllerMovementSpeed * Time.deltaTime * _zHorizontalSpeedMultiplier))) + 
                (Vector3.forward * (inputMove.y * controllerMovementSpeed * Time.deltaTime)));
        }
        else{
            _rigidbody.MovePosition((position + 
                (Vector3.right * (inputMove.x * keyboardMovementSpeed * Time.deltaTime * _zHorizontalSpeedMultiplier))) + 
                (Vector3.forward * (inputMove.y * keyboardMovementSpeed * Time.deltaTime)));
        }
        
        GameManager.Instance.speedMultiplier = 1 + (position.z * ((1 / GameManager.Instance.playableArea.x) * travelSpeed));
    }

    private void PlayerBoundaries(Vector3 position){
        if (position.x > GameManager.Instance.playableArea.x)
            _transform.position = new Vector3(GameManager.Instance.playableArea.x, 0, _transform.position.z);

        if (position.x < -GameManager.Instance.playableArea.x)
            _transform.position = new Vector3(-GameManager.Instance.playableArea.x, 0, _transform.position.z);

        if (position.z > GameManager.Instance.playableArea.x)
            _transform.position = new Vector3(_transform.position.x, 0, GameManager.Instance.playableArea.x);

        if (position.z < 0)
            _transform.position = new Vector3(_transform.position.x, 0, 0);
    }
}