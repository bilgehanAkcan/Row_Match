using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
public class GameManager : MonoBehaviour
{
    StartScreen startScreen;
    GameLogic gameLogic;
    CelebrationScript celebrationScreen;
    LevelsPopup levelsPopup;
    bool isCelebrationShown;
    private bool isConnected;
    bool check;
    void Awake()
    {
        startScreen = FindObjectOfType<StartScreen>();
        gameLogic = FindObjectOfType<GameLogic>();
        celebrationScreen = FindObjectOfType<CelebrationScript>();
        levelsPopup = FindObjectOfType<LevelsPopup>();
        startScreen.gameObject.SetActive(true);
        gameLogic.gameObject.SetActive(false);
        celebrationScreen.gameObject.SetActive(false);
        levelsPopup.gameObject.SetActive(false);
    }
    void Start() {
        
        isConnected = Application.internetReachability != NetworkReachability.NotReachable;
        if (isConnected) {
            string filePath = Application.dataPath + "/LevelURLs/LevelURLs.txt";
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines) {
                //StartCoroutine(DownloadLevelDataCoroutine(line));
            }
        }
    }
    void Update() {
        if (gameLogic.getIsComplete() && !isCelebrationShown && gameLogic.getHighestScoreAchieved()) {
            StartCoroutine(ShowCelebrationAndActivateNext());
        }
        else if (gameLogic.getIsComplete() && !check && !gameLogic.getHighestScoreAchieved()) {
            StartCoroutine(ShowLevelsPopup());
        }
        isConnected = Application.internetReachability != NetworkReachability.NotReachable;
        if (isConnected) {
            string filePath = Application.dataPath + "/LevelURLs/LevelURLs.txt";
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines) {
                //StartCoroutine(DownloadLevelDataCoroutine(line));
            }
        }      
    }

    private IEnumerator DownloadLevelDataCoroutine(string line)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(line))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Level data download failed: " + www.error);
            }
            else
            {
                string filePath = Application.dataPath + "/Resources/";
                File.WriteAllBytes(filePath, www.downloadHandler.data);

                Debug.Log("Level data downloaded and saved locally.");
            }
        }
    }

    IEnumerator ShowCelebrationAndActivateNext()
    {
        gameLogic.gameObject.SetActive(false);
        celebrationScreen.updateHighestScoreText();
        celebrationScreen.gameObject.SetActive(true);
        isCelebrationShown = true;

        yield return new WaitForSeconds(5f);

        celebrationScreen.gameObject.SetActive(false);
        
        levelsPopup.gameObject.SetActive(true);
        levelsPopup.generateLevels();
        ResetPageFlowVariables();
        
    }

    IEnumerator ShowLevelsPopup()
    {
        gameLogic.gameObject.SetActive(false);
        startScreen.gameObject.SetActive(true);
        check = true;

        yield return new WaitForSeconds(1f);

        startScreen.gameObject.SetActive(false);
        
        levelsPopup.gameObject.SetActive(true);
        levelsPopup.generateLevels();
        ResetPageFlowVariables();
        
    }

    void ResetPageFlowVariables() {
        isCelebrationShown = false;
        check = false;
        gameLogic.setIsComplete(false);
        gameLogic.setHighestScoreAchieved(false);
    }

    public void onLevelsButtonClicked() {
        levelsPopup.gameObject.SetActive(true);
        startScreen.gameObject.SetActive(false);
    }
}
