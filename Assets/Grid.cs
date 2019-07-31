using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public GameObject cell;
    public GameObject cam;
    public GameObject inputText;
    public GameObject Canvas;

    public float maxValue;
    public float minValue;

    public int rows;
    public int columns;

    public float[,] value;

    public float noise;
    public float discount;

    public bool[,] exit;
    public bool[,] inaccesible;

    public float[,] rewards;
    GameObject[,] cells;

    private void Start()
    {
        cells = new GameObject[rows, columns];
        rewards = new float[rows, columns];
        value = new float[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject gameObject = Instantiate(cell, new Vector3(j,0,i), Quaternion.identity);
                cells[i, j] = gameObject;    
                gameObject.transform.GetChild(4).GetComponent<Renderer>().material.color = Color.Lerp(Color.red,Color.green,value[i,j]/maxValue);
            }
        }
        cam.transform.position = new Vector3(((float)(columns - 1) / 2f), cam.transform.position.y, ((float)(rows - 1) / 2f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    float[] around = new float[4];

                    //left
                    around[0] = (j > 0) && !inaccesible[i,j] ? value[i, j-1] : -Mathf.Pow(10,9);

                    //up
                    around[1] = (i < rows - 1) && !inaccesible[i, j] ? value[i+1, j] : -Mathf.Pow(10, 9);

                    //right
                    around[2] = (j < columns - 1) && !inaccesible[i, j] ? value[i, j+1] : -Mathf.Pow(10, 9);

                    //down
                    around[3] = (i > 0) && !inaccesible[i, j] ? value[i-1, j] : -Mathf.Pow(10, 9);

                    //get max value from array around
                    
                }
            }
        }
    }
}
