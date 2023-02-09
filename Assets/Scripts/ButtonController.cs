using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UniTask;

public class ButtonController : MonoBehaviour
{


    async UniTask TestWebsiteResponse()
    {
        using (var request = UnityWebRequest.Get("https://www.example.com"))
        {
            await request.SendWebRequest().WaitUntilDone();

            if (request.isNetworkError || request.responseCode != (long)HttpStatusCode.OK)
            {
                Debug.LogError("Request failed: " + request.error);
            }
            else
            {
                Debug.Log("Response code: " + request.responseCode);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
