using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsScript : MonoBehaviour
{
    public TMP_Text username;
    // Start is called before the first frame update
    void Start()
    {
        if (Main.currentUser != -1)
        {
            string user = SecurePlayerPrefs.GetString("User", "error");
            if (user != "error") { username.text = "username = " + user; }
        }
    }

}
