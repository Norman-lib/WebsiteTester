
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Net;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using System;


public class ButtonController : MonoBehaviour
{
    //les boutons de controle
    public Button startButton;
    public Button stopButton;
    public Button errorButton;

    UnityAction actionStart;
    UnityAction actionStop;
    UnityAction actionError;

    

    string[] websites1 =
    {
        "www.amazon.fr",
        "www.google.fr",
        "https://drivronlinebackendtest.azurewebsites.net/api/v1/Applications",
    };
    string [] websites2 = { "https://drivronlinebackendtest.azurewebsites.net/api/v1/Applications",
        "www.microsoft.fr",
        "drivr.online",
        "www.sfr.fr",
    };
    string [] websites3 = { 
        "www.easyjet.fr",
        "https://drivronlinebackendtest.azurewebsites.net/api/v1/ApplicationConfiguration/parameters",
        "https://github.com/picoxr/support",
    };
    async UniTask TestWebsiteResponse(string[] webs)
    {
        var progress = Progress.Create<float>(x => Debug.Log(x));
        var progress1 = Progress.Create<float>(x => Debug.Log(x));

       /*var requestt = await UnityWebRequest.Get("www.amazon.fr")
            .SendWebRequest()
            .ToUniTask(progress: progress, timing: progress1 );*/

        using (var request = UnityWebRequest.Get("www.amazon.fr"))
        {
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.responseCode != (long)HttpStatusCode.OK)
            {
                Debug.Log("Request Failed: " + request.responseCode);
            } else
            {
                Debug.Log("Response code: " + request.responseCode);
            }

        }
    }

    async UniTask TestWebsiteResponse1(string[] webs) 
    {
        var cts = new System.Threading.CancellationTokenSource();
        var cts1 = new System.Threading.CancellationTokenSource();
        var cts2 = new System.Threading.CancellationTokenSource();

        var progress = Progress.Create<float>(x => Debug.Log(x));
        var startTime = DateTime.Now;

        var request = UnityWebRequest.Get(webs[0]);
        var request1 = UnityWebRequest.Get(webs[1]);
        var request2 = UnityWebRequest.Get(webs[2]);

        request.SendWebRequest();

        await UniTask.WaitWhile(() => !request.isDone, cancellationToken: cts.Token);

        var endTime = DateTime.Now;
        var elapsedTime = endTime - startTime;

        if (elapsedTime.TotalSeconds > 3)
        {
            cts.Cancel();
            Debug.Log("Response code: " + request.responseCode);
            Debug.Log("Request took more than 3 seconds and was cancelled.");
        }
        else
        {
            Debug.Log("Request completed in less than 3 seconds.");
            Debug.Log("Response code: " + request.responseCode);
        }
    }

    async void func()
    {

        await TestWebsiteResponse1(websites1);
       /* await TestWebsiteResponse(websites2);
        await TestWebsiteResponse(websites3);*/
    }
   
    
    // Start is called before the first frame update
       void Start()
     {
        stopButton.gameObject.SetActive(false);
        errorButton.gameObject.SetActive(false);
        actionStart += func;
        startButton.onClick.AddListener(actionStart);
     }

     
}
