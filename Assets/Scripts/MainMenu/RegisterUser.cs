using TMPro;
using UnityEngine;
public class RegisterUser : MonoBehaviour
{
    public TMP_InputField UsernameInput;
    public TMP_InputField PasswordInput;
    public TMP_Text text;

    public void SignUpClick()
    {
        StartCoroutine(Main.Instance.DBController.RegisterUser(UsernameInput.text, PasswordInput.text));
    }


}
