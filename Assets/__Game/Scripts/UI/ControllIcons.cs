using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ControllIcons : MonoBehaviour{
	public Sprite keyboardImage;
	public Sprite controllerImage;

	private Image _image;
	private bool _onController;

	private void Awake(){
		_image = GetComponent<Image>();
	}

	private void OnEnable(){
		GameEvents.current.IsUsingController += ControllerDetect;
	}

	private void OnDisable(){
		GameEvents.current.IsUsingController -= ControllerDetect;
	}

	private void ControllerDetect(bool usingController){
		if (usingController && !_onController){
			_image.sprite = controllerImage;
			_onController = true;
		}
		else if (!usingController && _onController){
			_onController = false;
			_image.sprite = keyboardImage;
		}
	}
}