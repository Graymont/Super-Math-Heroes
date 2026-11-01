using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class GuideManager : MonoBehaviour
{
    public List<GameObject> guideObjectList = new List<GameObject>();

    private int currentPage = 0;

    private void Start()
    {
        ShowPage();
    }

    public void NextPage()
    {
        if (currentPage < guideObjectList.Count - 1)
        {
            currentPage++;
            ShowPage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage();
        }
    }

    private void ShowPage()
    {
        for (int i = 0; i < guideObjectList.Count; i++)
        {
            guideObjectList[i].SetActive(false);
        }

        guideObjectList[currentPage].SetActive(true);
    }

    public GameObject characterDetailsGameObject;
    public GameObject basicGuideGameObject;
    public void OpenCharacterDetails()
    {
        basicGuideGameObject.SetActive(false);
        characterDetailsGameObject.SetActive(true);
        
    }
}