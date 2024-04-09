using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManagerScript : MonoBehaviour
{
    public void BubbleSortButton()
    {
        SceneManager.LoadScene("BubbleSortScene");
    }

    public void SelectionSortButton()
    {
        SceneManager.LoadScene("SelectionSortScene");
    }

    public void KruskalMSTButton()
    {
        SceneManager.LoadScene("KruskalTreeFinal");
    }

    public void PrimMSTButton()
    {
        SceneManager.LoadScene("PrimTreeFinal");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
