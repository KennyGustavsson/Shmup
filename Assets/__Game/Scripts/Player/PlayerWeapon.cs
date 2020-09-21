using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour{
	public List<Weapon> weapons;
	
	[Serializable]
	public struct Weapon{
		public int weapon;
		public float fireRate;
		public bool multiFire;
	}
	
	public int weaponIndex = 0;
	public bool onCoolDown;
	public bool shoot;

	[Header("MultiFire")]
	public int extraShotCount = 2;
	public float spreadAngle = 5f;
	
	private Transform _transform;
	
	private void Awake(){
		_transform = transform;
		UpdateWeaponDisplay();
	}

	private void OnEnable(){
		GameEvents.current.ONPickUpWeapon += PickUpWeapon;
	}

	private void Update(){
		if(!shoot || onCoolDown || GameManager.Instance.isPaused) return;
		Shoot();
	}

	private void Shoot(){
		ObjectPool.Instance.ObjectPooler(weapons[weaponIndex].weapon,
			_transform.position + _transform.forward * 3, _transform.rotation);

		if (weapons[weaponIndex].multiFire){
			if (extraShotCount > 1){
				int count = 0;
				int i = 1;
				while (extraShotCount > count){
					i *= -1;

					ObjectPool.Instance.ObjectPooler(weapons[weaponIndex].weapon,
						_transform.position + _transform.forward * 3,
						Quaternion.identity * Quaternion.Euler(0,1 * spreadAngle * i,0));

					count++;

					if (count % 2 != 0) continue;
					if (i < 0) i += -1;
					else i += 1;
				}
			}
		}

		GameEvents.current.UpdateWeaponCooldown(weapons[weaponIndex].fireRate);
		StartCoroutine(FireRate());
	}

	public void SwitchWeapon(Vector2 scrollDelta){
		if(GameManager.Instance.isPaused) return;
		
		if (scrollDelta.y > 0){
			weaponIndex += 1;
			if (weaponIndex > weapons.Count - 1){
				weaponIndex = 0;
			}
			UpdateWeaponDisplay();
		}
		else if (scrollDelta.y < 0){
			weaponIndex -= 1;
			if (weaponIndex < 0){
				weaponIndex = weapons.Count - 1;
			}
			UpdateWeaponDisplay();
		}
	}
	
	private void OnDisable(){
		StopCoroutine(FireRate());
		onCoolDown = false;
		GameEvents.current.ONPickUpWeapon -= PickUpWeapon;
	}

	private IEnumerator FireRate(){
		onCoolDown = true;
		yield return new WaitForSeconds(weapons[weaponIndex].fireRate);
		onCoolDown = false;
	}

	private void UpdateWeaponDisplay(){
		switch (weapons.Count){
			case 0:
				break;
			case 1:
				UIManager.instance.UpdateWeaponDisplay(weapons[weaponIndex].weapon);
				break;
			case 2:
				int tempNext = weaponIndex + 1;
				if (tempNext > (weapons.Count - 1)) tempNext = 0;
				
				UIManager.instance.UpdateWeaponDisplay(weapons[weaponIndex].weapon,
					weapons[tempNext].weapon);
				break;
			default:
				int tempPrev2 = weaponIndex - 1;
				int tempNext2 = weaponIndex + 1;
				if (tempPrev2 < 0) tempPrev2 = weapons.Count - 1;
				if (tempNext2 > (weapons.Count - 1)) tempNext2 = 0;
				
				UIManager.instance.UpdateWeaponDisplay(weapons[weaponIndex].weapon,
					weapons[tempNext2].weapon, weapons[tempPrev2].weapon);
				break;
		}
	}
	
	private void PickUpWeapon(int id, float fireRate, bool multiFire){
		if (weapons.Any(weapon => weapon.weapon == id) &&
		    weapons.Any(weapon => Math.Abs(weapon.fireRate - fireRate) < 0.01) &&
		    weapons.Any(weapon => weapon.multiFire == multiFire)){
			return;
		}

		var newWeapon = new Weapon{weapon = id, fireRate = fireRate, multiFire = multiFire};
		weapons.Add(newWeapon);
		weaponIndex = weapons.Count - 1;
		
		UpdateWeaponDisplay();
	}
}