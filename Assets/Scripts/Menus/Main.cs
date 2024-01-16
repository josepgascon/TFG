using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance;
    
    public DBController DBController;
    public UserInfo UserInfo;
    public Login Login;
    public GameObject Stats;
    public GameObject LevelMenu;
    public GameObject Main_Menu;
    public GameObject registerMenu;
    public static int currentUser;
    public static int user_level_id;
    public static int attempts;
    public static int average_score;
    public static int max_score; 
    public static int perfectly_completed;
    public static bool AudioEnabled;
    public static float speed;

    void Start()
    {
        Instance = this;
        DBController = GetComponent<DBController>();
        UserInfo = GetComponent<UserInfo>();
        //Main.AudioEnabled = true;

    }
}
