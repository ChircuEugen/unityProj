using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrimDetailScript : MonoBehaviour
{
	public Transform[] vertices;
	public Transform[] edgesObjects;
	public TMP_Text informativeText;
	private SpriteRenderer spriteRenderer;

	private int[] correctMinKeys = { 0, 1, 2, 4, 5 };
	private int[] correctKeys = { 0, 9, 8, 10, 7, 9 };

	public void OnEnable()
	{
		StartCoroutine(BeginAll());
	}
	IEnumerator BeginAll()
	{
		yield return StartCoroutine(loopDelay());
		int[,] graph =
		{
			{0, 9, 11, 14, 0, 0}, // vertex 0
			{9, 0, 8, 0, 10, 0}, // vertex 1
			{11, 8, 0, 15, 7, 9}, // vertex 2
			{14, 0, 15, 0, 0, 10}, // vertex 3
			{0, 10, 7, 0, 0, 14}, // vertex 4
			{0, 0, 9, 10, 14, 0}, // vertex 5
		};
		StartCoroutine(Prim(graph, 6));
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
		for (int i = 1; i < verticesCount; ++i)
		{
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
			UpdateText(i - 1);
			//if (i == 5) UpdateText(i);
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
				}
			}
		}
		yield return StartCoroutine(loopDelay());
		StartCoroutine(Print(parent, graph, verticesCount));
	}

    private void UpdateText(int index)
    {
		//if (index == 5) return;
		if (index == 3) informativeText.text += $"Varful ales: 3     key[{index}] = {correctKeys[index]}\n";
		else if (index == 4) informativeText.text += $"Varful ales: 4     key[{index}] = {correctKeys[index]}\n";
		else informativeText.text += $"Varful ales: {correctMinKeys[index]}     key[{index}] = {correctKeys[index]}\n";
    }

    IEnumerator loopDelay()
	{
		yield return new WaitForSeconds(1.5f);
	}

    private void OnDisable()
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
		informativeText.text = "";
	}
}
