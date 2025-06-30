using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {
    float ypos;
    Generator _Generator;
	// Use this for initialization
	void Start () {
        ypos = transform.position.y;
        _Generator = GameObject.Find("TilesGenerator").GetComponent<Generator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y < ypos-10f)
        {
            _Generator.GenerateTiles();
            Destroy(this.gameObject);
        }
		
	}
}
