using UnityEngine;

[CreateAssetMenu(fileName = "New HitScan Weapon", menuName = "ScriptableObjects/Hit Scan Weapon")]
public class HitScanWeapon : WeaponTemplete{
    [Header("Hit Scan Weapon")]
    public int damage;
    public float rayLength;
    public LayerMask damageAbleLayer;
    
    [Header("ObjectPool")]
    public int idShotEffect;
    public int idHitEffect;

    [Header("Sound")] 
    public AudioClip[] shootSound;

    [System.Serializable]
    public enum Type{
        SingleTarget,
        MultipleTargetDropOff,
        AllTargets
    }
    public Type hitScanType;

    private RaycastHit _hitInfo;
    private RaycastHit[] _hits;

    public override void Shoot(Transform transform){
        ObjectPool.Instance.ObjectPooler(idShotEffect, transform.position, transform.rotation);
        
        switch ((int)hitScanType){
            case 0:
                if(!Physics.Raycast(transform.position, transform.forward, out _hitInfo, rayLength, damageAbleLayer)) return;
                ObjectPool.Instance.ObjectPooler(idHitEffect, _hitInfo.point, transform.rotation);
                Hit(_hitInfo.point, _hitInfo.collider);
                break;
            
            case 1:
                _hits = Physics.RaycastAll(transform.position, transform.forward, rayLength, damageAbleLayer);
                if (_hits.Length == 0) return;

                int i, j;
                int n = _hits.Length;

                for (j = 0; j < n; j++){
                    for (i=j; i>0 && _hits[i].distance < _hits[i-1].distance; i--){
                        var temp = _hits[i];
                        _hits[i] = _hits[i - 1];
                        _hits[i - 1] = _hits[i];
                    }
                }

                int remainingDamage = damage;
                while (remainingDamage > 0){
                    foreach (var hit in _hits){
                        ObjectPool.Instance.ObjectPooler(idHitEffect, hit.point, transform.rotation);
                        remainingDamage = Hit(hit.point, hit.collider);
                    }
                }
                break;
            
            case 2:
                _hits = Physics.RaycastAll(transform.position, transform.forward, rayLength, damageAbleLayer);
                if (_hits.Length > 0){
                    foreach (var hit in _hits){
                        ObjectPool.Instance.ObjectPooler(idHitEffect, hit.point, transform.rotation);
                        Hit(hit.point, hit.collider);
                    }
                }
                break;
        }
    }

    public override int Hit(Vector3 position, Collider collider){
        int remainingDamage = 0;
        remainingDamage = collider.GetComponent<Health>().Damage(damage);
        ObjectPool.Instance.ObjectPooler(idHitEffect, position, Quaternion.identity);
        return remainingDamage;
    }
}