using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    Vector3 vel;
    public GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (player.GetComponent<Cubie>().isDead)
            return;
        Vector3 target = new Vector3(player.transform.position.x+3f,player.transform.position.y,transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position,target,ref vel,0.5f);
	}
}
