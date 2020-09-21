using UnityEngine;

public class PickupConstrainer : MonoBehaviour{
    [System.Serializable]
    public enum PickupTypes{
        Health,
        Weapon
    }
    public PickupTypes pickupType;
    
    private Transform _transform;
    private PickupWeapon _pickupWeapon;
    
    private void Awake(){
        _transform = transform;

        if (pickupType == PickupTypes.Weapon){
            _pickupWeapon = GetComponent<PickupWeapon>();
        }
    }

    private void Update(){
        if (_transform.position.z < PickupSpawner.Instance.deSpawnLocation.z){
            if (pickupType == PickupTypes.Weapon){
                if (_pickupWeapon.isPickedUp) return;
                PickupSpawner.Instance.EnquePickup(gameObject);
                gameObject.SetActive(false);
            }
            else{
                PickupSpawner.Instance.EnquePickup(gameObject);
                gameObject.SetActive(false); 
            }
        }
        else if(_transform.position.x > PickupSpawner.Instance.deSpawnLocation.x ||
                _transform.position.x < -PickupSpawner.Instance.deSpawnLocation.x){
            if (pickupType == PickupTypes.Weapon){
                if (_pickupWeapon.isPickedUp) return;
                PickupSpawner.Instance.EnquePickup(gameObject);
                gameObject.SetActive(false);
            }
            else{
                PickupSpawner.Instance.EnquePickup(gameObject);
                gameObject.SetActive(false); 
            }
        }
        else if(_transform.position.y > PickupSpawner.Instance.deSpawnLocation.y ||
                _transform.position.y < -PickupSpawner.Instance.deSpawnLocation.y){
            if (pickupType == PickupTypes.Weapon){
                if (_pickupWeapon.isPickedUp) return;
                PickupSpawner.Instance.EnquePickup(gameObject);
                gameObject.SetActive(false);
            }
            else{
                PickupSpawner.Instance.EnquePickup(gameObject);
                gameObject.SetActive(false); 
            }
        }
    }
}