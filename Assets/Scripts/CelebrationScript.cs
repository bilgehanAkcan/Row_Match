using UnityEngine;
using TMPro;
public class CelebrationScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameLogic gameLogic;
    private const string highestScoreKeyPrefix = "HighestScore_";
    void Start()
    {
        updateHighestScoreText();
    }

    public void updateHighestScoreText() {
        scoreText.text = "Highest Score\n" + PlayerPrefs.GetInt(highestScoreKeyPrefix + gameLogic.getLevel(), 0);
    }
}
