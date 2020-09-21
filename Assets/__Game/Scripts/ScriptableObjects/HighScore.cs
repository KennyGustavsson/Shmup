using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HighScore", menuName = "ScriptableObjects/High Score")]
public class HighScore : ScriptableObject{
    public int amountOfScoresToShow = 5;
    [System.Serializable]
    public struct HighScoreStruct{
        public int score;
        public string name;
    }
    public List<HighScoreStruct> highScore;

    public void AddScore(int score, string scoreName){
        var newScore = new HighScoreStruct{score = score, name = scoreName};
        highScore.Add(newScore);
        
        var newArray = highScore.ToArray();
        if (newArray.Length > 1) {
            for (var i = 0; i < newArray.Length; i++) {
                for (var j = 0; j < newArray.Length; j++)
                {
                    if (!(newArray[j].score < newArray[i].score)) continue;
                    var temp = newArray[i];
                    newArray[i] = newArray[j];
                    newArray[j] = temp;
                }
            }
        }

        highScore = new List<HighScoreStruct>();
        if (newArray.Length >= amountOfScoresToShow){
            for (int k = 0; k < amountOfScoresToShow; k++){
                highScore.Add((newArray[k]));
            }
        }
        else{
            highScore = new List<HighScoreStruct>();
            foreach (var item in newArray){
                highScore.Add(item);
            }
        }
    }
}