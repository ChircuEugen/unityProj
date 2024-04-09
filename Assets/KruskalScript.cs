using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class KruskalScript : MonoBehaviour
{
    public Transform[] vertices;
    public Transform[] edgesObjects;
    public TMP_Text[] edgesCostsText;
    private SpriteRenderer spriteRenderer;


    public void StartButton()
    {
        StartCoroutine(BeginAll());
    }



    IEnumerator BeginAll()
    {
        List<Edge> edges = new List<Edge>
        {
            new Edge(0, 1, 10),
            new Edge(0, 2, 5),
            new Edge(0, 4, 6),
            new Edge(1, 3, 9),
            new Edge(1, 4, 5),
            new Edge(2, 3, 5),
            new Edge(2, 4, 3),
            new Edge(3, 4, 4),
        };
        int numberOfVertices = 5;
        StartCoroutine(KruskalMST(edges, numberOfVertices));        
        yield return StartCoroutine(loopDelay());
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
    IEnumerator Union(int[] parent, int x, int y)
    {
        int xSet = FindParent(parent, x);
        int ySet = FindParent(parent, y);
        parent[xSet] = ySet;
        yield return StartCoroutine(loopDelay());
    }
    IEnumerator KruskalMST(List<Edge> edges, int numberOfVertices)
    {
        List<Edge> minimumSpanningTree = new List<Edge>();
        edges = edges.OrderBy(edge => edge.Weight).ToList();
        int[] parent = new int[numberOfVertices];
        for (int i = 0; i < numberOfVertices; i++)
            parent[i] = -1;
        int edgeCount = 0;
        int index = 0;
        while (edgeCount < numberOfVertices - 1)
        {
            int correctEdgeIndex = -2;            
            Edge nextEdge = edges[index++];

            // Find the correct edge to give to spriteRenderer.
            for(int q=0; q<edgesObjects.Length; q++)
            {
                EdgeScript edgeObject = edgesObjects[q].GetComponent<EdgeScript>();
                if(edgeObject.source == nextEdge.Source && edgeObject.destination == nextEdge.Destination)
                {
                    correctEdgeIndex = q;
                    break;
                }
            }
            yield return StartCoroutine(loopDelay());

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
                StartCoroutine(Union(parent, x, y));
                edgeCount++;
            }
        }
        yield return StartCoroutine(loopDelay());

        foreach (var edge in minimumSpanningTree)
        {
            Debug.Log($"{edge.Source} - {edge.Destination} (Weight: {edge.Weight})");
        }

        // Turn off edges that are not used
        for(int q=0; q<edgesObjects.Length; q++)
        {
            EdgeScript markedEdge = edgesObjects[q].GetComponent<EdgeScript>();
            if(!markedEdge.marked)
            {
                spriteRenderer = edgesObjects[q].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(0, 0, 0, 0);
            }
        }
        // index: 0, 2, 3, 5
        // zero to one
        // one to three
        // zero to four
        // two to three
        for(int q=0; q<edgesCostsText.Length; q++)
        {
            edgesCostsText[q].gameObject.SetActive(false);
        }
    }

    IEnumerator loopDelay()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
