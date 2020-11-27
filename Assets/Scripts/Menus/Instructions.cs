using System.Collections.Generic;
using UnityEngine;
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
        if (currentImageNum >= images.Count - 1)
            rightButton.SetActive(false);
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
        if(currentImageNum <= 0)
            leftButton.SetActive(false);
    }

    private void UpdateImage()
    {
        instructionImage.sprite = images[currentImageNum];
    }
}