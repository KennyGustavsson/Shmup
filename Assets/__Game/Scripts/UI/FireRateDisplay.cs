using UnityEngine;
using UnityEngine.UI;

public class FireRateDisplay : MonoBehaviour{
	public float shootCoolDownTime;

	private float _shotTimer = 0f;
	private Slider _slider;

	private void Awake(){
		_slider = GetComponent<Slider>();
	}

	private void OnEnable(){
		GameEvents.current.WeaponCooldown += UpdateFireCoolDown;
	}

	private void OnDisable(){
		GameEvents.current.WeaponCooldown -= UpdateFireCoolDown;
	}

	private void Update(){
		if (!(_shotTimer < shootCoolDownTime)) return;
		_shotTimer += Time.deltaTime;
		_slider.value = _shotTimer;
	}

	private void UpdateFireCoolDown(float currentCoolDown){
		shootCoolDownTime = currentCoolDown;
		_shotTimer = 0;
		_slider.maxValue = shootCoolDownTime;
		_slider.value = 0;
	}
}