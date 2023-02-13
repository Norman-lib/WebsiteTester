
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

    void CheckWebsiteResponse(string website, UnityWebRequest request)
    {
       if (request.isDone)
        {
            if (request.result == UnityWebRequest.Result.ConnectionError || request.responseCode != (long)HttpStatusCode.OK)
            {
                Debug.Log("Webtite " + website + " failed loading. Response code :" + request.responseCode);
            }
            else
            {
                Debug.Log("Webtite " + website + " successfully loaded. Response code :" + request.responseCode);
            }
        }
        return;
    }
   

    async UniTask TestWebsiteResponse1(string[] webs) 
    {
        var cts = new System.Threading.CancellationTokenSource();

        var progress = Progress.Create<float>(x => Debug.Log(x));
        var startTime = DateTime.Now;

        var request = UnityWebRequest.Get(webs[0]);
        var request1 = UnityWebRequest.Get(webs[1]);
        var request2 = UnityWebRequest.Get(webs[2]);

        request.SendWebRequest();
        request1.SendWebRequest();
        request2.SendWebRequest();

        /* await UniTask.WaitWhile(() => !request.isDone && !request1.isDone && !request2.isDone , cancellationToken: cts.Token);
         await UniTask.WaitUntil(() => request.isDone && !request1.isDone && !request2.isDone || );*/

        var endTime = DateTime.Now;
        var elapsedTime = endTime - startTime;
        while (elapsedTime.TotalSeconds <= 8 || (request.isDone && request1.isDone && request2.isDone))
        {
            CheckWebsiteResponse(webs[0] ,request1);
            CheckWebsiteResponse(webs[1], request2);
            CheckWebsiteResponse(webs[2], request2);
            if (request.isDone && request1.isDone && request2.isDone)
            {
                return;
            }

            endTime = DateTime.Now;
            elapsedTime = endTime - startTime;
        }
        cts.Cancel();
       
    }

    async UniTask<string> GetTextAsync(UnityWebRequest req)
    {
        var op = await req.SendWebRequest();
        return op.downloadHandler.text;
    }

    async UniTask TestWebsitesResponse (string[] webs)
    {

        var task1 = GetTextAsync(UnityWebRequest.Get("http://google.com"));
        var task2 = GetTextAsync(UnityWebRequest.Get("http://bing.com"));
        var task3 = GetTextAsync(UnityWebRequest.Get("http://yahoo.com"));

        // concurrent async-wait and get results easily by tuple syntax
        var (google, bing, yahoo) = await UniTask.WhenAll(task1, task2, task3);

        // shorthand of WhenAll, tuple can await directly
        var (google2, bing2, yahoo2) = await (task1, task2, task3);
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
    private void Update()
    {
        
    }


}
