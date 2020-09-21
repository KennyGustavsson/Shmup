using UnityEngine;

[CreateAssetMenu(fileName = "New Homing Weapon", menuName = "ScriptableObjects/Homing Weapon")]
public class HomingWeapon : WeaponTemplete{
    [Header("Homing Projectile")]
    public int damage;
    public float speed;
    public float rotationSpeed;
    public float homingRadius;
    public float searchNextTargetDelay;
    public LayerMask enemyLayer;
    
    [Header("ObjectPool")]
    public int shotEffectID;
    public int hitEffectID;

    [Header("Sound")] 
    public AudioClip[] shootSound;

    public override void Shoot(Transform transform){
        var position = transform.position;
        var rotation = transform.rotation;
        ObjectPool.Instance.ObjectPooler(shotEffectID, position, rotation);
    }

    public override int Hit(Vector3 position, Collider collider){
        int remainingDamage = collider.GetComponent<Health>().Damage(damage);
        ObjectPool.Instance.ObjectPooler(hitEffectID, position, Quaternion.identity);
        return remainingDamage;
    }
}