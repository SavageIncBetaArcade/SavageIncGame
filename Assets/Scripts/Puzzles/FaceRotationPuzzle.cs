using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FaceRotationPuzzle : InteractionTrigger
{
    public Vector3 RotationAxis = Vector3.up;
    public Vector3 ExpectedRotation;
    public int NumberOfFaces;
    public float RotateSpeed = 5.0f;

    public RaycastInteractionTrigger[] PuzzleObjects;

    private float rotateAmount;
    private Coroutine coroutine;

    protected override void Awake()
    {
        base.Awake();

        rotateAmount = 360.0f / NumberOfFaces;
        foreach (var puzzleObject in PuzzleObjects)
        {
            puzzleObject.OnTrigger += RotateObject;
        }
    }   

    private void OnDestroy()
    {
        foreach (var puzzleObject in PuzzleObjects)
        {
            puzzleObject.OnTrigger -= RotateObject;
        }
    }

    void RotateObject(bool triggered, InteractionTrigger trigger)
    {
        if(coroutine == null)
            coroutine = StartCoroutine(rotate(trigger.transform));
    }

    IEnumerator rotate(Transform transform)
    {
        float t = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(RotationAxis * rotateAmount);

        while (t < 1.0f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            t += Time.deltaTime * RotateSpeed;
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Lerp(startRotation, endRotation, 1.0f);

        //check if all objects rotations meet the expected rotation
        bool completed = true;
        foreach (var puzzleObject in PuzzleObjects)
        {
            Vector3 rotation = Vector3.Scale(puzzleObject.transform.rotation.eulerAngles, RotationAxis);
            if (rotation != ExpectedRotation)
            {
                completed = false;
                break;
            }

        }
        if (completed)
        {
            Interact();
        }

        coroutine = null;
    } 
}