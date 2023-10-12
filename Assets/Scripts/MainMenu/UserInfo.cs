using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
     public string UserID { get; private set; }  //public get pero private set, nomes aqui
     string UserName;
     string UserPassword;

    public void SetCredentials(string username, string userpassword)
    {
        UserName = username;
        UserPassword = userpassword;
    }

    public void SetId(string userid)
    {
        UserID = userid;
    }
}
