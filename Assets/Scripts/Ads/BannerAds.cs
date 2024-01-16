using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
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

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_RIGHT);
    }

    public void LoadBannerAd()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = BannerLoaded,
            errorCallback = BannerLoadedError
        };

        Advertisement.Banner.Load(adUnitId, options);
    }

    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = BannerShown,
            clickCallback = BannerClicked,
            hideCallback = BannerHidden
        };

        Advertisement.Banner.Show(adUnitId, options);
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }


    private void BannerHidden()
    {
    }

    private void BannerClicked()
    {
    }

    private void BannerShown()
    {
    }

    private void BannerLoadedError(string message)
    {
        Debug.Log("Banner Ad failed");
    }

    private void BannerLoaded()
    {
    }
}
