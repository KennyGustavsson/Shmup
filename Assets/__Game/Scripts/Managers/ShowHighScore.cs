using UnityEngine;
using UnityEngine.UI;

public class ShowHighScore : MonoBehaviour{
	public HighScore highScore;
	
	private Text _textComponent;
	private string _textString;
	
	private void Awake(){
		_textComponent = GetComponent<Text>();
		
		_textString = "High Scores:\n";
		foreach (var score in highScore.highScore){
			_textString += $"{score.score} {score.name}\n";
		}
		_textComponent.text = _textString;
	}
}