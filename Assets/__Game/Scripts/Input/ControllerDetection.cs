using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerDetection : MonoBehaviour{
    public static ControllerDetection instance;
    public bool usingController = false;
    
    private PlayerInput _input;

    private void Awake(){
        if (!instance) instance = this;
        else Destroy(gameObject);
        
        _input = new PlayerInput();
        _input.Player.ControllerDetect.performed += ctx => ControllerDetected();
        _input.Player.KeyboardDetect.performed += ctx => KeyboardDetected();
    }

    private void OnEnable(){
        _input.Player.ControllerDetect.Enable();
        _input.Player.KeyboardDetect.Enable();
    }

    private void OnDisable(){
        _input.Player.ControllerDetect.Disable();
        _input.Player.KeyboardDetect.Disable();
    }

    private void ControllerDetected(){
        if (SceneManager.GetActiveScene().buildIndex == 1){
            if (GameManager.Instance.playerHealth <= 0){
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else{
                usingController = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;  
            }
        }
        else{
            usingController = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;  
        }
    }

    private void KeyboardDetected(){
        usingController = false;

        if (SceneManager.GetActiveScene().buildIndex == 0){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        
        if (!GameManager.Instance.isPaused) return;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}