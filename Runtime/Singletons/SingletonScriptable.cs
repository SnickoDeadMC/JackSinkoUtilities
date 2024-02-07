using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Debug = UnityEngine.Debug;

namespace JacksUtils
{
    public class SingletonScriptable<T> : ScriptableObject where T : SingletonScriptable<T>
    {

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(typeof(T).Name);
                    instance = handle.WaitForCompletion();

                    stopwatch.Stop();
                    Debug.Log($"Had to load singleton scriptable {typeof(T).Name} synchronously ({stopwatch.ElapsedMilliseconds}ms)");
                }

                return instance;
            }
        }

    }
}