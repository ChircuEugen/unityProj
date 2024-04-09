using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AutoMergeSort : MonoBehaviour
{
    public Transform[] cubes;
    public float moveDistance = 1f;
    public float moveSpeed = 2.5f;
    //private Vector2 nerfUp = new Vector2(0, -0.2f);

    private SpriteRenderer spriteRender;
    public Sprite initialSprite;
    public Sprite currentComparingSprite;



    IEnumerator SortCubes()
    {
        yield return StartCoroutine(loopDelay());
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
