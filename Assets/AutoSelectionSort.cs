using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoSelectionSort : MonoBehaviour
{
    public Transform[] cubes;
    public float moveDistance = 2f;
    public float moveSpeed = 4f;
    //private Vector2 nerfUp = new Vector2(0, -0.2f);

    private SpriteRenderer spriteRender;
    public Sprite minSprite;
    public Sprite defaultSprite;

    private int smallest;

    public TMP_Text currentElement;
    public TMP_Text currentMin;
    public TMP_Text currentAction;
    public TMP_Text comparisonText;
    public TMP_Text swapText;

    private int comparisons = 0;
    private int swaps = 0;

    public void StartButton()
    {
        StartCoroutine(SortCubes());
    }


    IEnumerator SortCubes()
    {
        for (int i = 0; i < cubes.Length - 1; i++)
        {
            smallest = i;
            CubeScript tempCube = cubes[i].GetComponent<CubeScript>();
            int tempCurrentValue = 0;
            int tempSmallestValue = 0;

            // Find the index of the smallest element in the unsorted part of the array
            for (int j = i + 1; j < cubes.Length; j++)
            {
                CubeScript currentCube = cubes[j].GetComponent<CubeScript>();
                spriteRender = cubes[smallest].GetComponent<SpriteRenderer>();
                spriteRender.sprite = minSprite;

                CubeScript smallestCube = cubes[smallest].GetComponent<CubeScript>();
                spriteRender = cubes[j].GetComponent<SpriteRenderer>();
                spriteRender.sprite = minSprite;
                spriteRender.color = new Color(255, 0, 0);

                

                UpdateText(tempCube.value, smallestCube.value, currentCube.value, comparisons++, swaps, "Se compara:");
                yield return StartCoroutine(loopDelay());


                if (currentCube.value < smallestCube.value)
                {
                    Debug.Log("SE COMPARA CURRENT CU SMALLEST");
                    smallest = j;
                    smallestCube = cubes[smallest].GetComponent<CubeScript>();
                    spriteRender = cubes[smallest].GetComponent<SpriteRenderer>();
                    spriteRender.sprite = minSprite;
                    tempCurrentValue = currentCube.value;
                    tempSmallestValue = smallestCube.value;
                    
                    UpdateText(tempCube.value, smallestCube.value, currentCube.value, comparisons, swaps, "Se compara:");
                    //yield return StartCoroutine(loopDelay());
                }
            }

            // Restoring initial sprite to all elements that are not involved in swap that follows
            for(int k=0; k<cubes.Length; k++)
            {
                if(k != i && k != smallest)
                {
                    spriteRender = cubes[k].GetComponent<SpriteRenderer>();
                    spriteRender.sprite = defaultSprite;
                    spriteRender.color = new Color(255, 255, 255);
                }
            }

            if(i != smallest)
            {
                swaps++;
                UpdateText(tempCube.value, tempSmallestValue, tempCurrentValue, comparisons, swaps, "Se permuta:");

                yield return StartCoroutine(MoveCubeCoroutine(cubes[i], Vector2.up));
                yield return StartCoroutine(MoveCubeCoroutine(cubes[smallest], Vector2.down));
                yield return StartCoroutine(MoveCubeCoroutine(cubes[i], Vector2.right * (smallest - i)));
                yield return StartCoroutine(MoveCubeCoroutine(cubes[smallest], Vector2.left * (smallest - i)));
                yield return StartCoroutine(MoveCubeCoroutine(cubes[i], Vector2.down));
                yield return StartCoroutine(MoveCubeCoroutine(cubes[smallest], Vector2.up));


                SwapCubes(i, smallest);
            }

            // Restore remainding elements to initial sprite
            spriteRender = cubes[i].GetComponent<SpriteRenderer>();
            spriteRender.sprite = defaultSprite;
            spriteRender.color = new Color(255, 255, 255);

            spriteRender = cubes[smallest].GetComponent<SpriteRenderer>();
            spriteRender.sprite = defaultSprite;
            spriteRender.color = new Color(255, 255, 255);

        }
    }

    IEnumerator MoveCubeCoroutine(Transform cube, Vector3 direction)
    {
        Vector2 targetPosition = cube.position + direction * moveDistance;
        while (Vector2.Distance(cube.position, targetPosition) > 0.01f)
        {
            cube.position = Vector2.MoveTowards(cube.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void SwapCubes(int index1, int index2)
    {
        Transform temp = cubes[index1];
        cubes[index1] = cubes[index2];
        cubes[index2] = temp;
    }

    void UpdateText(int nowElement, int nowMinimal, int numberCompared, int comparisonNumber, int swapNumber, string action)
    {
        currentElement.text = "Elementul curent: " + nowElement.ToString();
        currentMin.text = "Minimul curent: " + nowMinimal.ToString();
        currentAction.text = action + " " + nowElement.ToString() + " si " + numberCompared.ToString();
        comparisonText.text = "Comparari: " + comparisonNumber.ToString();
        swapText.text = "Permutari: " + swapNumber.ToString();
    }

    IEnumerator loopDelay()
    {
        yield return new WaitForSeconds(2);
    }
}




//yield return StartCoroutine(MoveCubeCoroutine(cubes[0], Vector2.up));
//yield return StartCoroutine(MoveCubeCoroutine(cubes[3], Vector2.down));
//yield return StartCoroutine(MoveCubeCoroutine(cubes[0], Vector2.right * 3));
//yield return StartCoroutine(MoveCubeCoroutine(cubes[3], Vector2.left * 3));
//yield return StartCoroutine(MoveCubeCoroutine(cubes[0], Vector2.down));
//yield return StartCoroutine(MoveCubeCoroutine(cubes[3], Vector2.up));