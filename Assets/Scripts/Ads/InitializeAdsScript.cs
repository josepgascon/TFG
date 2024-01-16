using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAdsScript : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string androidGameId;
    [SerializeField] private string iosGameId;
    [SerializeField] private bool isTesting;

    string gameId;// = "1234567";

    public void OnInitializationComplete()
    {
        Debug.Log("Ads initialized");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Ads not initialized");
    }

    private void Awake()
    {
    #if UNITY_IOS
            gameId = iosGameId;
    #elif UNITY_ANDROID
            gameId = androidGameId;
    #elif UNITY_EDITOR
            gameId = iosGameId;
    #endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTesting, this);
        }
    }

}
