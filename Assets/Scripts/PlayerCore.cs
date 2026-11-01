using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [Header("Character")]
    public CharacterScript characterScript;
    public Animator animator;

    GameManager gameManager;
    public PlayerCore opponentsCore;
    public EnemyCore enemyCore;

    [Header("Stats")]
    [SerializeField] private float health = 100;
    [SerializeField] private float defense = 0;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float damage = 10;

    [Header("Player Settings")]
    public int playerID;

    public int answer = -1;
    public int numberOne = 0;
    public int numberTwo = 0;

    private void Awake()
    {
        
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetDamage()
    {
        damage = characterScript.damage;

        return damage;
    }

    public float GetDefense()
    {
        defense = characterScript.defense;

        return defense;
    }

    public void Init(int id)
    {
        playerID = id;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Question();
    }

    public void RemoveHealth(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            Die();
        }

        animator.SetTrigger("Hurt");
        gameManager.audioSource.PlayOneShot(gameManager.audioManagerGameObject.GetComponent<AudioManager>().audioClipList[4]);
    }

    public void AddHealth(float amount)
    {
        health += amount;
    }

    public void SetHealth(float amount)
    {
        health = amount;
    }

    public void SetMaxHealth(float amount)
    {
        maxHealth = amount;
    }

    private void Die()
    {
        gameManager.GameOver();

        if (playerID == 1)
        {
            gameManager.ShowPlayerWin(2);
        }
        else
        {
            gameManager.ShowPlayerWin(1);
        }

        Debug.Log("Player " + playerID + " died.");
    }

    public void Question()
    {
        MainMenuCore.Difficulty difficulty = MainMenuCore.GetDifficulty();
        int digitExtra = 0;
        if (difficulty == MainMenuCore.Difficulty.MEDIUM)
        {
            digitExtra = 1;
        }
        else if (difficulty == MainMenuCore.Difficulty.HARD)
        {
            digitExtra = 2;
        }
        else if (difficulty == MainMenuCore.Difficulty.VERY_HARD)
        {
            digitExtra = 3;
        }
        else if (difficulty == MainMenuCore.Difficulty.INSANE)
        {
            digitExtra = 4;
        }
        numberOne = Random.Range(1+ digitExtra, 10 + digitExtra);
        numberTwo = Random.Range(1 + digitExtra, 10 + digitExtra);
        answer = numberOne * numberTwo;
        ManageAnswerButton();
        Debug.Log("Player " + playerID + " "+numberOne+"x"+numberTwo+" = "+answer);
    }

    public IEnumerator Attack()
    {
        Vector3 spawnPoint = gameManager.playerOneSpawnPoint.transform.position;
        if (playerID == 2)
        {
            spawnPoint = gameManager.playerTwoSpawnPoint.transform.position;
        }
        MoveForward(10);
        animator.SetTrigger("Attack");
        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.MULTIPLAYER)
        {

            float totalDamage = GetDamage() - (opponentsCore.GetDefense() / 100);

            if (totalDamage < 0)
            {
                totalDamage = 1;
            }

            opponentsCore.RemoveHealth(totalDamage);
        }
        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.SINGLEPLAYER)
        {

            float totalDamage = GetDamage() - (enemyCore.GetDefense() / 100);

            if (totalDamage < 0)
            {
                totalDamage = 1;
            }

            enemyCore.RemoveHealth(GetDamage());
        }
        yield return new WaitForSeconds(1.5f);
        transform.position = spawnPoint;
    }

    public void Answer(int v)
    {
        if (playerID == 1)
        {
            opponentsCore = gameManager.playerTwoCore;
        }
        else if (playerID == 2)
        {
            opponentsCore = gameManager.playerOneCore;
        }
        if (MainMenuCore.GetGameMode() == MainMenuCore.GameMode.SINGLEPLAYER)
        {
            enemyCore = gameManager.enemyCore;
        }
        if (v == answer)
        {
            StartCoroutine(Attack());

            Debug.Log("Player " + playerID + " Correct!");
        }
        else
        {
            RemoveHealth(GetDamage());

            Debug.Log("Player " + playerID + " Wrong!");
        }
        answer = -1;
        numberOne = 0;
        numberTwo = 0;
        Question();
    }

    public int GetAnswer()
    {
        return answer;
    }

    public void ManageAnswerButton()
    {
        List<GameObject> targetButtons;

        if (playerID == 1)
        {
            targetButtons = gameManager.playerOneButton;
        }
        else
        {
            targetButtons = gameManager.playerTwoButton;
        }

        int randomCorrect = Random.Range(0, targetButtons.Count);

        for (int i = 0; i < targetButtons.Count; i++)
        {
            AnswerButton btn = targetButtons[i].GetComponent<AnswerButton>();

            if (i == randomCorrect)
            {
                btn.SetAnswer(answer);
            }
            else
            {
                btn.SetAnswer(answer + Random.Range(2, 7));
            }
        }
    }

    private void Update()
    {
        if (!gameManager.isGameStarted())
        {
            return;
        }

        if (health > maxHealth)
        {
           SetHealth(maxHealth);
        }

        if (health < 0)
        {
            SetHealth(0);
        }
    }

    [Header("Costume")]
    public GameObject headGameObject;
    public GameObject bodyGameObject;
    public GameObject handGameObject;

    public GameObject handHandlerGameObject;
    public GameObject leftFeetGameObject;
    public GameObject rightFeetGameObject;

    public void ManageCostume()
    {
        if (characterScript == null)
        {
            Debug.LogWarning("Character Script is null!");
            return;
        }

        SetupPart(headGameObject,
            characterScript.headTexture,
            characterScript.headPosX,
            characterScript.headPosY,
            characterScript.headRotation,
            characterScript.headSizeX,
            characterScript.headSizeY
        );

        SetupPart(
            bodyGameObject,
            characterScript.bodyTexture,
            characterScript.bodyPosX,
            characterScript.bodyPosY,
            characterScript.bodyRotation,
            characterScript.bodySizeX,
            characterScript.bodySizeY
        );

        SetupPart(
            handGameObject,
            characterScript.handTexture,
            characterScript.handPosX,
            characterScript.handPosY,
            characterScript.handRotation,
            characterScript.handSizeX,
            characterScript.handSizeY
        );

        if (characterScript.useHands!)
        {
            handHandlerGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }

        if (characterScript.leftFeetHeight > 0f && characterScript.rightFeetHeight > 0)
        {
            leftFeetGameObject.transform.localScale = new Vector3(7, characterScript.leftFeetHeight, 1);
            rightFeetGameObject.transform.localScale = new Vector3(7, characterScript.rightFeetHeight, 1);
        }

        else if (characterScript.leftFeetHeight == 0f && characterScript.rightFeetHeight == 0)
        {
            leftFeetGameObject.transform.localScale = new Vector3(7, 7, 1);
            rightFeetGameObject.transform.localScale = new Vector3(7, 7, 1);
        }
    }


    public void ManageGameComponents()
    {
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController =
        characterScript.overrideController;
    }

    private void SetupPart(
        GameObject part,
        Sprite sprite,
        float posX,
        float posY,
        float rotation,
        float sizeX,
        float sizeY)
    {
        if (part == null) { return; }

        SpriteRenderer sr = part.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.sprite = sprite;

            part.transform.localPosition = new Vector3(posX, posY, 0);
            part.transform.localRotation = Quaternion.Euler(0, 0, rotation);
            part.transform.localScale = new Vector3(sizeX, sizeY, 1);
        }

    }


    // Abilities

    public void MoveForward(float distance)
    {
        if (playerID == 1)
        {
            transform.position += transform.right * distance;
        }

        else if (playerID == 2)
        {
            transform.position += transform.right * -1 * distance;
        }
    }
}