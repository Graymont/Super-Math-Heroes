using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject audioManagerGameObject;
    public AudioSource audioSource;

    [Header ("Data Storage")]

    public List<CharacterScript> characterList = new List<CharacterScript>();

    [Header("Gameplay")]

    [Header("Main Player Stuffs")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Enemy Stuffs")]
    [SerializeField] private GameObject enemyPrefab;
    public GameObject enemy;
    public EnemyCore enemyCore;

    [Header("--")]

    public Transform playerOneSpawnPoint;
    public Transform playerTwoSpawnPoint;

    public GameObject gameOverUI;

    public GameObject playerOne;
    public GameObject playerTwo;

    public PlayerCore playerOneCore;
    public PlayerCore playerTwoCore;

    public List<GameObject> playerOneButton = new List<GameObject>();
    public List<GameObject> playerTwoButton = new List<GameObject>();

    public TextMeshProUGUI playerOneQuestionText;
    public TextMeshProUGUI playerTwoQuestionText;

    public Slider playerOneSlider;
    public Slider playerTwoSlider;

    public Slider playerOneSliderFiller;
    public Slider playerTwoSliderFiller;

    [Header("Match")]
    public GameObject startGameCountdown;
    public TextMeshProUGUI startGameCountdownText;
    public GameObject playerGameController;
    public float matchTime = 100;
    public TextMeshProUGUI matchTimeText;
    public bool isStarted = false;


    public GameObject winnerGameObject;
    public TextMeshProUGUI winnerText;


    public TextMeshProUGUI difficultyText;

    [Header("Game Controller")]
    public GameObject playerOneGameController;
    public GameObject playerTwoGameController;

    public GameObject playerOneQuestionBox;
    public GameObject playerTwoQuestionBox;

    public GameObject potraitGameObjectOne;
    public GameObject potraitGameObjectTwo;
    private void Start()
    {
        audioManagerGameObject = GameObject.Find("AudioManager");
        audioSource = audioManagerGameObject.GetComponent<AudioSource>();

        ManageTheme();
        faceOffGameObject.SetActive(false);

        SpawnPlayers();
        CharacterSetup();

        StartCoroutine(StartFaceOff());
    }
    public IEnumerator StartFaceOff()
    {
        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.SINGLEPLAYER)
        {
            SetRandomEnemy();

            enemyCore.ManageGameComponents();
            enemyCore.ManageCostume();
        }

        faceOffGameObject.SetActive(true);
        mascotGameObject.SetActive(false);

        SetGameState(GameState.FACE_OFF);

        yield return new WaitForSeconds(1f);
        SpawnMascot();
        mascotGameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        faceOffGameObject.SetActive(false);
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        battleSceneGameObject.SetActive(true);

        startGameCountdown.SetActive(true);
        difficultyText.gameObject.SetActive(false);
        potraitGameObjectOne.SetActive(false);
        potraitGameObjectTwo.SetActive(false);

        int startCount = 0;

        for (int i = 3; i > 0; i--)
        {
            startGameCountdownText.text = i.ToString();
            audioSource.PlayOneShot(audioManagerGameObject.GetComponent<AudioManager>().audioClipList[startCount]);

            startCount++;
            yield return new WaitForSeconds(1f);
        }

        startGameCountdownText.text = "FIGHT!";

        SetupAttributes();

        audioSource.PlayOneShot(audioManagerGameObject.GetComponent<AudioManager>().audioClipList[startCount]);
        yield return new WaitForSeconds(1f);

        potraitGameObjectOne.SetActive(true);
        potraitGameObjectTwo.SetActive(true);
        startGameCountdown.SetActive(false);
        difficultyText.gameObject.SetActive(true);

        isStarted = true;
        playerGameController.SetActive(true);

        playerOneSlider.maxValue = playerOneCore.GetHealth();
        playerOneSlider.value = playerOneCore.GetHealth();

        playerOneSliderFiller.maxValue = playerOneCore.GetHealth();
        playerOneSliderFiller.value = playerOneCore.GetHealth();

        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.MULTIPLAYER)
        {
            playerTwoSlider.maxValue = playerTwoCore.GetHealth();
            playerTwoSlider.value = playerTwoCore.GetHealth();

            playerTwoSliderFiller.maxValue = playerTwoCore.GetHealth();
            playerTwoSliderFiller.value = playerTwoCore.GetHealth();
        }
        else
        {
            playerTwoSlider.maxValue = enemyCore.GetHealth();
            playerTwoSlider.value = enemyCore.GetHealth();

            playerTwoSliderFiller.maxValue = enemyCore.GetHealth();
            playerTwoSliderFiller.value = enemyCore.GetHealth();
        }

        SetGameState(GameState.BATTLE);
        Debug.Log("Game Started");
    }

    public void SetupAttributes()
    {
        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.SINGLEPLAYER)
        {
            playerOneCore.SetMaxHealth(playerOneCore.characterScript.health);
            playerOneCore.SetHealth(playerOneCore.GetMaxHealth());

            enemyCore.SetMaxHealth(enemyCore.characterScript.health);
            enemyCore.SetHealth(enemyCore.GetMaxHealth());
        }

        else if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.MULTIPLAYER)
        {
            playerOneCore.SetMaxHealth(playerOneCore.characterScript.health);
            playerOneCore.SetHealth(playerOneCore.GetMaxHealth());

            playerTwoCore.SetMaxHealth(playerTwoCore.characterScript.health);
            playerTwoCore.SetHealth(playerTwoCore.GetMaxHealth());
        }
    }

    public void CharacterSetup()
    {
        int playerOneID = PlayerPrefs.GetInt("Player 1 Selected Character", 0);
        playerOneCore.characterScript = characterList.Find(character => character.id == playerOneID);
        if (playerOneCore.characterScript == null)
            playerOneCore.characterScript = characterList[0];
        playerOneCore.ManageGameComponents();
        playerOneCore.ManageCostume();

        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.MULTIPLAYER)
        {
            int playerTwoID = PlayerPrefs.GetInt("Player 2 Selected Character", 0);
            if (playerTwoCore.characterScript == null)
                playerTwoCore.characterScript = characterList[0];
            playerTwoCore.characterScript = characterList.Find(character => character.id == playerTwoID);
            playerTwoCore.ManageGameComponents();
            playerTwoCore.ManageCostume();
        }
    }

    public void StopGame()
    {
        isStarted = false;
    }

    public bool isGameStarted()
    {
        return isStarted;
    }


    public GameObject faceOffGameObject;
    public GameObject mascotGameObject;
    public GameObject battleSceneGameObject;

    public void SetRandomEnemy()
    {
        int enemyGet = Random.Range(0, characterList.Count);

        enemyCore.characterScript = characterList[enemyGet];
    }


    private void SpawnPlayers()
    {
        // Player 1
        playerOne = Instantiate(
            playerPrefab,
            playerOneSpawnPoint.position,
            Quaternion.identity
            );

        playerOneCore = playerOne.GetComponent<PlayerCore>();
        playerOneCore.Init(1);

        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.SINGLEPLAYER)
        {
            // Enemy
            enemy = Instantiate(
                enemyPrefab,
                playerTwoSpawnPoint.position,
                Quaternion.identity
                );

            enemyCore = enemy.GetComponent<EnemyCore>();
            enemyCore.Init();

            enemy.transform.localScale = new Vector2(-enemy.transform.localScale.x, enemy.transform.localScale.y);
            return;
        }

        // Player 2
        playerTwo = Instantiate(
            playerPrefab,
            playerTwoSpawnPoint.position,
            Quaternion.identity
            );

        playerTwoCore = playerTwo.GetComponent<PlayerCore>();
        playerTwoCore.Init(2);

        playerTwo.transform.localScale = new Vector2 ( -playerTwo.transform.localScale.x, playerTwo.transform.localScale.y );
    }

    public Image mascotOne;
    public Image mascotTwo;

    public TextMeshProUGUI mascotOneDisplayName;
    public TextMeshProUGUI mascotTwoDisplayName;

    

    public void SpawnMascot()
    {
        //float size = 500;
        // Player 1

        int playerOneID = PlayerPrefs.GetInt("Player 1 Selected Character", 0);
        CharacterScript playerOneCharacterScript = characterList.Find(character => character.id == playerOneID);
        float playerOnePotraitSize = playerOneCharacterScript.potraitSize;
        int playerTwoID = PlayerPrefs.GetInt("Player 2 Selected Character", 0);
        CharacterScript playerTwoCharacterScript = characterList.Find(character => character.id == playerTwoID);
        float playerTwoPotraitSize = playerTwoCharacterScript.potraitSize;

        mascotOne.sprite = playerOneCharacterScript.portrait;
        if (isSmallerPotraitCharacter(playerOneCharacterScript.name))
        {
            mascotOne.GetComponent<RectTransform>().sizeDelta = new Vector2(playerOnePotraitSize, playerOnePotraitSize);
        }

        mascotOneDisplayName.text = playerOneCharacterScript.name;

        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.SINGLEPLAYER)
        {
            mascotTwo.sprite = enemyCore.characterScript.portrait;
            if (isSmallerPotraitCharacter(playerTwoCharacterScript.name))
            {
                mascotTwo.GetComponent<RectTransform>().sizeDelta = new Vector2(playerTwoPotraitSize, playerTwoPotraitSize);
            }
            mascotTwoDisplayName.text = enemyCore.characterScript.name;
            return;
        }

        // Player 2
        mascotTwo.sprite = playerTwoCharacterScript.portrait;
        if (isSmallerPotraitCharacter(playerTwoCharacterScript.name))
        {
            mascotTwo.GetComponent<RectTransform>().sizeDelta = new Vector2(playerTwoPotraitSize, playerTwoPotraitSize);
        }
        mascotTwoDisplayName.text = playerTwoCharacterScript.name;
    }

    public bool isSmallerPotraitCharacter(string name)
    {
        return name == "Paladin" || name == "Spartan";
    }

    public static GameState currentState = GameState.NONE;

    public static GameState GetGameState()
    {
        return currentState;
    }

    public void SetGameState(GameState state)
    {
        currentState = state;

        Debug.Log("Set Game State to : "+GetGameState());
    }

    public enum GameState
    {
        NONE,
        FACE_OFF,
        BATTLE,
        GAME_OVER
    }

    private void Update()
    {
        GameOverClick();

        if (!isGameStarted())
        {
            return;
        }

        ManageUserInterface();

        matchTime -= 1 * Time.deltaTime;

        if (matchTime <= 0)
        {
            GameOver();

            float playerOneHealth = playerOneCore.GetHealth();
            float playerTwoHealth = playerTwoCore?.GetHealth() ?? 0f;

            if (playerOneHealth > playerTwoHealth)
            {
                ShowPlayerWin(1);
            }

            else {
                ShowPlayerWin(2);
            }
        }
    }

    public void GameOver()
    {
        audioSource.PlayOneShot(audioManagerGameObject.GetComponent<AudioManager>().audioClipList[5]);

        gameOverUI.SetActive(true);
        SetGameState(GameState.GAME_OVER);

        playerGameController.SetActive(false);
        StopGame();
    }

    public void ShowPlayerWin(int id)
    {
        string winner = "One";

        if (id == 2)
        {
            winner = "Two";
        }

        winnerGameObject.SetActive(true);

        winnerText.text = "Player " + winner + " Won!";
    }

    public void GameOverClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isGameStarted())
            {
                return;
            }
            gameOverUI.SetActive(false);

            SceneManager.LoadScene("MainMenu");
        }
    }

    public Image playerOnePotrait;
    public Image playerTwoPotrait;

    public GameObject theme;

    public List<ThemeScript> themeList = new List<ThemeScript>();

    public void ManageTheme()
    {
        int themeID = MainMenuCore.GetTheme();
        ThemeScript selectedTheme = themeList.Find(t => t.id == themeID);
        theme.GetComponent<SpriteRenderer>().sprite = selectedTheme.display;
        theme.transform.position = selectedTheme.position;
        theme.transform.localScale = selectedTheme.size;
    }

    public TextMeshProUGUI playerOneHealthText;
    public TextMeshProUGUI playerTwoHealthText;

    public void ManageUserInterface()
    {

        if (GetGameState() == GameState.FACE_OFF)
        {
            return;
        }

        MainMenuCore.GameMode gameMode = MainMenuCore.GetGameMode();

        if (gameMode == MainMenuCore.GameMode.SINGLEPLAYER)
        {
            playerTwoGameController.SetActive(false);
            playerTwoQuestionBox.SetActive(false);
        }

        int randomCorrect = Random.Range( 0, playerOneButton.Count );

        foreach (GameObject playerButton in playerOneButton)
        {
            int answer = playerButton.GetComponent<AnswerButton>().GetAnswer();
            playerButton.GetComponentInChildren<TextMeshProUGUI>().text = ""+answer;
        }

        foreach (GameObject playerButton in playerTwoButton)
        {
            int answer = playerButton.GetComponent<AnswerButton>().GetAnswer();
            playerButton.GetComponentInChildren<TextMeshProUGUI>().text = "" + answer;
        }

        playerOneQuestionText.text = playerOneCore.numberOne + " x " + playerOneCore.numberTwo + " = ?";

        if (gameMode == MainMenuCore.GameMode.MULTIPLAYER)
        {
            playerTwoQuestionText.text = playerTwoCore.numberOne + " x " + playerTwoCore.numberTwo + " = ?";
        }


        float playerOneHealth = playerOneCore.GetHealth();
        float playerTwoHealth;

        float playerOneMaxHealth = playerOneCore.characterScript.health;
        float playerTwoMaxHealth;

        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.SINGLEPLAYER)
        {
            playerTwoHealth = enemyCore.GetHealth();
            playerTwoMaxHealth = enemyCore.characterScript.health;
        }
        else
        {
            playerTwoHealth = playerTwoCore.GetHealth();
            playerTwoMaxHealth = playerTwoCore.characterScript.health;
        }

        playerOneSlider.value = playerOneHealth;
        playerTwoSlider.value = playerTwoHealth;

        if (playerOneSliderFiller.value > playerOneHealth)
        {
            playerOneSliderFiller.value -=
                (playerOneSliderFiller.maxValue * 0.05f) * Time.deltaTime;

            if (playerOneSliderFiller.value < playerOneHealth)
            {
                playerOneSliderFiller.value = playerOneHealth;
            }
        }

        if (playerTwoSliderFiller.value > playerTwoHealth)
        {
            playerTwoSliderFiller.value -=
                (playerTwoSliderFiller.maxValue * 0.05f) * Time.deltaTime;

            if (playerTwoSliderFiller.value < playerTwoHealth)
            {
                playerTwoSliderFiller.value = playerTwoHealth;
            }
        }

        playerOneHealthText.text = "" + playerOneHealth.ToString("F0") + "/" + playerOneMaxHealth.ToString("F0");
        playerTwoHealthText.text = "" + playerTwoHealth.ToString("F0") + "/" + playerTwoMaxHealth.ToString("F0");

        matchTimeText.text = matchTime.ToString("F0") + "";


        difficultyText.text = MainMenuCore.GetDifficulty().ToString().Replace("_", "");

        ManagePotrait();
    }

    public GameObject pauseMenuGameObject;

    public void PauseGame()
    {
        pauseMenuGameObject.SetActive(true);

        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        pauseMenuGameObject.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void ReturnToMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void ManagePotrait()
    {
        float size = 150;

        if (isSmallerPotraitCharacter(playerOneCore.characterScript.name))
        {
            potraitGameObjectOne.GetComponent<Image>().GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            
        }
        if (playerTwoCore != null)
        {
            if (isSmallerPotraitCharacter(playerTwoCore.characterScript.name))
            {
                potraitGameObjectTwo.GetComponent<Image>().GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            }

        }
        if (enemyCore != null)
        {
            if (isSmallerPotraitCharacter(enemyCore.characterScript.name))
            {
                potraitGameObjectTwo.GetComponent<Image>().GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            }

        }

        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.SINGLEPLAYER)
        {
            playerOnePotrait.sprite = playerOneCore.characterScript.portrait;
            playerTwoPotrait.sprite = enemyCore.characterScript.portrait;
        }
        else if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.MULTIPLAYER)
        {
            playerOnePotrait.sprite = playerOneCore.characterScript.portrait;
            playerTwoPotrait.sprite = playerTwoCore.characterScript.portrait;
        }
    }

}