using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SelectionMiniGameScript : MonoBehaviour
{
    List<string> actionList = new List<string>();
    public Transform[] cubes;
    int n = 1;
    bool swapFirstLess = false;
    bool swapFirstGreater = false;
    bool swapCorrect = false;

    private int smallest;

    public float moveDistance = 2.2f;
    public float moveSpeed = 5f;
    //private Vector2 nerfUp = new Vector2(0, -0.2f);
    //private Vector2 doubleDistance = new Vector2(2.2f, 0f);

    private SpriteRenderer spriteRender;
    public Sprite defaultSprite;
    public Sprite minSprite;

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

    public TMP_Text currentElement;
    public TMP_Text currentMin;
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

    public void StartButton()
    {
        StartCoroutine(SortCoroutine());
    }

    IEnumerator SortCoroutine()
    {
        EstablishActions();

        for (int i = 0; i < cubes.Length - 1; i++)
        {
            smallest = i;
            CubeScript tempCube = cubes[i].GetComponent<CubeScript>();
            int tempCurrentValue = 0;
            int tempSmallestValue = 0;

            
            if(swapCorrect) // Correct Choice Begin
            {
                for (int j = i + 1; j < n; j++)
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
                for (int k = 0; k < cubes.Length; k++)
                {
                    if (k != i && k != smallest)
                    {
                        spriteRender = cubes[k].GetComponent<SpriteRenderer>();
                        spriteRender.sprite = defaultSprite;
                        spriteRender.color = new Color(255, 255, 255);
                    }
                }

                if (i != smallest)
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

            } // Correct Choice End
            
            else if(swapFirstLess) // Swap First Less Begin
            {
                for (int j = i + 1; j < n; j++)
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

                        smallest = j;
                        smallestCube = cubes[smallest].GetComponent<CubeScript>();
                        spriteRender = cubes[smallest].GetComponent<SpriteRenderer>();
                        spriteRender.sprite = minSprite;
                        tempCurrentValue = currentCube.value;
                        tempSmallestValue = smallestCube.value;

                        UpdateText(tempCube.value, smallestCube.value, currentCube.value, comparisons, swaps, "Se compara:");

                        if (i != smallest)
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

                        for (int k = 0; k < cubes.Length; k++)
                        {
                            if (k != i && k != smallest)
                            {
                                spriteRender = cubes[k].GetComponent<SpriteRenderer>();
                                spriteRender.sprite = defaultSprite;
                                spriteRender.color = new Color(255, 255, 255);
                            }
                        }

                        // Restore remainding elements to initial sprite
                        spriteRender = cubes[i].GetComponent<SpriteRenderer>();
                        spriteRender.sprite = defaultSprite;
                        spriteRender.color = new Color(255, 255, 255);

                        spriteRender = cubes[smallest].GetComponent<SpriteRenderer>();
                        spriteRender.sprite = defaultSprite;
                        spriteRender.color = new Color(255, 255, 255);


                        //UpdateText(tempCube.value, smallestCube.value, currentCube.value, comparisons, swaps, "Se compara:");
                        //yield return StartCoroutine(loopDelay());
                        break;
                    }
                }
            } // Swap First Less End

            else if (swapFirstGreater) // Swap First Great Begin
            {
                for (int j = i + 1; j < n; j++)
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

                    if (currentCube.value > smallestCube.value)
                    {

                        smallest = j;
                        smallestCube = cubes[smallest].GetComponent<CubeScript>();
                        spriteRender = cubes[smallest].GetComponent<SpriteRenderer>();
                        spriteRender.sprite = minSprite;
                        tempCurrentValue = currentCube.value;
                        tempSmallestValue = smallestCube.value;

                        UpdateText(tempCube.value, smallestCube.value, currentCube.value, comparisons++, swaps, "Se compara:");
                        yield return StartCoroutine(loopDelay());

                        if (i != smallest)
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

                        for (int k = 0; k < cubes.Length; k++)
                        {
                            if (k != i && k != smallest)
                            {
                                spriteRender = cubes[k].GetComponent<SpriteRenderer>();
                                spriteRender.sprite = defaultSprite;
                                spriteRender.color = new Color(255, 255, 255);
                            }
                        }

                        // Restore remainding elements to initial sprite
                        spriteRender = cubes[i].GetComponent<SpriteRenderer>();
                        spriteRender.sprite = defaultSprite;
                        spriteRender.color = new Color(255, 255, 255);

                        spriteRender = cubes[smallest].GetComponent<SpriteRenderer>();
                        spriteRender.sprite = defaultSprite;
                        spriteRender.color = new Color(255, 255, 255);


                        UpdateText(tempCube.value, smallestCube.value, currentCube.value, comparisons, swaps, "Se compara:");
                        yield return StartCoroutine(loopDelay());
                        break;
                    }
                }
            } // Swap First Great End

        }

        currentElement.gameObject.SetActive(false);
        currentMin.gameObject.SetActive(false);
        currentAction.gameObject.SetActive(false);
        comparisonText.gameObject.SetActive(false);
        swapText.gameObject.SetActive(false);

        resultPanel.gameObject.SetActive(true);
        actualResult.text = "Numarul de comparari obtinut: " + comparisons.ToString() + "\nNumarul de permutari obtinut: " + swaps.ToString();
        if (comparisons > expectedComparison || swaps > expectedSwaps || swapFirstGreater || swapFirstLess)
        {
            testVerdict.text = "Fail";
            testVerdict.color = Color.red;
            DateTime date = DateTime.Now;
            PlayerPrefs.SetString("selectionTime", date.ToString());
            PlayerPrefs.SetString("selectionStatus", "Fail");
            PlayerPrefs.SetString("selectionFeedback", "Prea multe comparari/permutari");
            PlayerPrefs.Save();
        }
        else
        {
            testVerdict.text = "Success";
            testVerdict.color = Color.green;
            DateTime date = DateTime.Now;
            PlayerPrefs.SetString("selectionTime", date.ToString());
            PlayerPrefs.SetString("selectionStatus", "Success");
            PlayerPrefs.SetString("selectionFeedback", "Destule comparari/permutari");
            PlayerPrefs.Save();
        }
    }

    public void EstablishActions()
    {
        //foreach (string action in actionList)
        for (int i = 0; i < actionList.Count; i++)
        {

            switch (actionList[i])
            {
                case "LoopIncreaseNTimes":
                    n = cubes.Length;
                    break;
                case "LoopIncreaseN-1Times":
                    n = cubes.Length - 1;
                    break;
                case "LoopNTimes":
                    Debug.Log(":)");
                    break;
                case "SwapFirstLess":
                    swapFirstLess = true;
                    break;
                case "SwapFirstGreater":
                    swapFirstGreater = true;
                    break;
                case "SwapCorrect":
                    swapCorrect = true;
                    break;
            }
        }
    }

    public void AddAction(string action)
    {
        actionList.Add(action);
    }

    public void AddLoopIncreaseNButton()
    {
        ChangeButtonSprite(button1, ref clicked1);
        AddAction("LoopIncreaseNTimes");

    }

    public void AddLoopIncreaseNMinus1Button()
    {
        ChangeButtonSprite(button2, ref clicked2);
        AddAction("LoopIncreaseN-1Times");
    }

    public void AddLoopNButton()
    {
        ChangeButtonSprite(button3, ref clicked3);
        AddAction("LoopNTimes");
    }

    public void SwapFirstLessButton()
    {
        ChangeButtonSprite(button4, ref clicked4);
        AddAction("SwapFirstLess");
    }

    public void SwapFirstGreaterButton()
    {
        ChangeButtonSprite(button5, ref clicked5);
        AddAction("SwapFirstGreater");
    }

    public void SwapCorrectButton()
    {
        ChangeButtonSprite(button6, ref clicked6);
        AddAction("SwapCorrect");
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

    void UpdateText(int nowElement, int nowMinimal, int numberCompared, int comparisonNumber, int swapNumber, string action)
    {
        currentElement.text = "Elementul curent: " + nowElement.ToString();
        currentMin.text = "Minimul curent: " + nowMinimal.ToString();
        currentAction.text = action + " " + nowElement.ToString() + " si " + numberCompared.ToString();
        comparisonText.text = "Comparari: " + comparisonNumber.ToString();
        swapText.text = "Permutari: " + swapNumber.ToString();
    }

    public void RestartButton()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            for (int j = 0; j < cubes.Length; j++)
            {
                CubeScript currentCube = cubes[i].GetComponent<CubeScript>();
                Transform tmpPosition = cubes[i].transform;
                cubes[i].transform.position = cubePositions[currentCube.initialIndex];
                cubes[currentCube.initialIndex].transform.position = tmpPosition.position;
                SwapCubes(i, currentCube.initialIndex);
            }

        }

        resultPanel.rectTransform.anchoredPosition = new Vector2(0f, 970f);
        resultPanel.gameObject.SetActive(false);
        actionList.Clear();

        swaps = 0;
        comparisons = 0;
        swapFirstLess = false;
        swapFirstGreater = false;
        swapCorrect = false;
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

    IEnumerator loopDelay()
    {
        yield return new WaitForSeconds(2);
    }

}
