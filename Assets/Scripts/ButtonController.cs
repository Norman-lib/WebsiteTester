
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Net;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks.Linq;


public class ButtonController : MonoBehaviour
{
    //les boutons de controle
    public Button startButton;
    public Button stopButton;
    public Button errorButton;

    bool isStartClicked = false;
    bool isCancelClicked = false;

    UnityAction actionStart;
    UnityAction actionStop;
    UnityAction actionError;

    DateTime startTime;
    DateTime currentTime;
    TimeSpan elapsedTime;






    string[] websites1 =
    {
        "www.amazon.fr",
        "www.google.fr",
        "https://drivronlinebackendtest.azurewebsites.net/api/v1/Applications",
    };
    string[] websites2 = {
        "www.microsoft.fr",
        "drivr.online",
        "www.sfr.fr",
    };
    string[] websites3 = {
        "www.easyjet.fr",
        "https://drivronlinebackendtest.azurewebsites.net/api/v1/ApplicationConfiguration/parameters",
        "https://github.com/picoxr/support",
    };
  /*  async UniTask TestWebsiteResponse(string[] webs)
    {
        var progress = Progress.Create<float>(x => Debug.Log(x));
        var progress1 = Progress.Create<float>(x => Debug.Log(x));

        *//*var requestt = await UnityWebRequest.Get("www.amazon.fr")
             .SendWebRequest()
             .ToUniTask(progress: progress, timing: progress1 );*//*

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
    }*/

    void CheckWebsiteResponse(string website, UnityWebRequest request)
    {

        if (request.result == UnityWebRequest.Result.ConnectionError || request.responseCode != (long)HttpStatusCode.OK)
        {
            Debug.Log("Website " + website + " failed loading. Response code :" + request.responseCode);
        }
        else
        {
            Debug.Log("Website " + website + " successfully loaded. Response code :" + request.responseCode);
        }

        return;
    }

    async UniTask TestWebsiteResponse2(string[] webs)
    {
        var cancelToken = new CancellationTokenSource();
        stopButton.onClick.AddListener(() =>
        {
            cancelToken.Cancel(); // cancel from button click.
        });
        // timeoutTokens
        var timeoutToken = new CancellationTokenSource();
        timeoutToken.CancelAfterSlim(TimeSpan.FromSeconds(8));
        var timeoutToken1 = new CancellationTokenSource();
        timeoutToken1.CancelAfterSlim(TimeSpan.FromSeconds(8));
        var timeoutToken2 = new CancellationTokenSource();
        timeoutToken2.CancelAfterSlim(TimeSpan.FromSeconds(8));

        //defining the requests
        UnityWebRequest request = UnityWebRequest.Get(webs[0]);
        UnityWebRequest request1 = UnityWebRequest.Get(webs[1]);
        UnityWebRequest request2 = UnityWebRequest.Get(webs[2]);



        try
        {
            // combine tokens
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancelToken.Token, timeoutToken.Token);
            var linkedTokenSource1 = CancellationTokenSource.CreateLinkedTokenSource(cancelToken.Token, timeoutToken1.Token);
            var linkedTokenSource2 = CancellationTokenSource.CreateLinkedTokenSource(cancelToken.Token, timeoutToken2.Token);

            var linkedTokens = CancellationTokenSource.CreateLinkedTokenSource(cancelToken.Token, timeoutToken1.Token, timeoutToken2.Token, timeoutToken.Token);

            //sending the requests

            UniTask task = request.SendWebRequest().WithCancellation(linkedTokenSource.Token);
            UniTask task1 = request1.SendWebRequest().WithCancellation(linkedTokenSource1.Token);
            UniTask task2 = request2.SendWebRequest().WithCancellation(linkedTokenSource2.Token);

            await UniTask.WaitWhile((() => !(request.isDone && request1.isDone && request2.isDone)), cancellationToken: linkedTokens.Token);
            //   await UnityWebRequest.Get("http://foo").SendWebRequest().WithCancellation(linkedTokenSource.Token);
            CheckWebsiteResponse(webs[0], request);
            CheckWebsiteResponse(webs[1], request1);
            CheckWebsiteResponse(webs[2], request2);
        }

