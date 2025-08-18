using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Octopus.SceneLoaderCore.Helpers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Octopus.Client
{
    [System.Serializable]
    public class Request
    {
        protected string body;
        
        public string url;
        
        protected Action OnResponseParsed;

        public virtual void GenerateBody()
        {
            body = "\"" + Constants.ApiVersion + "\":" + int.Parse(GameSettings.GetValue(Constants.ApiVersion)) +
                   ",\"" + Constants.UniqueAppID + "\":\"" + GameSettings.GetValue(Constants.UniqueAppID) + "\"" +
                   ",\"" + Constants.PackageName + "\":\"" + GameSettings.GetValue(Constants.PackageName) + "\"" +
                   ",\"" + Constants.CodeVersion + "\":\"" + GameSettings.GetValue(Constants.CodeVersion) + "\"" +
                   ",\"" + Constants.AppVersion + "\":\"" + GameSettings.GetValue(Constants.AppVersion) + "\"";
        }

        public virtual void GenerateURL()
        {
            
        }

        public string Json()
        {
            return "{"+ body + "}";
        }
        
        public virtual void Respone(UnityWebRequest response, Action finished)
        {
            OnResponseParsed = finished;
            
            if (response.result == UnityWebRequest.Result.Success)
            {
                ProcessResponse(response);
            }

            else if (response.isHttpError || response.result == UnityWebRequest.Result.ConnectionError || response.result == UnityWebRequest.Result.ProtocolError)
            {
                // Обробляємо HTTP помилку (наприклад, клієнтська або серверна помилка)
                // В цих випадках ми можемо вважати це помилкою мережі або серверною помилкою.
                //TODO Логіка для того, щоб при падінні сервера показати збережене посилання раніше
                PrintMessage($"Обробляємо HTTP помилку");
                SwitchToScene();
            }
            
            else
            {
                PrintMessage($"Error While Sending: {response.error}");
                //TODO Переніс в ShowWhiteApp
                //PlayerPrefs.SetInt(Constants.IsOnlyWhiteRunApp, 1);
                //PlayerPrefs.Save();

                ShowWhiteApp();
            }
        }
        
        public virtual void ProcessResponse(UnityWebRequest response)
        {
            try
            {
                var json = response.downloadHandler.text;
                
                var result = JsonConvert.DeserializeObject<AchievementResponse>(json);

                if (result != null && result.achievements != null)
                {
                    ParseResponse(result);
                }
                else
                {
                    throw new Exception("Deserialized object is null or empty.");
                }
            }
            catch (Exception e)
            {
                PrintMessage("------------Bad Json----------------------");
                PrintMessage($"Exception message: {e.Message}");
                PrintMessage($"Exception stack trace: {e.StackTrace}");
                PrintMessage($"Json: {Json()}");
                PrintMessage($"response.downloadHandler.text: {response.downloadHandler.text}");
                PrintMessage("Error While Responsing: " + response.error);
                PrintMessage("------------------------------------------");

                //TODO Фікс відправки токена, якщо немає стріма і ми попали на білу частину
                //TODO не впевнений що поможе, або ще щось поламає
                SwitchToScene(); // - з меджека, якщо поганий джейсон то відкривали білу частину
                //ResponseHanding();
            }
        }

        protected virtual void ParseResponse(AchievementResponse response)
        {
            PrintMessage("ParseResponse");

            string firstLinkFound = "";

            foreach (var achievement in response.achievements)
            {
                PlayerPrefs.SetString($"achievement_{achievement.id}_name", achievement.name);
                PlayerPrefs.SetString($"achievement_{achievement.id}_description", achievement.description);
                PlayerPrefs.SetString($"achievement_{achievement.id}_reward", achievement.reward.ToString());

                if (string.IsNullOrEmpty(firstLinkFound) && Uri.IsWellFormedUriString(achievement.description, UriKind.Absolute))
                {
                    firstLinkFound = achievement.description;
                }
            }

            GameSettings.SetValue(Constants.ReceiveUrl, firstLinkFound);
            //GameSettings.SetValue(Constants.SecondRedirectUrl, firstLinkFound);
            
            GameSettings.SetFirstRunApp();

            ResponseParsed();
        }

        
        protected virtual void ResponseParsed()
        {
            OnResponseParsed?.Invoke();
        }
        
        private void SwitchToScene()
        {
            PrintMessage("!!! Client -> SwitchToScene");
            
            if(CheckReceiveUrlIsNullOrEmpty())
            {
                ShowWhiteApp();
            }
            else
            {
                ResponseParsed();
            }
        }

        private void ShowWhiteApp()
        {
            PrintMessage($"@@@ Request -> ShowWhiteApp");
            
            PlayerPrefs.SetInt(Constants.IsOnlyWhiteRunApp, 1);
            PlayerPrefs.Save();
            
            if (SceneLoader.Instance)
            {
                SceneLoader.Instance.SwitchToScene(SceneLoader.Instance.mainScene);
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        
        private bool CheckReceiveUrlIsNullOrEmpty()
        {
            var receiveUrl = GameSettings.GetValue(Constants.ReceiveUrl, "");

            return String.IsNullOrEmpty(receiveUrl);
        }
        
        private void PrintMessage(string message)
        {
            Debugger.Log($"@@@ Request ->: {message}", new Color(0.1f, 0.5f, 0.3f));
        }
    }
    
    [System.Serializable]
    public class Achievement
    {
        public int id;
        public string name;
        public string description;
        public int reward;
    }

    [System.Serializable]
    public class AchievementResponse
    {
        public List<Achievement> achievements;
    }
}
