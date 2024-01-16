using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class DBController : MonoBehaviour
{
    public TMP_Text SignInText;
    public TMP_Text SignUpText;
    //private EncryptedPlayerPrefs EPP;

    void Start()
    {
        //StartCoroutine(GetRequest("https://subcavexplorer.000webhostapp.com/GetUsers.php"));
        //String user = 
        //EPP.GetString("User");
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 4) StartCoroutine(DisplayBannerWithDelay());

        SecurePlayerPrefs.Init();

        if (SecurePlayerPrefs.HasKey("User") && SecurePlayerPrefs.HasKey("Password"))
        {
            Debug.Log("a veure user: " + SecurePlayerPrefs.GetString("User", "This is so simple"));
            Debug.Log("a veure user: " + SecurePlayerPrefs.GetString("Password", "This is so simple"));
            string user = SecurePlayerPrefs.GetString("User", "error");
            string pass = SecurePlayerPrefs.GetString("Password", "error");
            StartCoroutine(Login(user, pass));
        }
    }

    private IEnumerator DisplayBannerWithDelay()
    {
        yield return new WaitForSeconds(3f);
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }

    public void ShowUserLevel()
    {
        Debug.Log(Main.Instance.UserInfo.UserID);
    }

    public IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        UnityWebRequest www = UnityWebRequest.Post("https://subcavexplorer.000webhostapp.com/Login.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text.Contains("Wrong password") || www.downloadHandler.text.Contains("Username does not exist"))
            {
                SignInText.text = www.downloadHandler.text;
            }
            else
            {
                Main.Instance.UserInfo.SetCredentials(username, password);
                Main.Instance.UserInfo.SetId(www.downloadHandler.text);

                SecurePlayerPrefs.SetString("User", username);
                SecurePlayerPrefs.SetString("Password", password);
        
                Main.currentUser = int.Parse(www.downloadHandler.text);
                Main.Instance.LevelMenu.SetActive(true);
                Main.Instance.Main_Menu.SetActive(false);
                Main.Instance.Login.gameObject.SetActive(false);
            }

        }
    }

    public IEnumerator RegisterUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        UnityWebRequest www = UnityWebRequest.Post("https://subcavexplorer.000webhostapp.com/RegisterUser.php", form);
        yield return www.SendWebRequest();

        if (www.downloadHandler.text.Contains("Username is already taken"))
        {
            SignUpText.text = "Username is already taken";

        }
        else
        {
            www = UnityWebRequest.Post("https://subcavexplorer.000webhostapp.com/Login.php", form);
            yield return www.SendWebRequest();

            Debug.Log("id register user es=");
            Debug.Log(www.downloadHandler.text);

            Main.Instance.Login.gameObject.SetActive(true);
            Main.Instance.registerMenu.SetActive(false);
            SecurePlayerPrefs.SetString("User", username);
            SecurePlayerPrefs.SetString("Password", password);
            StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(www.downloadHandler.text, "6", "0", "0", "0", "0"));//endless te el num 6 pq en el scenemanager tambe es el 6
            StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(www.downloadHandler.text, "1", "0", "0", "0", "0"));//els altres levels tambe tenen el mateix num de scenemanager
            StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(www.downloadHandler.text, "2", "0", "0", "0", "0"));
            StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(www.downloadHandler.text, "3", "0", "0", "0", "0"));

        }
    }

    public IEnumerator RegisterAttempt(int user_id, int level_id)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);
        form.AddField("level_id", level_id);

        UnityWebRequest www = UnityWebRequest.Post("https://subcavexplorer.000webhostapp.com/RegisterAttempt.php", form);
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
    }

    public IEnumerator GetUser()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://subcavexplorer.000webhostapp.com/GetUsers.php")) { 
 
            // Request and wait for the desired page.
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }

    public IEnumerator GetUserLevel(string user_id, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);

        using (UnityWebRequest www = UnityWebRequest.Post("https://subcavexplorer.000webhostapp.com/GetUserLevel.php", form))
        {
            // Request and wait for the desired page.
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;

                //call callback function to pass results
                callback(jsonArray);
            }
        }
    }

    public IEnumerator GetUserLevelAttempt(string user_id, string level_id)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);
        form.AddField("level_id", level_id);

        using (UnityWebRequest www = UnityWebRequest.Post("https://subcavexplorer.000webhostapp.com/GetUserLevelAttempt.php", form))
        {
            // Request and wait for the desired page.
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {

                Debug.Log(www.downloadHandler.text);
                string jsonArrayString = www.downloadHandler.text;
                JSONArray attemptInfoJsonArray = new JSONArray();
                attemptInfoJsonArray = (JSONArray)JSON.Parse(jsonArrayString);
                Debug.Log(attemptInfoJsonArray);

                JSONObject attemptInfoJson = new JSONObject();

                attemptInfoJson = attemptInfoJsonArray[0].AsObject;

                  Main.user_level_id = attemptInfoJson["user_level_id"];
                  Main.attempts = attemptInfoJson["attempts"];
                  Main.average_score = attemptInfoJson["average_score"];
                  Main.max_score = attemptInfoJson["max_score"];
                  Main.perfectly_completed = attemptInfoJson["perfectly_completed"];

                  Debug.Log(Main.user_level_id);
                  Debug.Log(Main.attempts);
                  Debug.Log(Main.average_score);
                  Debug.Log(Main.max_score);
                  Debug.Log(Main.perfectly_completed);  
            }
        }
    }

    public IEnumerator DeleteUserLevelAttempt(string user_id, string level_id)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);
        form.AddField("level_id", level_id);

        using (UnityWebRequest www = UnityWebRequest.Post("https://subcavexplorer.000webhostapp.com/DeleteUserLevelAttempt.php", form))
        {
            // Request and wait for the desired page.
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string jsonArrayString = www.downloadHandler.text;
            }
        }
    }

    public IEnumerator RegisterUserLevelAttempt(string user_id, string level_id, string attempts, string average_score, string max_score, string perfectly_completed)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);
        form.AddField("level_id", level_id);
        form.AddField("attempts", attempts);
        form.AddField("average_score", average_score);
        form.AddField("max_score", max_score);
        form.AddField("perfectly_completed", perfectly_completed);

        using (UnityWebRequest www = UnityWebRequest.Post("https://subcavexplorer.000webhostapp.com/RegisterUserLevelAttempt.php", form))
        {
            // Request and wait for the desired page.
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string jsonArrayString = www.downloadHandler.text;
            }
        }
    }

    public IEnumerator GetAttempt(string user_level_id, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_level_id", user_level_id);

        using (UnityWebRequest www = UnityWebRequest.Post("https://subcavexplorer.000webhostapp.com/GetAttempt.php", form))
        {
            // Request and wait for the desired page.
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;

                //call callback function to pass results
                callback(jsonArray);
            }
        }
    }

}