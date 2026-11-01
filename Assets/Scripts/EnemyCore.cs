using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    public CharacterScript characterScript;
    public Animator animator;

    public Transform enemySpawnPoint;
    public float attackTimer = 0f;

    public float attackSpeed = 3;

    public GameManager gameManager;

    [Header("Stats")]
    [SerializeField] private float health = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float damage = 10;
    [SerializeField] private float defense = 0;

    
    private void Update()
    {
        if (GameManager.GetGameState() == GameManager.GameState.BATTLE)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackSpeed)
            {
                StartCoroutine(Attack());
                attackTimer = 0f;
            }
        }
    }

    private void Awake()
    {

        MainMenuCore.Difficulty difficulty = MainMenuCore.GetDifficulty();

        switch (difficulty)
        {
            case MainMenuCore.Difficulty.EASY:
                attackSpeed = 8;
                break;
            case MainMenuCore.Difficulty.MEDIUM:
                attackSpeed = 6;
                break;
            case MainMenuCore.Difficulty.HARD:
                attackSpeed = 4;
                break;
            case MainMenuCore.Difficulty.VERY_HARD:
                attackSpeed = 2;
                break;
            case MainMenuCore.Difficulty.INSANE:
                attackSpeed = 1;
                break;
        }

        health = maxHealth;

        attackTimer = -2f;
    }

    public void ManageGameComponents()
    {
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController =
        characterScript.overrideController;
    }

    public IEnumerator Attack()
    {
        if (GameManager.GetGameState() == GameManager.GameState.BATTLE)
        {
            gameManager.playerOneCore.RemoveHealth(GetDamage());


            Vector3 spawnPoint = gameManager.playerTwoSpawnPoint.transform.position;

            MoveForward(10);
            animator.SetTrigger("Attack");

            yield return new WaitForSeconds(1.5f);
            transform.position = spawnPoint;
        }
        Debug.Log("Enemy Attacked!");
    }

    public void MoveForward(float distance)
    {
        transform.position += transform.right * -1 * distance;
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

        SetupPart(
            headGameObject,
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

    public void Init()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
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
        if (part == null)
            return;

        SpriteRenderer sr = part.GetComponent<SpriteRenderer>();

        if (sr != null)
            sr.sprite = sprite;

        part.transform.localPosition = new Vector3(posX, posY, 0);
        part.transform.localRotation = Quaternion.Euler(0, 0, rotation);
        part.transform.localScale = new Vector3(sizeX, sizeY, 1);
    }

    private void Die()
    {
        gameManager.GameOver();
    }
}
