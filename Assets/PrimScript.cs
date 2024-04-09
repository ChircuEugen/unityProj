using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimScript : MonoBehaviour
{
    public Transform[] vertices;
    public Transform[] edgesObjects;
    //public TMP_Text[] edgesCostsText;
    private SpriteRenderer spriteRenderer;

    public void StartButton()
    {
		StartCoroutine(BeginAll());
    }
    IEnumerator BeginAll()
    {
		yield return StartCoroutine(loopDelay());
		int[,] graph =
		{
			{0, 9, 7, 0, 0}, // vertex 0
			{9, 0, 6, 0, 0 }, // vertex 1
			{7, 6, 0, 7, 10}, // vertex 2
			{0, 0, 7, 0, 8}, // vertex 3
			{0, 0, 10, 8, 0}, // vertex 4
		};
        StartCoroutine(Prim(graph, 5));
    }

	private int MinKey(int[] key, bool[] set, int verticesCount)
	{
		int min = int.MaxValue, minIndex = 0;

		for (int v = 0; v < verticesCount; ++v)
		{
			if (set[v] == false && key[v] < min)
			{
				min = key[v];
				minIndex = v;
			}
		}

		return minIndex;
	}

	IEnumerator Print(int[] parent, int[,] graph, int verticesCount)
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

			Debug.Log(correctIndex);
			spriteRenderer = edgesObjects[correctIndex].GetComponent<SpriteRenderer>();
			spriteRenderer.color = new Color(0, 0, 255);
			yield return StartCoroutine(loopDelay());
		}
	}

	IEnumerator Prim(int[,] graph, int verticesCount)
	{
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

		for (int count = 0; count < verticesCount - 1; ++count)
		{
			int u = MinKey(key, mstSet, verticesCount);
			mstSet[u] = true;

			for (int v = 0; v < verticesCount; ++v)
			{
				if (graph[u, v] != 0 && mstSet[v] == false && graph[u, v] < key[v])
				{
					parent[v] = u;
					key[v] = graph[u, v];

					//for(int q=0; q<edgesObjects.Length; q++)
     //               {
					//	EdgeScript edge = edgesObjects[q].GetComponent<EdgeScript>();
					//	if(edge.source == u && edge.destination == v && edge.cost == graph[u,v])
     //                   {
					//		correctIndex = q;
					//		break;
     //                   }
     //               }
					//spriteRenderer = vertices[u].GetComponent<SpriteRenderer>();
					//spriteRenderer.color = new Color(255, 0, 0);

					//spriteRenderer = vertices[v].GetComponent<SpriteRenderer>();
					//spriteRenderer.color = new Color(255, 0, 0);

					//spriteRenderer = edgesObjects[correctIndex].GetComponent<SpriteRenderer>();
					//spriteRenderer.color = new Color(0, 0, 255);
					Debug.Log($"u = {u}. v = {v}. parent[v] = {parent[v]}. key[v] = {key[v]}. graph[u, v] = {graph[u, v]}.");
				}
			}
		}
		yield return StartCoroutine(loopDelay());
		StartCoroutine(Print(parent, graph, verticesCount));
    }

	IEnumerator loopDelay()
	{
		yield return new WaitForSeconds(1.5f);
	}
}
