using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Weapon", menuName = "ScriptableObjects/Projectile Weapon")]
public class ProjectileWeapon : WeaponTemplete{
    [Header("Projectile")]
    public int damage;
    public float speed;
    public LayerMask damageableLayer = 9;

    [Header("ObjectPool")]
    public int idShotEffect;
    public int idHitEffect;

    [Header("Sound")] 
    public AudioClip[] shootSound;

    public override void Shoot(Transform transform){
        var position = transform.position;
        var rotation = transform.rotation;
        
        ObjectPool.Instance.ObjectPooler(idShotEffect, position, rotation);
    }

    public override int Hit(Vector3 position, Collider collider){
        int remainingDamage = collider.GetComponent<Health>().Damage(damage);
        ObjectPool.Instance.ObjectPooler(idHitEffect, position, Quaternion.identity);
        return remainingDamage;
    }
}