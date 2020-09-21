using UnityEngine;
using System;

[DefaultExecutionOrder(-120)]
public class GameEvents : MonoBehaviour{
    public static GameEvents current;

    private void Awake(){
        if (current == null) current = this;
        else Destroy(gameObject);
    }

    public event Action<int, int> ONPickUpHealth;
    public void PickUpHealth(int id, int heal){
        ONPickUpHealth?.Invoke(id, heal);
    }

    public event Action<int, float, bool> ONPickUpWeapon;
    public void PickUpWeapon(int id, float fireRate, bool multiFire){
        ONPickUpWeapon?.Invoke(id, fireRate, multiFire);
    }
    
    public event Action ONActivateShield;
    public void ActivateShield(){
        ONActivateShield?.Invoke();
    }

    public event Action<bool> IsUsingController;
    public void UsingController(bool usingController){
        IsUsingController?.Invoke(usingController);
    }

    public event Action<float> WeaponCooldown;
    public void UpdateWeaponCooldown(float cooldown){
        WeaponCooldown?.Invoke(cooldown);
    }
}