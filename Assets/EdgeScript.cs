using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeScript : MonoBehaviour
{
    public int source;
    public int destination;
    public int cost;

    [SerializeField] public bool marked;
}
