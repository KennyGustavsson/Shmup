using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-110)]
public class GameManager : MonoBehaviour{
	public static GameManager Instance;
	
	[Tooltip("X = side movement and player area, Y = forward, Z = backwards")]
	public Vector3 playableArea = new Vector3(5, 90, -5);
	public float speedMultiplier = 1f;

	[Header("UI")] 
	public GameObject hudObj;
	public GameObject menuObj;
	public int score;
	public Text healthText;
	public Text scoreText;
	
	[Header("Player Stats")]
	public int playerHealth;

	[Header("Game Stats")] 
	public HighScore highScore;
	public bool isPaused;

	[Header("Game Over")] 
	public float gameOverDelay = 1f;
	public GameObject gameOverScreen;
	public FreezeCamera freezeCamera;
	public GameObject gameOverButton;

	[Header("Music")] 
	public MusicIntesity musicIntesity;
	
	private void Awake(){
		if (Instance == null) Instance = this;
		else Destroy(gameObject);

		hudObj.SetActive(true);
		menuObj.SetActive(false);
		gameOverScreen.SetActive(false);
		
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void Pause(){
		if (playerHealth <= 0){
			menuObj.SetActive(false);
			return;
		}
		menuObj.SetActive(!menuObj.activeSelf);

		if (menuObj.activeSelf){
			isPaused = true;
			Time.timeScale = 0;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else{
			isPaused = false;
			Time.timeScale = 1;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	public void UpdatePlayerScore(int addToScore){
		score += addToScore;
		scoreText.text = $"Score: {score}";
	}

	public void UpdatePlayerHealth(int currentHealth){
		playerHealth = currentHealth;
		healthText.text = playerHealth.ToString();
		if (currentHealth <= 0){
			StartCoroutine(EndGame());
		}
	}

	private IEnumerator EndGame(){
		yield return new WaitForSeconds(gameOverDelay);
		UIManager.instance.SetCurrentlySelected(gameOverButton);
		freezeCamera.Freeze();
		Time.timeScale = 0;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		musicIntesity.StopMusic();
		gameOverScreen.SetActive(true);
	}
	
	public void EnterNameConfirm(string highScoreName){
		highScore.AddScore(score, highScoreName);
		Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}
}