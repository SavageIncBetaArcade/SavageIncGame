using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    public Image instructionImage;
    public List<Sprite> images;
    public GameObject leftButton;
    public GameObject rightButton;
    private int currentImageNum;

    public void NextImage()
    {
        if (currentImageNum < images.Count - 1)
        {
            currentImageNum++;
            leftButton.SetActive(true);
            DisableRightButton();
        }
        UpdateImage();
    }

    private void DisableRightButton()
    {
        if (currentImageNum < images.Count - 1) return;
        rightButton.SetActive(false);
        EventSystem.current.SetSelectedGameObject(leftButton);
    }

    public void PreviousImage()
    {
        if (currentImageNum > 0)
        {
            currentImageNum--;
            rightButton.SetActive(true);
            DisableLeftButton();
        }
        UpdateImage();
    }

    private void DisableLeftButton()
    {
        if (currentImageNum > 0) return;
        leftButton.SetActive(false);
        EventSystem.current.SetSelectedGameObject(rightButton);
    }

    private void UpdateImage()
    {
        instructionImage.sprite = images[currentImageNum];
    }
}