using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class LevelsPopup : MonoBehaviour
{
    private string fileName;
    [SerializeField] Transform levelsTableContent;
    [SerializeField] GameObject levelRowPrefab;
    GameLogic gameLogic; 
    private int totalLevels = 25;
    private int enabledLevels = 25;
    private bool internetAvailable = false;
    private const string highestScoreKeyPrefix = "HighestScore_";
    private const string IsUnlockedPrefix = "Unlocked_";
    bool isFirstTime = true;

    void Awake() {
        gameLogic = FindObjectOfType<GameLogic>();
    }
    void Start()
    {
        generateLevels();
    }

    void Update()
    {
    }

    public void generateLevels() {
        
        if (levelsTableContent == null)
        {
            Debug.LogError("The levelsTableContent reference is not set. Please assign the content area GameObject of the Scroll View.");
            return;
        }

        foreach (Transform child in levelsTableContent)
        {
            Destroy(child.gameObject);
        }

        int levelsToShow = Mathf.Min(enabledLevels, totalLevels);

        for (int i = 1; i <= levelsToShow; i++)
        {
            GameObject levelRow = Instantiate(levelRowPrefab, levelsTableContent);
            int highestScore = PlayerPrefs.GetInt(highestScoreKeyPrefix + i, 0);
            string filePath;
            if (i <= 15)
                filePath = Application.dataPath + "/Resources/RM_A" + i;
            else
                filePath = Application.dataPath + "/Resources/RM_B" + (i - 15);
            string[] lines = File.ReadAllLines(filePath);
            int moveCount = 0;
            foreach (string line in lines) {
                string[] parts = line.Split(':');
                if (parts.Length != 2)
                    continue;
                string variableName = parts[0].Trim().ToLower();
                string value = parts[1].Trim();
                switch (variableName)
                {
                    case "level_number":
                        break;
                    case "grid_width":
                        break;
                    case "grid_height":
                        break;
                    case "move_count":
                        int.TryParse(value, out moveCount);
                        break;
                    case "grid":
                        break;
                    default:
                        break;
                }
            }   
            TextMeshProUGUI levelNumberText = levelRow.transform.Find("LevelNumber").GetComponent<TextMeshProUGUI>();
            levelNumberText.text = "Level " + i.ToString() + "\n" + moveCount + " Moves\n" + "Highest Score: " + highestScore;

            Button playButton = levelRow.transform.Find("PlayButton").GetComponent<Button>();
            if (i == 1)
                playButton.interactable = true;
            else if (PlayerPrefs.GetInt(IsUnlockedPrefix + i, 0) != 1) {
                playButton.interactable = false;
            }
            int levelToPlay = i;
            playButton.onClick.AddListener(() => PlayLevel(levelToPlay, moveCount, highestScore));
        }
    }

    private void PlayLevel(int levelNumber, int moveCount, int highestScore)
    {
        if (levelNumber <= 15)
            FileName.filePath = "RM_A" + levelNumber;
        else
            FileName.filePath = "RM_B" + (levelNumber - 15); 
        this.gameObject.SetActive(false);
        if (!isFirstTime) {
            gameLogic.setMoveCount(moveCount);
            gameLogic.setScore(0);
            gameLogic.displayMoveCountLeft();
            gameLogic.UpdateScore();
            gameLogic.setLevelNumber(levelNumber);
            gameLogic.setHighestScore(highestScore);
            gameLogic.updateHighestScore();
            gameLogic.getCandyGrid().readFileAndInitializeGridVariables();
            gameLogic.getCandyGrid().createItemGrid();
            gameLogic.gameObject.SetActive(true);
        }
        else {
            gameLogic.gameObject.SetActive(true);
            isFirstTime = false;
        }
        
    }

}
