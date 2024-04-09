using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeSort : MonoBehaviour
{
    public Transform[] cubes;
    public float moveDistance = 1f;
    public float moveSpeed = 2.5f;
    private Vector2 nerfUp = new Vector2(0, -0.2f);

    private SpriteRenderer spriteRender;
    public Sprite initialSprite;
    public Sprite currentComparingSprite;

    public TMP_Text currentAction;
    public TMP_Text comparisonText;
    public TMP_Text swapText;

    private int comparisons = 0;
    private int swaps = 0;


    //void Start()
    //{
    //    StartCoroutine(SortCubes());
    //}

    public void StartButton()
    {
        StartCoroutine(SortCubes());
    }

    IEnumerator SortCubes()
    {
        for (int i = 0; i < cubes.Length - 1; i++)
        {
            for(int j=0; j<cubes.Length - i - 1; j++)
            {
                CubeScript currentCube = cubes[j].GetComponent<CubeScript>();
                CubeScript nextCube = cubes[j + 1].GetComponent<CubeScript>();

                spriteRender = cubes[j].GetComponent<SpriteRenderer>();
                spriteRender.sprite = currentComparingSprite;

                spriteRender = cubes[j+1].GetComponent<SpriteRenderer>();
                spriteRender.sprite = currentComparingSprite;

                UpdateText(currentCube.value, nextCube.value, comparisons++, swaps, "Se compara:");
                yield return StartCoroutine(loopDelay());


                if (currentCube.value > nextCube.value)
                {
                    swaps++;
                    UpdateText(currentCube.value, nextCube.value, comparisons, swaps, "Se permuta:");
                    yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.up + nerfUp));
                    //yield return StartCoroutine(loopDelay());

                    // Move left
                    yield return StartCoroutine(MoveCubeCoroutine(cubes[j + 1], Vector2.left));
                    //yield return StartCoroutine(loopDelay());

                    // Move right
                    yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.right));
                    //yield return StartCoroutine(loopDelay());

                    // Move down
                    yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.down - nerfUp));
                    //yield return StartCoroutine(loopDelay());


                    SwapCubes(j, j + 1);

                    //yield return StartCoroutine(loopDelay());
                }

                spriteRender = cubes[j].GetComponent<SpriteRenderer>();
                spriteRender.sprite = initialSprite;
                spriteRender = cubes[j + 1].GetComponent<SpriteRenderer>();
                spriteRender.sprite = initialSprite;

            }
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

    void UpdateText(int value1, int value2, int comparisonNumber, int swapNumber, string action)
    {
        currentAction.text = action + " " + value1.ToString() + " si " + value2.ToString();
        comparisonText.text = "Comparari: " + comparisonNumber.ToString();
        swapText.text = "Permutari: " + swapNumber.ToString();
    }

    IEnumerator loopDelay()
    {
        yield return new WaitForSeconds(2);
    }
}
