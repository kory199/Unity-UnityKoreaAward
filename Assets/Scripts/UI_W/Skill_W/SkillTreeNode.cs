using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeNode : MonoBehaviour
{
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetSprite(Sprite newSprite)
    {
        if(image != null)
        {
            image.sprite = newSprite;
        }
    }
}