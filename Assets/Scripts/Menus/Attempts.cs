using System;
using System.Collections;
using SimpleJSON;
using UnityEngine;
using TMPro;

public class Attempts : MonoBehaviour
{

    Action<string> _createAttemptsCallback;

    // Start is called before the first frame update
     public void Start()
    {
        _createAttemptsCallback = (jsonArrayString) => {
            StartCoroutine(CreateAttemptsRoutine(jsonArrayString)); 
        };

        CreateAttempts();
            
    }



    public void CreateAttempts()
    {
        string user_id = Main.currentUser.ToString();
        StartCoroutine(Main.Instance.DBController.GetUserLevel(user_id, _createAttemptsCallback));

    }

    IEnumerator CreateAttemptsRoutine(string jsonArrayString)
    {
        //parsing jsonarray as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //create local variables 
            bool isDone = false;  //are we done downloading?
            string user_level_id = jsonArray[i].AsObject["user_level_id"];
            JSONObject attemptInfoJson = new JSONObject();

            //Create a callback to get the information from Web.cs
            Action<string> getAttemptInfoCallback = (attemptInfo) => {
                isDone = true;
                JSONArray tempArray = JSON.Parse(attemptInfo) as JSONArray;
                attemptInfoJson = tempArray[0].AsObject;
            };
            //wait until DBController calls the callback we passed as parameter
            StartCoroutine(Main.Instance.DBController.GetAttempt(user_level_id, getAttemptInfoCallback));   

            //Wait until the callback is called from DBController (info finished downloaded)
            yield return new WaitUntil(() => isDone == true);

            //Instantiate GameObject (Attempt prefab)
            GameObject attempt = Instantiate(Resources.Load("Prefabs/level_attempt") as GameObject);
            attempt.transform.SetParent(this.transform);
            attempt.transform.localScale = Vector3.one;
            attempt.transform.localPosition = Vector3.zero;

            //fill the information 
            attempt.transform.Find("Level").GetComponent<TMP_Text>().text = "level "+ attemptInfoJson["level_id"];
            attempt.transform.Find("attempts").GetComponent<TMP_Text>().text = "attempts: " + attemptInfoJson["attempts"];
            attempt.transform.Find("avg_score").GetComponent<TMP_Text>().text = "avg score = " + attemptInfoJson["average_score"] + "%";
            attempt.transform.Find("max_score").GetComponent<TMP_Text>().text = "max score = " + attemptInfoJson["max_score"] + "%";
            if (attemptInfoJson["perfectly_completed"] == "1") attempt.transform.Find("perfect_completed").GetComponent<TMP_Text>().text = "perfect = yes";
            else attempt.transform.Find("perfect_completed").GetComponent<TMP_Text>().text = "perfect = no";
            //continue to the next attempt

        }

    }
}
