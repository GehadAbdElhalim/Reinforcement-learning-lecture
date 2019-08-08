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
    public float movmentCost;
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
    float negInf = -1*1e9f;
    private void Start()
    {   
        cells = new GameObject[rows, columns];
        rewards = new float[rows, columns];
        value = new float[rows, columns];
        inaccesible= new bool[rows,columns];
        exit=new bool [rows,columns];
        exit[2,3]=true;
        exit[1,3]=true;
        rewards[2,3]=1;
        rewards[1,3]=-1;
        
        inaccesible[1,1]=true;
        if(minValue <0){
            maxValue=maxValue-minValue;
        }
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject gameObject = Instantiate(cell, new Vector3(j,0,i), Quaternion.identity);
                cells[i, j] = gameObject; 
               
               
            }
        }
        cam.transform.position = new Vector3(((float)(columns - 1) / 2f), cam.transform.position.y, ((float)(rows - 1) / 2f));
        setColors();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // float[] around = new float[4];

                    // //left
                    // around[0] = (j > 0) && !inaccesible[i,j] ? value[i, j-1] : -Mathf.Pow(10,9);

                    // //up
                    // around[1] = (i < rows - 1) && !inaccesible[i, j] ? value[i+1, j] : -Mathf.Pow(10, 9);

                    // //right
                    // around[2] = (j < columns - 1) && !inaccesible[i, j] ? value[i, j+1] : -Mathf.Pow(10, 9);

                    // //down
                    // around[3] = (i > 0) && !inaccesible[i, j] ? value[i-1, j] : -Mathf.Pow(10, 9);

                    // //get max value from array around
                   value[i,j]= valueUpdate(j,i);
                    
                }
            }
            setColors();
            print(value[0,1]);
        }
      
               
                  
        
                
         
    }


    private float valueUpdate(int x , int y){
        int [] hDir = {1,0,-1,0};
        int [] vDir = {0,1,0,-1};   
        float [] action ={0,0,0,0};
        float [] left = {0,0,0,0};
        float [] right = {0,0,0,0};
        float myMax=negInf;
        int myArgMax=-1;
        float myValue =negInf;
        if(exit[y,x]){
            return rewards[y,x];
        }
        for(int i=0;i<4;i++){
            myValue = Mathf.Max(myValue,movmentCost+discount*(retunedValue(x,y,i,1-noise)+LeftAndRight(x,y,i)));
        } 
        
        return myValue;
    }
    private float retunedValue(int x,int y,int i,float p){
        int [] hDir = {1,0,-1,0};
        int [] vDir = {0,1,0,-1};
        if (inGridAndAcc(x+hDir[i],y+vDir[i])){
                return value[y+vDir[i],x+hDir[i]]*p;
            }else{
                return value[y,x]*p;  
            }
    }
    private float LeftAndRight(int x,int y,int i){
        int [] hDir = {1,0,-1,0};
        int [] vDir = {0,1,0,-1};
        if( inGridAndAcc(hDir[left(i)],vDir[left(i)]) && inGridAndAcc(hDir[right(i)],vDir[right(i)])){
            return retunedValue(x,y,left(i),noise/2)+  retunedValue(x,y,right(i),noise/2) ;
        }else if (inGridAndAcc(hDir[left(i)],vDir[left(i)])){
                retunedValue(x,y,left(i),noise);
        } else if (inGridAndAcc(hDir[right(i)],vDir[right(i)])){
                 retunedValue(x,y,right(i),noise);
        }
        return retunedValue(x,y,i,noise);
    }
    int left(int i){
        return (i+16-1)%4;
    }
    int right(int i){
        return (i+1)%4;
    }
    private bool inGridAndAcc (int x,int y){
        if (x<0 || y<0){
            return false;
        }
        if(x>columns-1 || y>rows-1){
            return false;
        }
        if(inaccesible[y,x]){
            return false;
        }
        return true;
    }
   void  setColors(){
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
               
                if(!inaccesible[i,j]){
                    cells[i,j].transform.GetChild(4).GetComponent<Renderer>().material.color = Color.Lerp(Color.red,new Color(00,1,00),(minValue>=0)?(value[i,j])/(maxValue):(value[i,j]-minValue)/(maxValue));
                }else{
                    cells[i,j].transform.GetChild(4).GetComponent<Renderer>().material.color = Color.Lerp(Color.white,Color.black,.5f);
                }
                
                
            }
        }
   }
}
