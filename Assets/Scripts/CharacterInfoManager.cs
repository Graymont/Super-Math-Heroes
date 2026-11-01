using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CharacterInfoManager : MonoBehaviour
{
    public List<CharacterScript> characterList = new List<CharacterScript>();

    public Image characterPotrait;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;

    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characterDescriptionText;

    private int currentCharacter = 0;

    private void Start()
    {
        UpdateCharacter();
    }

    public void NextPage()
    {
        if (currentCharacter < characterList.Count - 1)
        {
            currentCharacter++;

            UpdateCharacter();
        }
    }

    public void PreviousPage()
    {
        if (currentCharacter > 0)
        {
            currentCharacter--;

            UpdateCharacter();
        }
    }

    private void UpdateCharacter()
    {
        CharacterScript character = characterList[currentCharacter];

        characterNameText.text = character.name;
        characterDescriptionText.text = character.description;

        damageText.text = character.damage.ToString();
        healthText.text = character.health.ToString();
        armorText.text = character.defense.ToString();

        characterPotrait.sprite = character.portrait;
    }
}