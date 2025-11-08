using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {
    float ypos;
    Generator _Generator;
    MusicalGenerator _MusicalGenerator;
    
	// Use this for initialization
	void Start () {
        ypos = transform.position.y;
        
        // Find the generator object
        GameObject generatorObj = GameObject.Find("TilesGenerator");
        if (generatorObj != null)
        {
            _Generator = generatorObj.GetComponent<Generator>();
            _MusicalGenerator = generatorObj.GetComponent<MusicalGenerator>();
            
            Debug.Log($"TileScript: Found generators - Generator: {_Generator != null}, MusicalGenerator: {_MusicalGenerator != null}");
            if (_Generator != null) Debug.Log($"TileScript: Generator.enabled = {_Generator.enabled}");
            if (_MusicalGenerator != null) Debug.Log($"TileScript: MusicalGenerator.enabled = {_MusicalGenerator.enabled}");
        }
        else
        {
            Debug.LogError("TileScript: Could not find TilesGenerator object!");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y < ypos-10f)
        {
            // Call the appropriate generator's GenerateTiles method
            // Check which generator is currently enabled
            if (_MusicalGenerator != null && _MusicalGenerator.enabled)
            {
                Debug.Log("TileScript: Calling MusicalGenerator.GenerateTiles()");
                _MusicalGenerator.GenerateTiles();
            }
            else if (_Generator != null && _Generator.enabled)
            {
                Debug.Log("TileScript: Calling Generator.GenerateTiles()");
                _Generator.GenerateTiles();
            }
            else
            {
                Debug.LogWarning("TileScript: No enabled generator found!");
            }
            
            Destroy(this.gameObject);
        }
		
	}
}
