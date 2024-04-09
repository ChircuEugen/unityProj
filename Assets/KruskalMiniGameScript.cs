using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class KruskalMiniGameScript : MonoBehaviour
{
    List<string> actionList = new List<string>();
    public Transform[] vertices;
    public Transform[] edgesObjects;
    private SpriteRenderer spriteRenderer;

    // variables for mini-game
    bool sortCosts = false;
    bool verifyCycle = false;
    bool edgesEqualVertices = false;
    bool edgesMinusVertices = false;
    bool addLeastCost = false;
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
        List<Edge> edges = new List<Edge>
        {
            new Edge(0, 1, 7),
            new Edge(0, 7, 11),
            new Edge(1, 2, 11),
            new Edge(1, 7, 14),
            new Edge(2, 3, 9),
            new Edge(2, 8, 5),
            new Edge(2, 5, 7),
            new Edge(3, 4, 12),
            new Edge(3, 5, 17),
            new Edge(4, 5, 13),
            new Edge(5, 6, 5),
            new Edge(6, 7, 4),
            new Edge(6, 8, 8),
            new Edge(7, 8, 9),
        };
        int numberOfVertices = 9;
        StartCoroutine(KruskalMST(edges, numberOfVertices));
        Debug.Log("Edges in the Minimum Spanning Tree:");
        yield return StartCoroutine(loopDelay(1.5f));
    }


    class Edge
    {
        public int Source;
        public int Destination;
        public int Weight;

        public Edge(int source, int destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }
    }
    int FindParent(int[] parent, int vertex)
    {
        if (parent[vertex] == -1)
            return vertex;
        return FindParent(parent, parent[vertex]);
    }
    void Union(int[] parent, int x, int y)
    {
        int xSet = FindParent(parent, x);
        int ySet = FindParent(parent, y);
        parent[xSet] = ySet;
    }

    IEnumerator KruskalMST(List<Edge> edges, int numberOfVertices)
    {
        EstablishActions();
        List<Edge> minimumSpanningTree = new List<Edge>();

        if(sortCosts) edges = edges.OrderBy(edge => edge.Weight).ToList(); // Sort Cost Button was pressed

        int[] parent = new int[numberOfVertices];
        for (int i = 0; i < numberOfVertices; i++)
            parent[i] = -1;
        int edgeCount = 0;
        int index = 0;

        int actualNumberOfVertices = 0;
        if (edgesMinusVertices) actualNumberOfVertices = numberOfVertices - 1;
        else if (edgesEqualVertices) actualNumberOfVertices = numberOfVertices;

        while (edgeCount < actualNumberOfVertices)
        {
            if (index == 14) break;
            int correctEdgeIndex = -2;
            Edge nextEdge = edges[index++];
            if(addGreaterCost) // Should be false for correct implementation
            {
                nextEdge = edges[14 - index];
            }

            // Find the correct edge to give to spriteRenderer.
            for (int q = 0; q < edgesObjects.Length; q++)
            {
                EdgeScript edgeObject = edgesObjects[q].GetComponent<EdgeScript>();
                if (edgeObject.source == nextEdge.Source && edgeObject.destination == nextEdge.Destination)
                {
                    correctEdgeIndex = q;
                    break;
                }
            }
            yield return StartCoroutine(loopDelay(1.5f));

            int x = FindParent(parent, nextEdge.Source);
            int y = FindParent(parent, nextEdge.Destination);
            if (x != y)
            {
                spriteRenderer = vertices[x].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(255, 0, 0);

                spriteRenderer = vertices[y].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(255, 0, 0);

                spriteRenderer = edgesObjects[correctEdgeIndex].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(0, 0, 255);

                EdgeScript markedEdge = edgesObjects[correctEdgeIndex].GetComponent<EdgeScript>();
                markedEdge.marked = true;

                minimumSpanningTree.Add(nextEdge);
                Union(parent, x, y);
                //yield return StartCoroutine(loopDelay());
                edgeCount++;
            }
            else if(x == y && !verifyCycle) // if Verify Cycle was not pressed
            {
                spriteRenderer = edgesObjects[correctEdgeIndex].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(255, 255, 0);
                yield return StartCoroutine(loopDelay(0.5f));
            }
            else if(x == y)
            {
                spriteRenderer = edgesObjects[correctEdgeIndex].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(255, 255, 0);
                yield return StartCoroutine(loopDelay(1.5f));
                spriteRenderer.color = new Color(0, 0, 0);
            }
        }
        yield return StartCoroutine(loopDelay(1.5f));
        CallRestulPanel();
        

    }

    // Turn on result panel
    private void CallRestulPanel()
    {
        bool failure = false;
        resultPanel.gameObject.SetActive(true);
        if (!sortCosts || addGreaterCost)
        {
            actualResult.text += "<b>Arborele</b> obtinut <color=red>nu este minim</color>.\n";
            PlayerPrefs.SetString("kruskalFeedback", "Arborele obtinut nu este minim.");
            failure = true;
        }
        if (!verifyCycle)
        {
            actualResult.text += "S-au depistat muchii care creeaza ciclu.\n";
            PlayerPrefs.SetString("kruskalFeedback", "S-au depistat muchii care creeaza ciclu.");
            failure = true;
        }
        if (edgesEqualVertices)
        {
            actualResult.text += "S-a depistat o iteratie infinita.\n";
            PlayerPrefs.SetString("kruskalFeedback", "S-a depistat o iteratie infinita.");
            failure = true;
        }

        DateTime date = DateTime.Now;

        if(!failure)
        {
            actualResult.text = "Arborele obtinut <color=green>este minim</color>.";
            testVerdict.text = "Success";
            testVerdict.color = Color.green;
            PlayerPrefs.SetString("kruskalTime", date.ToString());
            PlayerPrefs.SetString("kruskalFeedback", "Arborele obtinut este minim.");
            PlayerPrefs.SetString("kruskalStatus", "Success");
            PlayerPrefs.Save();

        }
        else
        {
            testVerdict.text = "Fail";
            testVerdict.color = Color.red;
            PlayerPrefs.SetString("kruskalTime", date.ToString());
            PlayerPrefs.SetString("kruskalStatus", "Fail");
            PlayerPrefs.Save();
        }
    }

    public void RestartButton()
    {

        // Reset Colors
        for(int i=0; i<vertices.Length; i++)
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
        sortCosts = false;
         verifyCycle = false;
         edgesEqualVertices = false;
         edgesMinusVertices = false;
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
        for(int i=0; i<actionList.Count; i++)
        {
            switch(actionList[i])
            {
                case "OrderCost":
                    sortCosts = true;
                    break;
                case "VerifyCycle":
                    verifyCycle = true;
                    break;
                case "EdgesEqual":
                    edgesEqualVertices = true;
                    break;
                case "EdgesMinus":
                    edgesMinusVertices = true;
                    break;
                case "LeastCost":
                    addLeastCost = true;
                    break;
                case "GreaterCost":
                    addGreaterCost = true;
                    break;
            }
        }
    }

    public void AddAction(string action)
    {
        actionList.Add(action);
    }

    public void OrderCostButton()
    {
        ChangeButtonSprite(button1, ref clicked1);
        AddAction("OrderCost");
    }

    public void VerifyCycleButton()
    {
        ChangeButtonSprite(button2, ref clicked2);
        AddAction("VerifyCycle");
    }

    public void EdgesEqualButton()
    {
        ChangeButtonSprite(button3, ref clicked3);
        AddAction("EdgesEqual");
    }

    public void EdgesMinusButton()
    {
        ChangeButtonSprite(button4, ref clicked4);
        AddAction("EdgesMinus");
    }
    public void LeastCostButton()
    {
        ChangeButtonSprite(button6, ref clicked6);
        AddAction("LeastCost");
    }

    public void GreaterCostButton()
    {
        ChangeButtonSprite(button5, ref clicked5);
        AddAction("GreaterCost");
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
