using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "ScriptableObjects/Shield")]
public class SOShield : ScriptableObject, IShield{
	public float shieldTime = 3f;
	public float cooldownTime = 10f;
	public float shieldNearEndIndicator = 0.5f;
	public float flickerTime = 0.1f;
	
	private GameObject _shieldObj;
	private Health _health;

	public void SetVariables(Health health, GameObject shieldObj){
		_health = health;
		_shieldObj = shieldObj;
		_shieldObj.SetActive(false);
	}

	public IEnumerator Shield(Shield shield, GameObject shieldObject){
		if (GameManager.Instance.isPaused){
			yield return false;
		}
		else{
			// Activate
			shield.canShield = false;
			_health.isShielded = true;
			_shieldObj.SetActive(true);
			UIManager.instance.ActivateShield();
			yield return new WaitForSeconds(shieldTime - shieldNearEndIndicator);
	
			// Near End
			float time = 0;
			while (time < shieldNearEndIndicator){
				shieldObject.SetActive(!shieldObject.activeSelf);
				yield return new WaitForSeconds(flickerTime);
				time += flickerTime;
			}
			
			// Cooldown
			_shieldObj.SetActive(false);
			_health.isShielded = false;
			UIManager.instance.UpdateShieldDisplay(cooldownTime);
			yield return new WaitForSeconds(cooldownTime);
			shield.canShield = true;
		}
	}
}