        catch (OperationCanceledException ex)
        {

            if (timeoutToken.IsCancellationRequested)
            {
                Debug.Log("Website" + webs[0] + "took more than 8 seconds");
            }
            else if (timeoutToken1.IsCancellationRequested)
            {
                Debug.Log("Website" + webs[1] + "took more than 8 seconds");
            }
            else if (timeoutToken.IsCancellationRequested)
            {
                Debug.Log("Website" + webs[2] + "took more than 8 seconds");
            }
            else if (cancelToken.IsCancellationRequested)
            {
                Debug.Log("Operation canceled");
            }
        }
        catch (UnityWebRequestException ex)
        {

        }
        catch (Exception ex)
        {

        }

       

    }

   /* async void TestWebsiteResponse1(string[] webs)
    {
        *//* var cts = new System.Threading.CancellationTokenSource();

         var progress = Progress.Create<float>(x => Debug.Log(x));*/


        /*request = UnityWebRequest.Get(webs[0]).SendWebRequest();
        request1 = UnityWebRequest.Get(webs[1]);
        request2 = UnityWebRequest.Get(webs[2]);*//*






        // await UniTask.WaitWhile(() => !request.isDone && !request1.isDone && !request2.isDone, cancellationToken: cts.Token);
        // await UniTask.WaitUntil(() => request.isDone && !request1.isDone && !request2.isDone || );


        *//*while (elapsedTime.TotalSeconds <= 8 || (request.isDone && request1.isDone && request2.isDone))
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
        }*//*
        //  cts.Cancel();

    }*/

   /* async UniTask<string> GetTextAsync(UnityWebRequest req)
    {
        var op = await req.SendWebRequest();
        return op.downloadHandler.text;
    }

    async UniTask TestWebsitesResponse(string[] webs)
    {

        var task1 = GetTextAsync(UnityWebRequest.Get("http://google.com"));
        var task2 = GetTextAsync(UnityWebRequest.Get("http://bing.com"));
      //  var task3 = GetTextAsync(UnityWebRequest.Get("http://yahoo.com

            // concurrent async-wait and get results easily by tuple sy
          //  var (google, bing, yahoo) = await UniTask.WhenAll(task1, task2, task3);

            // shorthand of WhenAll, tuple can await dire
          //  var (google2, bing2, yahoo2) = await (task1, task2, task3);
    } */

     void start()
    {
        isStartClicked = true;
        isCancelClicked = false;
    }

    async void func()
    {
        stopButton.gameObject.SetActive(true);
        errorButton.gameObject.SetActive(true);
        
       
        if (! isCancelClicked) 
        {
            await TestWebsiteResponse2(websites1);
            await TestWebsiteResponse2(websites2);
            await TestWebsiteResponse2(websites3);

            func();
        }
}
        
    
   

     void stop ()
     {
        isCancelClicked = true;
        isStartClicked = false;
     }
    // Start is called before the first frame update
       void Start()
     {
        stopButton.gameObject.SetActive(false);
        errorButton.gameObject.SetActive(false);
        actionStart += start;
        actionStart += func;
        actionStop += stop;
        startButton.onClick.AddListener(actionStart);
        stopButton.onClick.AddListener(actionStop);
     }



       void Update()
        {
            
        }

/*private void Updatee()
    {
        if (isStartClicked)
        {
             currentTime = DateTime.Now;
             elapsedTime = currentTime - startTime;
          //  Debug.Log(elapsedTime.TotalSeconds);
            if (elapsedTime.TotalSeconds < 8)
            {
                if (request.isDone && !isResponsePrinted)
                {
                    isResponsePrinted = true;
                    if (request.result == UnityWebRequest.Result.ConnectionError || request.responseCode != (long)HttpStatusCode.OK)
                    {
                        Debug.Log("Webtite " + websites1[0] + " failed loading. Response code :" + request.responseCode);
                    }
                    else
                    {
                        Debug.Log("Webtite " + websites1[0] + " successfully loaded. Response code :" + request.responseCode);
                    }
                }
                if (request1.isDone && !isResponsePrinted1)
                {
                    isResponsePrinted1 = true;
                    if (request1.result == UnityWebRequest.Result.ConnectionError || request1.responseCode != (long)HttpStatusCode.OK)
                    {
                        Debug.Log("Webtite " + websites1[1] + " failed loading. Response code :" + request1.responseCode);
                    }
                    else
                    {
                        Debug.Log("Webtite " + websites1[1] + " successfully loaded. Response code :" + request1.responseCode);
                    }
                }  if (request2.isDone && !isResponsePrinted2)
                {
                    isResponsePrinted2 = true;
                    if (request2.result == UnityWebRequest.Result.ConnectionError || request2.responseCode != (long)HttpStatusCode.OK)
                    {
                        Debug.Log("Webtite " + websites1[2] + " failed loading. Response code :" + request2.responseCode);
                    }
                    else
                    {
                        Debug.Log("Webtite " + websites1[2] + " successfully loaded. Response code :" + request2.responseCode);
                    }
                }
            } else
            {
                Debug.Log("time ends");
            }
        }
    }*/
   

}
