﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArtifactDisplay : MonoBehaviour
{
    public Image buttonImage;
    public Image iconImage;
    public TextMeshProUGUI count;

    void Start()
    {
        
    }

    public void UpdateDisplay(Sprite image, int count)
    {
        if (count > 0)
        {
            this.iconImage.sprite = image;
        }
        if (count > 1)
        {
            this.count.text = count.ToString();
        }
        else
        {
            this.count.text = "";
        }
    }
}
