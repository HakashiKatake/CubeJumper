using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetConnectivity : MonoBehaviour {

    // Use this for initialization
    void Start() {
        StartCoroutine(checkInternetConnection((isConnected) =>
        {
            if (isConnected)
            {
                print("Connected...");
            }
            else
            {

                print("Not Connected...");

            }
        }));
    }
        
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }


    public void RateUs()
    {
        #if UNITY_ANDROID
        Application.OpenURL("market://details?id=codematics.cubie.jumping.game");
        
        #elif UNITY_IPHONE
         Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
        
        #endif
    }
}
