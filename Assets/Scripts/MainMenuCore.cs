using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCore : MonoBehaviour
{

    public GameMode gameMode = GameMode.SINGLEPLAYER;
    public Difficulty difficulty = Difficulty.MEDIUM;

    private void Update()
    {
        playerIdText.text = "Player "+characterSelector.playerId;
    }

    private void Start()
    {
        difficultySelectionMenu.SetActive(false);
        characterSelectionMenu.SetActive(false);

        themeDisplay.sprite = themeList[currentThemeIndex].display;
        themeName.text = themeList[currentThemeIndex].name;
    }

    public void SelectDifficulty(int dif)
    {
        difficulty = (Difficulty)dif;

        PlayerPrefs.SetString("Difficulty", difficulty.ToString());

        if (gameMode == GameMode.SINGLEPLAYER) { Singleplayer(); }

        else if (gameMode == GameMode.MULTIPLAYER) { Multiplayer(); }
    }

    public void SelectGameMode(int gm)
    {
        gameMode = (GameMode)gm;

        PlayerPrefs.SetString("GameMode", gameMode.ToString());

        OpenDifficultySelector();
    }

    public static Difficulty GetDifficulty()
    {
        string savedDifficulty =
            PlayerPrefs.GetString("Difficulty", Difficulty.MEDIUM.ToString());

        if (System.Enum.TryParse(savedDifficulty, out Difficulty result))
        {
            return result;
        }

        return Difficulty.MEDIUM;
    }

    public static GameMode GetGameMode()
    {
        string savedDifficulty =
            PlayerPrefs.GetString("GameMode", GameMode.SINGLEPLAYER.ToString());

        if (System.Enum.TryParse(savedDifficulty, out GameMode result))
        {
            return result;
        }

        return GameMode.SINGLEPLAYER;
    }

    public enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD,
        VERY_HARD,
        INSANE
    }

    public enum GameMode
    {
        SINGLEPLAYER,
        MULTIPLAYER
    }

    public Difficulty ParseDifficulty(string t)
    {
        switch (t.ToLower())
        {
            case "easy":
                return Difficulty.EASY;

            case "medium":
                return Difficulty.MEDIUM;

            case "hard":
                return Difficulty.HARD;

            case "very_hard":
                return Difficulty.VERY_HARD;

            case "insane":
                return Difficulty.INSANE;

            default:
                return Difficulty.MEDIUM;
        }
    }

    public GameMode ParseGameMode(string t)
    {
        switch (t.ToLower())
        {
            case "singleplayer":
                return GameMode.SINGLEPLAYER;

            case "multiplayer":
                return GameMode.MULTIPLAYER;

            default:
                return GameMode.SINGLEPLAYER;
        }
    }

    public void Ping(string text)
    {
        Debug.Log(text);
    }

    public GameObject difficultySelectionMenu;
    public GameObject characterSelectionMenu;
    public GameObject gameOptionsMenu;
    public GameObject guideMenu;

    public CharacterSelector characterSelector;

    public TextMeshProUGUI playerIdText;

    public void OpenCharacterSelector()
    {
        difficultySelectionMenu.SetActive(false);
        characterSelectionMenu.SetActive(true);
    }

    public void OpenDifficultySelector()
    {
        difficultySelectionMenu.SetActive(true);
    }
    public void Singleplayer()
    {
        OpenCharacterSelector();
        characterSelector.SetPlayerID(1);
    }

    public void Multiplayer()
    {
        OpenCharacterSelector();
        characterSelector.SetPlayerID(1);
    }

    public void OpenGameOptions()
    {
        gameOptionsMenu.SetActive(true);
    }

    public void OpenGuideMenu()
    {
        guideMenu.SetActive(true);
    }

    public List<ThemeScript> themeList = new List<ThemeScript>();
    public void SelectTheme(int id)
    {
        PlayerPrefs.SetInt("Theme", id);
    }

    public static int GetTheme()
    {
        return PlayerPrefs.GetInt("Theme", 1);
    }

    private int currentThemeIndex = 0;

    public ThemeScript CurrentTheme
    {
        get { return themeList[currentThemeIndex]; }
    }

    public void NextTheme()
    {
        currentThemeIndex++;

        if (currentThemeIndex >= themeList.Count)
            currentThemeIndex = 0;

        RefreshTheme();
    }

    public void PreviousTheme()
    {
        currentThemeIndex--;

        if (currentThemeIndex < 0)
            currentThemeIndex = themeList.Count - 1;

        RefreshTheme();
    }

    public Image themeDisplay;
    public TextMeshProUGUI themeName;

    public void RefreshTheme()
    {
        ThemeScript theme = themeList[currentThemeIndex];

        SelectTheme(theme.id);

        themeDisplay.sprite = theme.display;
        themeName.text = theme.name;

        Debug.Log("Selected Theme: "+theme.name+" id: "+GetTheme());
    }

    public void PlayButtonSound()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(
            GameObject.Find("AudioManager").GetComponent<AudioManager>().audioClipList[1]);
    }
}
