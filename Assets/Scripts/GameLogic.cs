using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameLogic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI moveCountText;
    [SerializeField] TextMeshProUGUI highestScoreText;
    [SerializeField] GridLayoutGroup itemGroup;
    private CandyGrid candyGrid;
    private int score = 0;
    private int moveCount;
    private GameObject selectedCandy1;
    private GameObject selectedCandy2;
    [SerializeField] GameObject replacementPrefab;
    [SerializeField] GameObject item1;
    [SerializeField] GameObject item2;
    [SerializeField] GameObject item3;
    [SerializeField] GameObject item4;
    bool rowMatch;
    bool rowMatch2;
    private bool isComplete;
    private bool highestScoreAchieved;
    public GameObject selectionTool;
    bool hasBecomeActive;
    private int[] highestScores;
    private int[] isUnlocked;
    private int levelNumber;
    private const string HighestScoreKeyPrefix = "HighestScore_";
    private const string IsUnlockedPrefix = "Unlocked_";
    private int highestScore;


    void Awake() {
        selectionTool = GameObject.FindGameObjectWithTag("Selection"); 
        highestScores = new int[25];
        isUnlocked = new int[25];
    }
    void Start()
    {
        selectionTool.SetActive(false);
        candyGrid = itemGroup.GetComponent<CandyGrid>();
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void displayMoveCountLeft() {
        moveCountText.text = "Moves Left: " + moveCount.ToString();
    }
    void UpdateMoveCount()
    {
        moveCount--;
        displayMoveCountLeft();
        if (moveCount == 0) {
            setHighestScoreForLevel(candyGrid.getLevel(), score);
            GameObject[] instances1 = GameObject.FindGameObjectsWithTag(replacementPrefab.tag);
            GameObject[] instances2 = GameObject.FindGameObjectsWithTag(item1.tag);
            GameObject[] instances3 = GameObject.FindGameObjectsWithTag(item2.tag);
            GameObject[] instances4 = GameObject.FindGameObjectsWithTag(item3.tag);
            GameObject[] instances5 = GameObject.FindGameObjectsWithTag(item4.tag);
            foreach (GameObject instance in instances1) {
                Destroy(instance);
            }
            foreach (GameObject instance in instances2) {
                Destroy(instance);
            }
            foreach (GameObject instance in instances3) {
                Destroy(instance);
            }
            foreach (GameObject instance in instances4) {
                Destroy(instance);
            }
            foreach (GameObject instance in instances5) {
                Destroy(instance);
            }
            selectionTool.SetActive(false);

            isComplete = true; 
        }
        
    }

    public void updateHighestScore() {
        highestScoreText.text = "Highest Score: " + highestScore;
    }

    void Update()
    {
        if (!hasBecomeActive) {
            moveCount = candyGrid.getMoveCount();
            levelNumber = candyGrid.getLevel();
            moveCountText.text = "Moves Left: " + moveCount.ToString();
            highestScore = PlayerPrefs.GetInt(HighestScoreKeyPrefix + levelNumber, 0);
            updateHighestScore();
            hasBecomeActive = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedCandy = hit.collider.gameObject;
                selectionTool.SetActive(true);
                selectionTool.transform.position = clickedCandy.transform.position;
                if (selectedCandy1 == null || selectedCandy1 == clickedCandy)
                {
                    PerformSwap(clickedCandy, selectedCandy1);
                }
                else if (selectedCandy2 == null)
                {
                    PerformSwap(selectedCandy1, clickedCandy);
                }

            }
        }
    }
    public void PerformSwap(GameObject candy1, GameObject candy2)
    {
        if (selectedCandy1 == null)
        {
            selectedCandy1 = candy1;
        }
        else if (selectedCandy2 == null)
        {
            selectedCandy2 = candy2;
            SwapCandies();
        }
    }

    private void SwapCandies()
    {
        if (IsAdjacent(selectedCandy1, selectedCandy2))
        {
            Vector2Int candy1GridPos = candyGrid.getGridPosition(selectedCandy1);
            Vector2Int candy2GridPos = candyGrid.getGridPosition(selectedCandy2);

            candyGrid.swapCandiesInGrid(candy1GridPos, candy2GridPos);

            candyGrid.swapCandiesInScene(selectedCandy1, selectedCandy2);
            bool rowMatch = checkRowMatch(selectedCandy1);
            bool rowMatch2 = checkRowMatch(selectedCandy2);
            if (rowMatch)
            {
                for (int i = 0; i < candyGrid.getWidth(); i++)
                {
                    GameObject itemGameObject = candyGrid.getCandies()[i, (int) selectedCandy1.transform.position.y];
                    Vector3 deletedPosition = itemGameObject.transform.position;
                    Instantiate(replacementPrefab, deletedPosition, Quaternion.identity);
                    if (itemGameObject.tag == "Green")
                    {
                        score += 150;
                    }
                    else if (itemGameObject.tag == "Blue")
                    {
                        score += 200;
                    }
                    else if (itemGameObject.tag == "Red")
                    {
                        score += 100;
                    }
                    else if (itemGameObject.tag == "Yellow")
                    {
                        score += 250;
                    }
                    UpdateScore();
                    itemGameObject.SetActive(false);

                }
            }
            if (rowMatch2)
            {
                for (int i = 0; i < candyGrid.getWidth(); i++)
                {
                    GameObject itemGameObject = candyGrid.getCandies()[i, (int) selectedCandy2.transform.position.y];
                    Vector3 deletedPosition = itemGameObject.transform.position;
                    Instantiate(replacementPrefab, deletedPosition, Quaternion.identity);
                    if (itemGameObject.tag == "Green")
                    {
                        score += 150;
                    }
                    else if (itemGameObject.tag == "Blue")
                    {
                        score += 200;
                    }
                    else if (itemGameObject.tag == "Red")
                    {
                        score += 100;
                    }
                    else if (itemGameObject.tag == "Yellow")
                    {
                        score += 250;
                    }
                    UpdateScore();
                    itemGameObject.SetActive(false);

                }
            }
            UpdateMoveCount();
        }
        selectedCandy1 = null;
        selectedCandy2 = null;
    }

    private bool IsAdjacent(GameObject candy1, GameObject candy2)
    {
        float xDifference = Mathf.Abs(candy1.transform.position.x - candy2.transform.position.x);
        float yDifference = Mathf.Abs(candy1.transform.position.y - candy2.transform.position.y);
        if (xDifference + yDifference == 1)
            return true;
        else
            return false;
    }

    public bool checkRowMatch(GameObject gameObj1)
    {
        bool match = true;
        for (int i = (int) gameObj1.transform.position.x + 1; i < candyGrid.getWidth(); i++)
        {
            if (gameObj1.name != candyGrid.getCandies()[i, (int) gameObj1.transform.position.y].name)
            {
                match = false;
                break;
            }
        }

        if (match)
        {
            for (int i = (int)gameObj1.transform.position.x - 1; i >= 0; i--)
            {
                if (gameObj1.name != candyGrid.getCandies()[i, (int)gameObj1.transform.position.y].name)
                {
                    match = false;
                    break;
                }
            }
        }
        return match;
    }

    public bool getIsComplete() {
        return isComplete;
    }

    public bool getHighestScoreAchieved() {
        return highestScoreAchieved;
    }

    public void setHighestScoreAchieved(bool highestScoreAchieved) {
        this.highestScoreAchieved = highestScoreAchieved;
    }

    public int getLevel() {
        return levelNumber;
    }

    public void setLevelNumber(int levelNumber) {
        this.levelNumber = levelNumber;
    }

    public void setHighestScoreForLevel(int level, int score)
    {
        if (level != 25) {
            PlayerPrefs.SetInt(IsUnlockedPrefix + (level + 1), 1);
        }

        if (score > highestScore)
        {
            highestScores[level] = score;

            PlayerPrefs.SetInt(HighestScoreKeyPrefix + level, score);
            highestScoreAchieved = true;
        }
    }

    public void setScore(int score) {
        this.score = score;
    }

    public int getScore() {
        return score;
    }

    public void setMoveCount(int moveCount) {
        this.moveCount = moveCount;
    }

    public int getMoveCount() {
        return moveCount;
    }

    public void setHighestScore(int highestScore) {
        this.highestScore = highestScore;
    }

    public void setIsComplete(bool isComplete) {
        this.isComplete = isComplete;
    }

    public CandyGrid getCandyGrid() {
        return candyGrid;
    }
}
