using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public enum PuzzleType
    {
        Any,
        All,
        Order
    }

    public InteractionTrigger[] Triggers;
    public float AlertAmount = 0.0f;
    public PuzzleType Type;

    private void Awake()
    {

    }

}