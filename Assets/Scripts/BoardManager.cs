using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class BoardManager : MonoBehaviour
{
    public class Count
    {
        int minimum;
        int maximum;

        public int max { get; set; }
        public int min { get; set; }

        public Count(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
    
    public GameObject Fog;
    public GameObject[] pines;
    public GameObject[] rocks;

    //Приблизительное кол-во деревьев и камней
    public Count pinesCount = new Count(420, 540);
    public Count rocksCount = new Count(120, 180);

    int rows = 40;
    int columns = 40;
    const int edgesCount = 900;

    Transform boardHolder;

    List<Vector3> gridPositions = new List<Vector3>();
    int[] edgesPosition = new int[edgesCount]; //массив для хранения порядкового номера позиции на grid
    List<int> checkIndex = new List<int>(); //список для храрения индексов уже расставленных эл-тов
    
    private void Start()
    {
        BoardSetup();
        Fog.SetActive(true);
    }

    void BoardSetup()
    { 
        InitialiseList();

        InitialiseEdgePosition();

        boardHolder = new GameObject("Board").transform;

        LayoutObjectAtRandom(pines, pinesCount.min, pinesCount.max);
        LayoutObjectAtRandom(rocks, rocksCount.min, rocksCount.max);
    }
    
    //Функция для рандомного расставления объектов
    void LayoutObjectAtRandom(GameObject[] objectArray, int minimum, int maximum)
    { 
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
             
            GameObject tileChoice = objectArray[Random.Range(0, objectArray.Length)];

            GameObject newObject = Instantiate(tileChoice, randomPosition, Quaternion.Euler(0,Random.Range(0,360),0));
            newObject.transform.SetParent(boardHolder);
        }
    }

    void InitialiseList()
    { 
        gridPositions.Clear();
         
        for (int x = 1; x < columns; x++)
        { 
            for (int z = 1; z < rows; z++)
            { 
                gridPositions.Add(new Vector3(x + 0.5f, 0f, z + 0.5f));//+0.5f - для центра клетки
            }
        }
    }
    void InitialiseEdgePosition()
    { 
        int i = 0, pos = 0;
        for (int x = 1; x < columns; x++)
        {
            for (int z = 1; z < rows; z++)
            {
                if (x < 8)
                {
                    edgesPosition[i] = pos; 
                    ++i;
                }
                else if ((x > 8 || x < 32) && (z < 8 || z > 37))
                {
                    edgesPosition[i] = pos; 
                    ++i;
                }
                else if (x > 37)
                {
                    edgesPosition[i] = pos; 
                    ++i;
                }
                pos++;
            }
        } 
    }
     
    Vector3 RandomPosition()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, edgesPosition.Length);
        } while (checkIndex.Contains(randomIndex));

        checkIndex.Add(randomIndex);
        
        Vector3 randomPosition = gridPositions[edgesPosition[randomIndex]]; 

        return randomPosition;
    }

    
    
}
