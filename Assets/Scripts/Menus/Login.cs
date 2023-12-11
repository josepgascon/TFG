using TMPro;
using UnityEngine;
public class Login : MonoBehaviour
{
    public TMP_InputField UsernameInput;
    public TMP_InputField PasswordInput;


    public void SignInClick()
    {
        StartCoroutine(Main.Instance.DBController.Login(UsernameInput.text, PasswordInput.text));
    }


}
