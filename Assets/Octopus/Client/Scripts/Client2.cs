using System;
using System.Collections.Generic;
using Core;
using Octopus.SceneLoaderCore.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

namespace Octopus.Client
{
    public class Client2 : MonoBehaviour
    {
        public static Client2 Instance;
        
        private List<Request> requests = new List<Request>();
        
        private UniWebView _webView;
        
        private string generatedURL;
        
        protected void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        
        public void Initialize()
        {
            PrintMessage($"!!! Initialize: IsFirstRunApp={GameSettings.GetValue(Constants.IsFirstRunApp)}");
            
            if(GameSettings.HasKey(Constants.IsFirstRunApp))
            {
                PrintMessage("!!! Повторно запустили додаток");
                
                if (CheckReceiveUrlIsNullOrEmpty())
                {
                    PrintMessage("!!! Стартова сторінка з Бінома є порожня, показуєм білу апку");
                    
                    SwitchToScene();
                }
                else
                {
                    PrintMessage("!!! Біном не є порожній");

                    SwitchToScene();
                }
            }
            else 
            {
                PrintMessage("!!! Перший раз запустили додаток");
                
                GameSettings.Init();
                
                requests.Add(new InitRequest());
                
                Send(requests[0]);
            }
        }

        private void Send(Request request)
        {
            PrintMessage($"Send Request {request.GetType()}");
            
            requests.Remove(request);

            StartCoroutine(SenderRequest.Send(request, CheckRequests));
        }

        private void CheckRequests()
        {
            PrintMessage("!!! CheckRequests");
            
            if (requests.Count != 0)
            {
                Send(requests[0]);
            }
            else
            {
                SwitchToScene();
            }
        }
        
        private void SwitchToScene()
        {
            var scene = CheckReceiveUrlIsNullOrEmpty() ? SceneLoader.Instance.mainScene : SceneLoader.Instance.webviewScene;
            
            PrintMessage($"SwitchToScene {scene}");
            
            if (SceneLoader.Instance)
            {
                SceneLoader.Instance.SwitchToScene(scene);
            }
            else
            {
                SceneManager.LoadScene(scene);
            }
        }

        private bool CheckReceiveUrlIsNullOrEmpty()
        {
            PrintMessage("!!! CheckStartUrlIsNullOrEmpty");
            
            var receiveUrl = GameSettings.GetValue(Constants.ReceiveUrl, "");

            PrintMessage($"@@@ StartUrl: {receiveUrl}");

            return String.IsNullOrEmpty(receiveUrl);
        }
        
        private void PrintMessage(string message)
        {
            Debugger.Log($"@@@ Client2 ->: {message}", new Color(0.2f, 0.4f, 0.9f));
        }
    }
}
