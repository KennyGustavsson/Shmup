using UnityEngine;

public interface IWeapon{
    void Shoot(Transform transform);
    int Hit(Vector3 position, Collider collider);
}