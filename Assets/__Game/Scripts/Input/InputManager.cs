using UnityEngine;

public class InputManager : MonoBehaviour{
	public PlayerMovement playerMovement;
	public PlayerWeapon playerWeapon;
	public GameManager gameManager;
	
	private PlayerInput _input;
	private void Awake(){
		_input = new PlayerInput();

		// Movement
		_input.Player.Vertical.performed += ctx => playerMovement.inputMove.y = ctx.ReadValue<float>();
		_input.Player.Vertical.canceled += ctx => playerMovement.inputMove.y = 0f;
		_input.Player.Horizontal.performed += ctx => playerMovement.inputMove.x = ctx.ReadValue<float>();
		_input.Player.Horizontal.canceled += ctx => playerMovement.inputMove.x = 0f;

		// Actions
		_input.Player.Shoot.performed += ctx => playerWeapon.shoot = true;
		_input.Player.Shoot.canceled += ctx => playerWeapon.shoot = false;
		_input.Player.Shield.performed += ctx => GameEvents.current.ActivateShield();
		_input.Player.SwitchWeapon.performed += ctx => playerWeapon.SwitchWeapon(ctx.ReadValue<Vector2>());
		_input.Player.Pause.performed += ctx => gameManager.Pause();
		
		// Detection
		_input.Player.ControllerDetect.performed += ctx => GameEvents.current.UsingController(true);
		_input.Player.KeyboardDetect.performed += ctx => GameEvents.current.UsingController(false);
	}

	private void OnEnable(){
		_input.Player.Vertical.Enable();
		_input.Player.Horizontal.Enable();
		_input.Player.Shoot.Enable();
		_input.Player.Shield.Enable();
		_input.Player.SwitchWeapon.Enable();
		_input.Player.Pause.Enable();
		_input.Player.ControllerDetect.Enable();
		_input.Player.KeyboardDetect.Enable();
	}

	private void OnDisable(){
		_input.Player.Vertical.Disable();
		_input.Player.Horizontal.Disable();
		_input.Player.Shoot.Disable();
		_input.Player.Shield.Disable();
		_input.Player.SwitchWeapon.Disable();
		_input.Player.Pause.Disable();
		_input.Player.ControllerDetect.Disable();
		_input.Player.KeyboardDetect.Disable();
	}
}