using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour{
	public static ScreenShake instance;
	
	public bool disableScreenShake = false;

	private Transform _transform;
	private Vector3 _smoothDamp;

	private void Awake(){
		if (instance) Destroy(gameObject);
		else instance = this;

		_transform = transform;
	}

	public void StopShake(){
		StopAllCoroutines();
	}
	
	public void Shake(float duration, float magnitude){
		StartCoroutine(CameraShake(duration, magnitude));
	}

	// Camera screen shake effect
	private IEnumerator CameraShake(float duration, float magnitude) {
		if (!disableScreenShake || GameManager.Instance.playerHealth <= 0) {
			float elapsedTime = 0f;

			while (elapsedTime < duration || GameManager.Instance.playerHealth <= 0) {
				if (!GameManager.Instance.isPaused){
					float x = Random.Range(-1f, 1f) * magnitude;
					float y = Random.Range(-1f, 1f) * magnitude;

					var position = _transform.position;
					position = new Vector3(position.x + x,
						position.y + y,
						position.z);
					_transform.position = position;

					elapsedTime += Time.deltaTime;
				}
				yield return new WaitForSeconds(0);
			}
		}
		else yield return new WaitForSeconds(0);
	}
}