using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PrimMiniGameScript : MonoBehaviour
{
    List<string> actionList = new List<string>();
    public Transform[] vertices;
    public Transform[] edgesObjects;
    private SpriteRenderer spriteRenderer;

    // variables for mini-game
    bool addMinKey = false;
    bool searchNotVisited = false;
    bool edgesMinusVertices = false;
    bool updateParent = false;
    bool addGreaterCost = false;


    public Sprite unclickedSprite;
    public Sprite clickedSprite;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;

    private bool clicked1 = false;
    private bool clicked2 = false;
    private bool clicked3 = false;
    private bool clicked4 = false;
    private bool clicked5 = false;
    private bool clicked6 = false;

    public Image resultPanel;
    public TMP_Text actualResult;
    public TMP_Text testVerdict;

    public void StartButton()
    {
        StartCoroutine(BeginAll());
    }
    IEnumerator BeginAll()
    {
        yield return StartCoroutine(loopDelay(1));
        int[,] graph =
        {
            {0, 7, 0, 0, 0, 0, 11}, // vertex 0
			{7, 0, 9, 0, 0, 0, 15}, // vertex 1
			{0, 9, 0, 10, 0, 5, 0}, // vertex 2
			{0, 0, 10, 0, 14, 0, 0}, // vertex 3
			{0, 0, 0, 14, 0, 4, 5}, // vertex 4
            {0, 0, 5, 0, 4, 0, 10}, // vertex 5
            {11, 15, 0, 0, 5, 10, 0}, // vertex 6
		};
        StartCoroutine(Prim(graph, 7));
    }

    private int MinKey(int[] key, bool[] set, int verticesCount)
    {
        int min = int.MaxValue, minIndex = 0;

        for (int v = 0; v < verticesCount; ++v)
        {
            if(searchNotVisited)
            {
                if (set[v] == false && key[v] < min)
                {
                    min = key[v];
                    minIndex = v;
                }
            }
            else if(!searchNotVisited)
            {
                if (key[v] < min)
                {
                    min = key[v];
                    minIndex = v;
                }
            }
            
        }

        return minIndex;
    }

    IEnumerator Print(int[] parent, int[,] graph, int verticesCount)
    {
        if (!addMinKey || !searchNotVisited || !updateParent)
        {
            Debug.Log("Stop IF called");
            spriteRenderer = vertices[0].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(255, 0, 0);
            spriteRenderer = vertices[1].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(255, 0, 0);
            spriteRenderer = vertices[6].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(255, 0, 0);
            spriteRenderer = edgesObjects[0].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(0, 0, 255);
            spriteRenderer = edgesObjects[1].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(0, 0, 255);
            yield return StartCoroutine(loopDelay(1));
        }
        else
        {
            Debug.Log("Edge     Weight");
            for (int i = 1; i < verticesCount; ++i)
            {
                Debug.Log($"{parent[i]} - {i}    {graph[i, parent[i]]}");
                spriteRenderer = vertices[parent[i]].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(255, 0, 0);

                spriteRenderer = vertices[i].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(255, 0, 0);



                int correctIndex = -1;
                for (int q = 0; q < edgesObjects.Length; q++)
                {
                    EdgeScript edge = edgesObjects[q].GetComponent<EdgeScript>();
                    if (edge.source == parent[i] && edge.destination == i)
                    {
                        correctIndex = q;
                        break;
                    }
                    // When source and destinaton are swapped
                    else if (edge.source == i && edge.destination == parent[i])
                    {
                        correctIndex = q;
                        break;
                    }


                }
                spriteRenderer = edgesObjects[correctIndex].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(0, 0, 255);
                yield return StartCoroutine(loopDelay(1));
            }
        }
        yield return StartCoroutine(loopDelay(1));
        CallRestulPanel();
        
    }



    IEnumerator Prim(int[,] graph, int verticesCount)
    {
        EstablishActions();

        int[] parent = new int[verticesCount];
        int[] key = new int[verticesCount];
        bool[] mstSet = new bool[verticesCount];
        //int correctIndex = -1;

        for (int i = 0; i < verticesCount; ++i)
        {
            key[i] = int.MaxValue;
            mstSet[i] = false;
        }

        key[0] = 0;
        parent[0] = -1;

        int correctNumberVertices = verticesCount;
        if (edgesMinusVertices) correctNumberVertices = verticesCount - 1;

        Debug.Log("CorrectNumberVertices: " + correctNumberVertices);
        for (int count = 0; count < correctNumberVertices; ++count)
        {
            int u = MinKey(key, mstSet, verticesCount);
            if (addMinKey)
            {
                mstSet[u] = true;
            }

            for (int v = 0; v < verticesCount; ++v)
            {
                if (graph[u, v] != 0 && mstSet[v] == false && graph[u, v] < key[v])
                {
                    if(updateParent)
                    {
                        parent[v] = u;
                        key[v] = graph[u, v];
                    }
                    

                    Debug.Log($"u = {u}. v = {v}. parent[v] = {parent[v]}. key[v] = {key[v]}. graph[u, v] = {graph[u, v]}.");
                }
            }
        }
        yield return StartCoroutine(loopDelay(1));
        StartCoroutine(Print(parent, graph, verticesCount));
    }

    private void CallRestulPanel()
    {
        bool failure = false;
        resultPanel.gameObject.SetActive(true);
        if (!addMinKey || !searchNotVisited || !updateParent)
        {
            actualResult.text += "Nu s-a putut obtine arborele minim.\n";
            PlayerPrefs.SetString("primFeedback", "Nu s-a putut obtine arborele minim.");
            failure = true;
        }

        DateTime date = DateTime.Now;

        if (!failure)
        {
            actualResult.text = "Arborele obtinut <color=green>este minim</color>.";
            testVerdict.text = "Success";
            testVerdict.color = Color.green;
            PlayerPrefs.SetString("primTime", date.ToString());
            PlayerPrefs.SetString("primFeedback", "Arborele obtinut este minim.");
            PlayerPrefs.SetString("primStatus", "Success");
            PlayerPrefs.Save();
        }
        else
        {
            testVerdict.text = "Fail";
            testVerdict.color = Color.red;
            PlayerPrefs.SetString("primTime", date.ToString());
            PlayerPrefs.SetString("primStatus", "Fail");
            PlayerPrefs.Save();
        }
    }

    public void RestartButton()
    {

        // Reset Colors
        for (int i = 0; i < vertices.Length; i++)
        {
            spriteRenderer = vertices[i].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(255, 255, 255);
        }

        for (int i = 0; i < edgesObjects.Length; i++)
        {
            spriteRenderer = edgesObjects[i].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(0, 0, 0);
        }

        // Reset every variable
        addMinKey = false;
        searchNotVisited = false;
        edgesMinusVertices = false;
        updateParent = false;
        addGreaterCost = false;
        resultPanel.rectTransform.anchoredPosition = new Vector2(0f, 970f);
        resultPanel.gameObject.SetActive(false);
        actionList.Clear();
        actualResult.text = "";

        clicked1 = true;
        clicked2 = true;
        clicked3 = true;
        clicked4 = true;
        clicked5 = true;
        clicked6 = true;
        ChangeButtonSprite(button1, ref clicked1);
        ChangeButtonSprite(button2, ref clicked2);
        ChangeButtonSprite(button3, ref clicked3);
        ChangeButtonSprite(button4, ref clicked4);
        ChangeButtonSprite(button5, ref clicked5);
        ChangeButtonSprite(button6, ref clicked6);
    }

    public void FadePanelButton()
    {
        CanvasGroup canvasGroup = resultPanel.GetComponent<CanvasGroup>();
        if (canvasGroup.alpha == 1) canvasGroup.alpha = 0;
        else canvasGroup.alpha = 1;
    }



    private void EstablishActions()
    {
        for (int i = 0; i < actionList.Count; i++)
        {
            switch (actionList[i])
            {
                case "AddMin":
                    addMinKey = true;
                    break;
                case "SearchNON":
                    searchNotVisited = true;
                    break;
                case "SearchEvery":
                    searchNotVisited = false;
                    break;
                case "EdgesMinus":
                    edgesMinusVertices = true;
                    break;
                case "UpdateParent":
                    updateParent = true;
                    break;
                case "GreaterCost":
                    addGreaterCost = true;
                    break;
            }
        }
        Debug.Log("addMinKey " + addMinKey);
        Debug.Log("searchNotVisited: " + searchNotVisited);
        Debug.Log("edgesMinusVertices: " + edgesMinusVertices);
        Debug.Log("addGreaterCost: " + addGreaterCost);
        Debug.Log("updateParent: " + updateParent);
    }

    public void AddAction(string action)
    {
        actionList.Add(action);
    }

    public void AddMinVertexButton()
    {
        ChangeButtonSprite(button1, ref clicked1);
        AddAction("AddMin");
        
    }

    public void SearchNonVisitedVertexButton()
    {
        ChangeButtonSprite(button2, ref clicked2);
        AddAction("SearchNON");
        
    }

    public void SearchEveryVertexButtonButton()
    {
        ChangeButtonSprite(button3, ref clicked3);
        AddAction("SearchEvery");
        
    }

    public void EdgesMinusButton()
    {
        ChangeButtonSprite(button4, ref clicked4);
        AddAction("EdgesMinus");
        
    }

    public void GreaterCostButton()
    {
        ChangeButtonSprite(button5, ref clicked5);
        AddAction("GreaterCost");
        
    }

    public void UpdateParentButton()
    {
        ChangeButtonSprite(button6, ref clicked6);
        AddAction("UpdateParent");
        

    }



    private void ChangeButtonSprite(Button button, ref bool click)
    {
        click = !click;
        if (click)
        {
            button.image.sprite = clickedSprite;
        }
        else
        {
            button.image.sprite = unclickedSprite;
        }
    }

    IEnumerator loopDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
