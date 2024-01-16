using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;

    private void Awake()
    {
        #if UNITY_IOS
                adUnitId = iosAdUnitId;
        #elif UNITY_ANDROID
                adUnitId = androidAdUnitId;
        #endif
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial Ad initialized");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Interstitial Ad failed");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Interstitial Ad SHOW failed");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
    }

    public void LoadInterstitialAd()
    {
        Advertisement.Load(adUnitId, this);
    }
    public void ShowInterstitialAd()
    {
        Advertisement.Show(adUnitId, this);
        LoadInterstitialAd();
    }
}
