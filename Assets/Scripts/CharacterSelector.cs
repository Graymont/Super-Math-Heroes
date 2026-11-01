using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    int selectedCharacter = 0;
    public int playerId = 0;

    public MainMenuCore mainMenuCore;

    public void SelectCharacter(int id)
    {
        selectedCharacter = id;

        PlayerPrefs.SetInt("Player " + playerId + " Selected Character", id);

        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.MULTIPLAYER)
        {
            StartCoroutine(SecondCharacterSelection());
        }
        else if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.SINGLEPLAYER)
        {
            SceneManager.LoadScene("Spring Valley");
        }
    }

    public IEnumerator SecondCharacterSelection()
    {
        if (playerId == 2)
        {
            SceneManager.LoadScene("Spring Valley");
            yield return null;
        }
        SetPlayerID(2);
        mainMenuCore.characterSelectionMenu.SetActive(false);

        yield return new WaitForSeconds(1f);
        mainMenuCore.OpenCharacterSelector(); 
    }

    public void SetPlayerID(int id)
    {
        playerId = id;

        if (playerId > 0)
        {
            PlayerPrefs.SetInt("Player "+id+" Character", id);
        }
    }

    public void Ping(string text)
    {
        Debug.Log(text);
    }
}
