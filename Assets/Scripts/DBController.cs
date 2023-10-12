using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;
using TMPro;
using Unity.Collections;
using SimpleJSON;


// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class DBController : MonoBehaviour
{
    public TMP_Text SignInText;
    public TMP_Text SignUpText;

    void Start()
    {
        // A correct website page.
        //StartCoroutine(GetRequest("https://subcavexplorer.000webhostapp.com/GetUsers.php"));
        //StartCoroutine(Login("pep1", "123"));
        //StartCoroutine(RegisterUser("pep5", "1234"));
        //StartCoroutine(GetUserLevel("1"));

        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));
    }

    public void ShowUserLevel()
    {
        Debug.Log("eu");

        Debug.Log(Main.Instance.UserInfo.UserID);

       // StartCoroutine(GetUserLevel(Main.Instance.UserInfo.UserID));
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
            Main.Instance.UserInfo.SetCredentials(username, password);
            //if(wrong user not found)
            Main.Instance.UserInfo.SetId(www.downloadHandler.text);

            if (www.downloadHandler.text.Contains("Wrong password") || www.downloadHandler.text.Contains("Username does not exist"))
            {
                //Debug.Log("Try again");
                SignInText.text = www.downloadHandler.text;
            }
            else
            {
                Main.currentUser = int.Parse(www.downloadHandler.text);
                Main.Instance.LevelMenu.SetActive(true);
                Main.Instance.Login.gameObject.SetActive(false);

                //Attempts a = gameObject.GetComponent<Attempts>();
                //a.CreateAttempts();
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
            //Debug.Log(www.error);
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
            StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(www.downloadHandler.text, "1", "0", "0", "0", "0"));
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
        Debug.Log("1 p");

        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);
        form.AddField("level_id", level_id);

        using (UnityWebRequest www = UnityWebRequest.Post("https://subcavexplorer.000webhostapp.com/GetUserLevelAttempt.php", form))
        {
            // Request and wait for the desired page.
            yield return www.SendWebRequest();
            Debug.Log("2 p");

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {

                Debug.Log(www.downloadHandler.text);

                string jsonArrayString = www.downloadHandler.text;

                Debug.Log("aixo va be?");
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
        Debug.Log("2 d");

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
        Debug.Log("2 d");

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