using UnityEngine;

public class FreezeCamera : MonoBehaviour{
    public bool freezeCamera = false;

    private Vector3 _freezePosition;
    private Transform _transform;
    
    private void Awake(){
        _transform = transform;
    }

    public void Freeze(){
        freezeCamera = true;
        _freezePosition = _transform.position;
    }
    
    private void LateUpdate(){
        if(!freezeCamera) return;

        _transform.position = _freezePosition;
    }
}