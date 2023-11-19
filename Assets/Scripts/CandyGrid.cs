using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CandyGrid : MonoBehaviour
{
    [SerializeField] int height;
    [SerializeField] int width;
    private string filePath;
    private int levelNumber;
    private int gridWidth;
    private int gridHeight;
    private int moveCount;
    private string grid;
    private string[] itemsList;
    [SerializeField] GameObject[] candyPrefabs;
    private GameObject[,] candies;
    private GridLayoutGroup gridLayout;

    void Start()
    {
        readFileAndInitializeGridVariables();
        createItemGrid();
    }

    public void readFileAndInitializeGridVariables() {
        filePath = Application.dataPath + "/Resources/" + FileName.filePath;
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines) {
            string[] parts = line.Split(':');
            if (parts.Length != 2)
                continue;
            string variableName = parts[0].Trim().ToLower();
            string value = parts[1].Trim();
            switch (variableName)
            {
                case "level_number":
                    int.TryParse(value, out levelNumber);
                    break;
                case "grid_width":
                    int.TryParse(value, out gridWidth);
                    width = gridWidth;
                    break;
                case "grid_height":
                    int.TryParse(value, out gridHeight);
                    height = gridHeight;
                    break;
                case "move_count":
                    int.TryParse(value, out moveCount);
                    break;
                case "grid":
                    grid = value;
                    break;
                default:
                    break;
            }
        }
        itemsList = grid.Split(',');
        height = gridHeight;
        width = gridWidth;
    }

    public void createItemGrid() {
        gridLayout = GetComponent<GridLayoutGroup>();
        GameObject randomCandy = null;
        candies = new GameObject[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                
                string element = itemsList[height * i + j];
                if (element == "r") {
                    randomCandy = candyPrefabs[2];
                }
                else if (element == "g") {
                    randomCandy = candyPrefabs[1];
                }
                else if (element == "b") {
                    randomCandy = candyPrefabs[0];
                }
                else if (element == "y") {
                    randomCandy = candyPrefabs[3];
                }
                
                GameObject candy = Instantiate(randomCandy, new Vector3(i, j, 0), Quaternion.identity);
                
                candy.transform.SetParent(transform);
                candies[i, j] = candy;

                
            }
        }
        
        
        if (width % 2 == 1 && height % 2 == 1) {
            Camera.main.transform.position = new Vector3((width / 2), (height / 2), -10);
        }
        else if (width % 2 == 0 && height % 2 == 0) {
            Camera.main.transform.position = new Vector3(width / 2 - 0.5f, (height / 2) -0.5f, -10);
        }
        else if (width % 2 == 0 && height % 2 == 1) {
            Camera.main.transform.position = new Vector3(width / 2 - 0.5f, (height / 2) , -10);
        }
        else {
            Camera.main.transform.position = new Vector3(width / 2, (height / 2) -0.5f , -10);
        }
    }

    public GameObject[,] getCandies() {
        return candies;
    }

    public void setCandies(int x, int y, GameObject gameObj) {
        candies[x, y] = gameObj;
    }

    public Vector2Int getGridPosition(GameObject gameObj) {
        return new Vector2Int((int) gameObj.transform.position.x, (int) gameObj.transform.position.y);
    }

    public void swapCandiesInGrid(Vector2Int pos1, Vector2Int pos2) {
        GameObject gameObj = candies[pos1.x, pos1.y];
        candies[pos1.x, pos1.y] = candies[pos2.x, pos2.y];
        candies[pos2.x, pos2.y] = gameObj;
    }

    public void swapCandiesInScene(GameObject gameObj1, GameObject gameObj2) {
        Vector3 pos1 = gameObj1.transform.position;
        Vector3 pos2 = gameObj2.transform.position;
        gameObj1.transform.position = pos2;
        gameObj2.transform.position = pos1;
    }

    public int getMoveCount() {
        return moveCount;
    }

    public int getWidth() {
        return width;
    }

    public int getLevel() {
        return levelNumber;
    }
   
}
