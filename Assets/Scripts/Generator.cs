using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    public GameObject TilePrefab;

    private float xDiff = 1.1f;
    private float yDiffSmall = 0.4f;
    private float yDiffBig = 1.35f;

    private float xpos = -2.5f;
    private float Ypos = -4.5f;

    private string smallTag = "smallTile";
    private string bigTile = "bigTile";
    
    // Use this for initialization
    void Start () {
        for (int i = 0; i < 7; ++i)
        {
            GenerateTiles();
        }
	}


    public void GenerateTiles()
    {
        int random = Random.Range(0, 5);
        if (random <= 2)
        {
            GenerateSmallTile();
        }
        else {
            GenerateBigTile();
        }
    }
    void GenerateSmallTile()
    {


        xpos += xDiff;
        Ypos += yDiffSmall;
        TilePrefab.tag = smallTag;
        Instantiate(TilePrefab, new Vector3(xpos, Ypos, 0),TilePrefab.transform.rotation);
     

    }
    void GenerateBigTile()
    {
        xpos += xDiff;
        Ypos += yDiffBig;
        TilePrefab.tag = bigTile;
         Instantiate(TilePrefab, new Vector3(xpos, Ypos, 0), TilePrefab.transform.rotation);
        
    }
}
