using UnityEngine;

public abstract class WeaponTemplete : ScriptableObject, IWeapon{
	public abstract void Shoot(Transform transform);
	public abstract int Hit(Vector3 position, Collider collider);
}