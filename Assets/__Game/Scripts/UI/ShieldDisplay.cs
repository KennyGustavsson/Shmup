using UnityEngine;
using UnityEngine.UI;

public class ShieldDisplay : MonoBehaviour{
	public float shieldCoolDownTime;

	private float _shieldTimer = 0f;
	private Slider _slider;

	private void Awake(){
		_slider = GetComponent<Slider>();
	}

	private void Update(){
		if (!(_shieldTimer < shieldCoolDownTime)) return;
		_shieldTimer += Time.deltaTime;
		_slider.value = _shieldTimer;
	}

	public void UpdateShieldCoolDown(float currentCoolDown){
		shieldCoolDownTime = currentCoolDown;
		_shieldTimer = 0;
		_slider.maxValue = shieldCoolDownTime;
	}

	public void ActivateShield(){
		_slider.value = 0;
	}
}