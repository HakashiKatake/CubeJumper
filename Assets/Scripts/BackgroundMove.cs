using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    private GameObject player;
    Vector3 vel;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (player.GetComponent<Cubie>().isDead)
            return;
        transform.position = Vector3.SmoothDamp
            (
                transform.position,
                new Vector3(Camera.main.transform.position.x + 3f, player.transform.position.y, transform.position.z),
                ref vel,
                1f
            );
    }
}
