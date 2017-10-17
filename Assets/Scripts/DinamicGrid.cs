using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinamicGrid : MonoBehaviour
{
    public float bestFillSize = 2;

    float countElement;
    public RectTransform parent;
    GridLayoutGroup grid;
     
    void Start ()
    { 
        grid = gameObject.GetComponent<GridLayoutGroup>();
         
        grid.cellSize = new Vector2(parent.rect.height / bestFillSize, parent.rect.height / bestFillSize);
        
        //Динамическое задание ширины для ShopList
        countElement = grid.GetComponentsInChildren<LayoutElement>().Length - 1;
        if (countElement % 2 == 0)
        {
            countElement /= 2;
        }
        else
        {
              countElement = countElement / 2 + 0.5f;
        }

        float rightOffset = countElement * (parent.rect.height / bestFillSize) + (countElement * grid.spacing.x); 
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (rightOffset, parent.rect.height);

    } 
}
