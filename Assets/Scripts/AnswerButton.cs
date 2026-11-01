using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] int playerID = -1;

    int answer = -1;

    public void SetAnswer(int v)
    {
        answer = v;
    }

    public int GetAnswer()
    {
        return answer;
    }

    public void Answer()
    {

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (!gameManager.isGameStarted())
        {
            Debug.Log("Game Over!");
            return;
        }

        PlayerCore playerOne = GameObject.Find("GameManager").GetComponent<GameManager>().playerOne.GetComponent<PlayerCore>();
        PlayerCore playerTwo = null;
        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.MULTIPLAYER)
        {
            playerTwo = GameObject.Find("GameManager").GetComponent<GameManager>().playerTwo.GetComponent<PlayerCore>();
        }

        PlayerCore playerCore = null;

        if (playerID > 2 || playerID < 1)
        {
            return;
        }

        if (playerID == 1)
        {
            playerCore = playerOne;
        }

        else if (playerID == 2)
        {
            playerCore = playerTwo;
        }

        playerCore.Answer(GetAnswer());

        Debug.Log("Player ID: " + playerID);
    }
}
