using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MiniGameScript : MonoBehaviour
{
    List<string> actionList = new List<string>();
    public Transform[] cubes;
    int n = 1;
    bool swapIfGreater = false;
    bool swapIfLess = false;
    bool swapIfGreater2 = false;
    bool correctAnswer = false;

    public float moveDistance = 2.2f;
    public float moveSpeed = 3.5f;
    private Vector2 nerfUp = new Vector2(0, -0.2f);
    private Vector2 doubleDistance = new Vector2(2.2f, 0f);

    private SpriteRenderer spriteRender;
    public Sprite initialSprite;
    public Sprite currentComparingSprite;

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

    public Sprite unclickedSprite;
    public Sprite clickedSprite;

    public TMP_Text currentAction;
    public TMP_Text comparisonText;
    public TMP_Text swapText;

    private int comparisons = 0;
    private int swaps = 0;

    public Image resultPanel;
    public TMP_Text actualResult;
    public TMP_Text testVerdict;

    int expectedComparison = 15;
    int expectedSwaps = 4;

    private Vector2[] cubePositions = new Vector2[6];

    private void Start()
    {
        cubePositions[0] = new Vector2(-5.357361f, 0.3554688f);
        cubePositions[1] = new Vector2(-3.15741f, 0.3554688f);
        cubePositions[2] = new Vector2(-0.9573975f, 0.3554688f);
        cubePositions[3] = new Vector2(1.242615f, 0.3554688f);
        cubePositions[4] = new Vector2(3.442627f, 0.3554688f);
        cubePositions[5] = new Vector2(5.642517f, 0.3554688f);
    }

    public void EstablishActions()
    {
        //foreach (string action in actionList)
        for (int i = 0; i < actionList.Count; i++)
        {

            switch (actionList[i])
            {
                case "Loop n times":
                    n = cubes.Length;
                    break;

                case "Loop n-1 times":
                    n = cubes.Length - 1;
                    break;

                case "Loop n-2 times":
                    correctAnswer = true;
                    n = cubes.Length;
                    break;

                case "SwapIfGreat":
                    swapIfGreater = true;
                    break;

                case "SwapIfLess":
                    swapIfLess = true;
                    break;

                case "SwapIfGreat2":
                    swapIfGreater2 = true;
                    break;
            }
        }
    }

    public void StartButton()
    {
        StartCoroutine(SortCoroutine());
    }

    IEnumerator SortCoroutine()
    {
        EstablishActions();

        for (int i = 0; i < n; i++)
        {
            int correctLoop = n;
            if (correctAnswer) correctLoop = n - i - 1;
            for (int j = 0; j < correctLoop; j++)
            {
                CubeScript currentCube = cubes[j].GetComponent<CubeScript>();
                //if (j + 1 >= n) continue;
                CubeScript nextCube = cubes[j + 1].GetComponent<CubeScript>();

                spriteRender = cubes[j].GetComponent<SpriteRenderer>();
                spriteRender.sprite = currentComparingSprite;

                spriteRender = cubes[j + 1].GetComponent<SpriteRenderer>();
                spriteRender.sprite = currentComparingSprite;

                UpdateText(currentCube.value, nextCube.value, comparisons++, swaps, "Se compara:");
                yield return StartCoroutine(loopDelay());

                if (swapIfGreater)
                {
                    Debug.Log("SWAP IF GREATER");
                    if (currentCube.value > nextCube.value)
                    {
                        swaps++;
                        UpdateText(currentCube.value, nextCube.value, comparisons, swaps, "Se permuta:");

                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.up + nerfUp));                        

                        // Move left
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j + 1], Vector2.left));

                        // Move right
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.right));

                        // Move down
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.down - nerfUp));

                        SwapCubes(j, j + 1);
                        
                    }
                }
                else if(swapIfGreater2)
                {
                    Debug.Log("GREATER TWO");
                    spriteRender = cubes[j].GetComponent<SpriteRenderer>();
                    spriteRender.sprite = currentComparingSprite;

                    spriteRender = cubes[j + 1].GetComponent<SpriteRenderer>();
                    spriteRender.sprite = currentComparingSprite;

                    spriteRender = cubes[j + 2].GetComponent<SpriteRenderer>();
                    spriteRender.sprite = currentComparingSprite;

                    CubeScript secondNextCube = cubes[j + 2].GetComponent<CubeScript>();
                    if (currentCube.value > nextCube.value)
                    {
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.up + nerfUp));

                        // Move left
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j + 1], Vector2.left));

                        // Move right
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.right));

                        // Move down
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.down - nerfUp));

                        SwapCubes(j, j + 1);

                        //spriteRender = cubes[j].GetComponent<SpriteRenderer>();
                        //spriteRender.sprite = initialSprite;
                        //spriteRender = cubes[j + 1].GetComponent<SpriteRenderer>();
                        //spriteRender.sprite = initialSprite;
                    }
                    else if (currentCube.value > secondNextCube.value)
                    {
                        Debug.Log("GREATER TWO ELSE");
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.up + nerfUp));

                        // Move left
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j + 1], Vector2.left + doubleDistance));

                        // Move right
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.right - doubleDistance));

                        // Move down
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.down - nerfUp));

                        SwapCubes(j, j + 2);

                        //spriteRender = cubes[j].GetComponent<SpriteRenderer>();
                        //spriteRender.sprite = initialSprite;
                        //spriteRender = cubes[j + 2].GetComponent<SpriteRenderer>();
                        //spriteRender.sprite = initialSprite;
                    }
                }
                else if (swapIfLess)
                {
                    Debug.Log("SWAP IF LESS");
                    //spriteRender = cubes[j].GetComponent<SpriteRenderer>();
                    //spriteRender.sprite = currentComparingSprite;

                    //spriteRender = cubes[j + 1].GetComponent<SpriteRenderer>();
                    //spriteRender.sprite = currentComparingSprite;

                    if (currentCube.value < nextCube.value)
                    {
                        swaps++;
                        UpdateText(currentCube.value, nextCube.value, comparisons, swaps, "Se permuta:");

                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.up + nerfUp));

                        // Move left
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j + 1], Vector2.left));

                        // Move right
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.right));

                        // Move down
                        yield return StartCoroutine(MoveCubeCoroutine(cubes[j], Vector2.down - nerfUp));


                        SwapCubes(j, j + 1);

                        //spriteRender = cubes[j].GetComponent<SpriteRenderer>();
                        //spriteRender.sprite = initialSprite;
                        //spriteRender = cubes[j + 1].GetComponent<SpriteRenderer>();
                        //spriteRender.sprite = initialSprite;
                    }
                }
                else
                {
                    Debug.Log("Text");
                }

                spriteRender = cubes[j].GetComponent<SpriteRenderer>();
                spriteRender.sprite = initialSprite;
                spriteRender = cubes[j + 1].GetComponent<SpriteRenderer>();
                spriteRender.sprite = initialSprite;
            }
        }

        currentAction.gameObject.SetActive(false);
        comparisonText.gameObject.SetActive(false);
        swapText.gameObject.SetActive(false);

        resultPanel.gameObject.SetActive(true);
        actualResult.text = "Numarul de comparari obtinut: " + comparisons.ToString() + "\nNumarul de permutari obtinut: " + swaps.ToString();
        if(comparisons > expectedComparison || swaps > expectedSwaps)
        {
            testVerdict.text = "Fail";
            testVerdict.color = Color.red;
            DateTime date = DateTime.Now;
            PlayerPrefs.SetString("bubbleTime", date.ToString());
            PlayerPrefs.SetString("bubbleStatus", "Fail");
            PlayerPrefs.SetString("bubbleFeedback", "Prea multe comparari/permutari");
            PlayerPrefs.Save();
        }
        else
        {
            testVerdict.text = "Success";
            testVerdict.color = Color.green;
            DateTime date = DateTime.Now;
            PlayerPrefs.SetString("bubbleTime", date.ToString());
            PlayerPrefs.SetString("bubbleStatus", "Success");
            PlayerPrefs.SetString("bubbleFeedback", "Destule comparari/permutari");
            PlayerPrefs.Save();
        }
    }

    public void AddAction(string action)
    {
        actionList.Add(action);
    }

    public void AddNTimesLoopButton()
    {
        ChangeButtonSprite(button1, ref clicked1);
        AddAction("Loop n times");

    }

    public void AddNMinus1imesLoopButton()
    {
        ChangeButtonSprite(button2, ref clicked2);
        AddAction("Loop n-1 times");
    }

    public void AddNMinus2TimesLoopButton()
    {
        ChangeButtonSprite(button3, ref clicked3);
        AddAction("Loop n-2 times");
    }

    public void SwapIfGreaterButton()
    {
        ChangeButtonSprite(button4, ref clicked4);
        AddAction("SwapIfGreat");
    }

    public void SwapIfLessButton()
    {
        ChangeButtonSprite(button5, ref clicked5);
        AddAction("SwapIfLess");
    }

    public void SwapIfGreat2Button()
    {
        ChangeButtonSprite(button6, ref clicked6);
        AddAction("SwapIfGreat2");

        for(int i=0; i<actionList.Count; i++)
        {
            Debug.Log(actionList[i]);
        }
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

    public void RestartButton()
    {
        for(int i=0; i<cubes.Length; i++)
        {
            for(int j=0; j<cubes.Length; j++)
            {
                CubeScript currentCube = cubes[i].GetComponent<CubeScript>();
                Transform tmpPosition = cubes[i].transform;
                cubes[i].transform.position = cubePositions[currentCube.initialIndex];
                cubes[currentCube.initialIndex].transform.position = tmpPosition.position;
                SwapCubes(i, currentCube.initialIndex);
            }
            
        }

        resultPanel.rectTransform.anchoredPosition = new Vector2(430f, 974f);
        resultPanel.gameObject.SetActive(false);
        actionList.Clear();

        swaps = 0;
        comparisons = 0;
        swapIfGreater = false;
        swapIfLess = false;
        swapIfGreater2 = false;
        correctAnswer = false;
        n = 1;
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

    IEnumerator loopDelay()
    {
        yield return new WaitForSeconds(1);
    }

}
