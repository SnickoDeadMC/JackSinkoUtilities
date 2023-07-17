using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public static class AddressableUtils
{

    /// <remarks>Does not release the handles. This is typically for loading things that will stay in memory (like for ScriptableObject catalogues).</remarks>
    public static IEnumerator LoadAssetsAsync<T>(string label, List<T> list, Type type, Action onComplete = null)
    {
        AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(label, type);
        yield return handle;

        foreach (IResourceLocation resourceLocation in handle.Result)
        {
            var locationHandle = Addressables.LoadAssetAsync<T>(resourceLocation);
            yield return locationHandle;
            list.Add(locationHandle.Result);
        }
        
        Addressables.Release(handle);
        onComplete?.Invoke();
    }

    /// <remarks>Does not release the handles. This is typically for loading things that will stay in memory (like for ScriptableObject catalogues).</remarks>
    public static List<T> LoadAssetsSync<T>(string label, Type type)
    {
        List<T> assets = new List<T>();
        
        AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(label, type);
        handle.WaitForCompletion();

        foreach (IResourceLocation resourceLocation in handle.Result)
        {
            var locationHandle = Addressables.LoadAssetAsync<T>(resourceLocation);
            locationHandle.WaitForCompletion();
            assets.Add(locationHandle.Result);
        }
        
        Addressables.Release(handle);
        return assets;
    }
    
}
