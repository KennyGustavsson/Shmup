using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour{
	public static UIManager instance;
	
	public AudioMixer audioMixer;
	public Slider slider;
	public Options options;
	public Text inputNameText;

	[Header("Objects")]
	public ShieldDisplay shieldDisplay;
	public GameObject currentWeaponSlot;
	public GameObject prevWeaponSlot;
	public GameObject nextWeaponSlot;
	public GameObject shieldIcon;
	public GameObject weaponSwapIcon;
	public InputField inputField;

	[Header("Event System")] 
	public EventSystem eventSystem;
	
	private Text _currentWeaponText;
	private Text _prevWeaponText;
	private Text _nextWeaponText;
	
	private void Awake(){
		if (instance) Destroy(gameObject);
		else instance = this;

		slider.value = options.masterVolume;

		if (currentWeaponSlot == null) return;
		_currentWeaponText = currentWeaponSlot.GetComponentInChildren<Text>();
		_prevWeaponText = prevWeaponSlot.GetComponentInChildren<Text>();
		_nextWeaponText = nextWeaponSlot.GetComponentInChildren<Text>();
	}

	private void Start(){
		audioMixer.SetFloat("Master", options.masterVolume);
	}

	public void Resume(){
		Time.timeScale = 1;
		GameManager.Instance.Pause();
	}

	public void VolumeSlider(float volume){
		audioMixer.SetFloat("Master", volume);
		options.masterVolume = volume;
	}
	
	public void Exit(){
		Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}

	public void StartGame(){
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Time.timeScale = 1;
		SceneManager.LoadScene(1);
	}

	public void UpdateShieldDisplay(float currentCoolDown){
		shieldDisplay.UpdateShieldCoolDown(currentCoolDown);
		StartCoroutine(HideShieldIcon(currentCoolDown));
	}

	private IEnumerator HideShieldIcon(float coolDown){
		shieldIcon.SetActive(false);
		yield return new WaitForSeconds(coolDown);
		shieldIcon.SetActive(true);
	}
	
	public void ActivateShield(){
		shieldDisplay.ActivateShield();
	}
	
	public void UpdateWeaponDisplay(int currentID){
		nextWeaponSlot.SetActive(false);
		prevWeaponSlot.SetActive(false);
		weaponSwapIcon.SetActive(false);
		_currentWeaponText.text = ObjectPool.Instance.IDToName(currentID);
	}

	public void UpdateWeaponDisplay(int currentID, int nextID){
		nextWeaponSlot.SetActive(true);
		prevWeaponSlot.SetActive(false);
		weaponSwapIcon.SetActive(true);
		_currentWeaponText.text = ObjectPool.Instance.IDToName(currentID);
		_nextWeaponText.text = ObjectPool.Instance.IDToName(nextID);
	}

	public void UpdateWeaponDisplay(int currentID, int nextID, int prevID){
		nextWeaponSlot.SetActive(true);
		prevWeaponSlot.SetActive(true);
		weaponSwapIcon.SetActive(true);
		_currentWeaponText.text = ObjectPool.Instance.IDToName(currentID);
		_nextWeaponText.text = ObjectPool.Instance.IDToName(nextID);
		_prevWeaponText.text = ObjectPool.Instance.IDToName(prevID);
	}
	
	public void ExitToDesktop(){
		Time.timeScale = 1;
		Application.Quit();
	}

	public void ForceUpperCase(){
		if(inputField.text == "") return;
		inputField.text = inputField.text.ToUpper();
	}
	
	public void ConfirmName(){
		GameManager.Instance.EnterNameConfirm(inputNameText.text);
	}

	public void SetCurrentlySelected(GameObject obj){
		var button = obj.GetComponent<Button>();
		eventSystem.SetSelectedGameObject(obj);
		button.OnSelect(null);
		
	}
}