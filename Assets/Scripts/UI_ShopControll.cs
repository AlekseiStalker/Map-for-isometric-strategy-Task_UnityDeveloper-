using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopControll : MonoBehaviour
{ 
    public GameObject shopPanel;
    public GameObject[] gameObjects;
    public Transform BuyObjects; 
     
    bool dropCurObject = true;
    RaycastHit hit;
    float rayCastLength = 1000;

    CameraDrag cameraGrag;

    Dictionary<Vector3, bool> objectOnGrid = new Dictionary<Vector3, bool>(); //список для хранения координат "купленных" объектов
     
    private void Start()
    {
        cameraGrag = Camera.main.GetComponent<CameraDrag>();//для блокировки передвижения карты
        shopPanel.SetActive(false);
        InitializeGridList();
    }

    void InitializeGridList()
    {
        for (float i = 8.5f; i < GridOverlay.instanse.gridSizeX; i++)
        {
            for (float j = 8.5f; j < GridOverlay.instanse.gridSizeZ; j++)
            {
                objectOnGrid.Add(new Vector3(i, 0f, j), false);
            }
        }
    }
     
    public void OnObjectStoreClick(int objNum) //нажав на объект в магазине, включается grid и инициализируется новый объект
    {
        shopPanel.SetActive(false); 
        dropCurObject = true;

        cameraGrag.enabled = true; 
        GridOverlay.instanse.GridEnabled(true);

        StartCoroutine(PutNewObject(objNum));
    }

    IEnumerator PutNewObject(int objNumber)
    {
        GameObject newObject = Instantiate(gameObjects[objNumber], hit.point, Quaternion.identity) as GameObject;

        while (dropCurObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, rayCastLength))
            { 
                if (hit.collider.name == "Plane")
                {
                    newObject.transform.position = new Vector3(hit.point.x, 0f, hit.point.z);
                }
                if (Input.GetMouseButtonDown(1))
                {  
                    float pos_X = Mathf.Round(newObject.transform.position.x) + 0.5f; //добавление 0.5 для центра клетки
                    float pos_Z = Mathf.Round(newObject.transform.position.z) + 0.5f;
                    Vector3 objectPosition = new Vector3(pos_X, 0f, pos_Z);
                    
                    if (CheckGridPosition(objectPosition))
                    {
                        dropCurObject = false;
                        newObject.transform.position = objectPosition;
                         
                        newObject.transform.SetParent(BuyObjects);
                        newObject.name = "NewObject";
                    } 
                }
            }
            yield return new WaitForFixedUpdate();
        }
        GridOverlay.instanse.GridEnabled(false);
    }

    public void ViewShopClick()
    {
        shopPanel.SetActive(true);
        cameraGrag.enabled = false; 
    }

    public void CloseShopClick()
    {
        shopPanel.SetActive(false);
        cameraGrag.enabled = true;
    }

    bool CheckGridPosition(Vector3 objPos)
    { 
        print(objPos);
        if (objectOnGrid[objPos] == true)
        {
            return false;
        }
        else
        {
            objectOnGrid[objPos] = true;
            return true;
        }

    }
}
