using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Pool;
using Debug = UnityEngine.Debug;

namespace JacksUtils
{ 
    public class ObjectPool : PersistentSingleton<ObjectPool> 
    {
        
        [Tooltip("Assign poolable objects here")]
        [SerializeField] private PoolablePrefab[] poolablePrefabs = Array.Empty<PoolablePrefab>();

        private readonly Dictionary<string, PoolablePrefab> poolablePrefabsByName = new();
        private readonly Dictionary<string, List<GameObject>> spareObjectsByName = new();

        /// <summary>
        /// Retrieve a spare object from a prefab, otherwise create a new one.
        /// </summary>
        /// <param name="prefab">The prefab from the assets folder</param>
        /// <param name="assignToParent">The parent transform to assign the object too</param>
        /// <param name="poolOnDisable">Should the object be pooled when disabled in the hierarchy? If false, the object will need to be manually pooled.</param>
        public GameObject GetSpareOrCreate(GameObject prefab, Transform assignToParent = null, Vector3 position = default, Quaternion rotation = default, bool poolOnDisable = true, Action onPool = null)
        {
            bool objectIsInScene = prefab.scene.name != null;
            if (objectIsInScene)
            {
                Debug.LogWarning($"'{prefab.name}' is an object in the scene. It is recommended to only use prefab objects from the assets folder, in case the object is modified or destroyed.");
            }

            string prefabName = prefab.name;
            if (!poolablePrefabsByName.ContainsKey(prefabName))
            {
                Debug.LogWarning($"Could not find pooled prefab with name '{prefabName}'. Creating the PoolablePrefab now. Consider pre-pooling this object in the ObjectPool instance.");

                PoolablePrefab poolablePrefab = new PoolablePrefab(prefab);
                InitialisePoolablePrefab(poolablePrefab);
            }
            return GetSpareOrCreate(prefabName, assignToParent, position, rotation, poolOnDisable, onPool);
        }

        /// <summary>
        /// Retrieve a spare object from a prefab name, otherwise create a new one.
        /// </summary>
        /// <remarks>It is recommended to get the object using the prefab object rather than the string name, in case the object name changes.</remarks>
        /// <param name="prefabName">The prefab name from the assets folder</param>
        /// <param name="assignToParent">The parent transform to assign the object too</param>
        /// <param name="poolOnDisable">Should the object be pooled when disabled in the hierarchy? If false, the object will need to be manually pooled.</param>
        public GameObject GetSpareOrCreate(string prefabName, Transform assignToParent = null, Vector3 position = default, Quaternion rotation = default, bool poolOnDisable = true, Action onPool = null)
        {
            if (!poolablePrefabsByName.ContainsKey(prefabName))
            {
                Debug.LogError($"Could not find pooled prefab with name '{prefabName}'. Is it set up in the ObjectPool instance?");
                return null;
            }

            GameObject objectToGive = null;
            
            List<GameObject> spares = spareObjectsByName.ContainsKey(prefabName) ? spareObjectsByName[prefabName] : new List<GameObject>();
            if (spares.Count > 0)
            {
                UtilsLoggers.ObjectPoolLogger.Log($"Taking object from '{prefabName}' pool. ({spareObjectsByName[prefabName].Count} spare)");

                //use first available spare
                objectToGive = spares[0];
                spares.Remove(objectToGive);

                objectToGive.transform.position = position;
                objectToGive.transform.rotation = rotation;
            } else
            {
                UtilsLoggers.ObjectPoolLogger.Log($"No available pooled items in '{prefabName}' pool. Creating a new one.");
                
                //instantiate a new one
                objectToGive = CreateObjectFromPrefab(GetPrefabByName(prefabName), position, rotation);
            }
            
            objectToGive.SetActive(true); //set object active
            objectToGive.transform.SetParent(assignToParent, false); //set parent?
            objectToGive.GetComponent<PooledObject>().PoolOnDisable = poolOnDisable; //all objects at this point will have a PooledObject attached, so no need to check
            objectToGive.GetComponent<PooledObject>().IsPooled = false;
            objectToGive.GetComponent<PooledObject>().OnPoolAction = onPool;
            
            return objectToGive;
        }
        
        /// <summary>
        /// Discard the game object and add it to a pool.
        /// <remarks>Objects created by the pool will automatically be pooled on disable.</remarks>
        /// <seealso cref="PooledObject"/>
        /// </summary>
        /// <param name="gameObject">The game object to discard.</param>
        public void Pool(GameObject gameObject)
        {
            String prefabName = gameObject.name;
            if (!spareObjectsByName.ContainsKey(prefabName))
            {
                // create a new pool
                spareObjectsByName[prefabName] = new List<GameObject>();
                UtilsLoggers.ObjectPoolLogger.Log($"Creating new pool for '{prefabName}'.");
            }

            if (spareObjectsByName[prefabName].Contains(gameObject))
                return; //the object is already pooled
            
            spareObjectsByName[prefabName].Add(gameObject); //add to the spare object list

            //disable the object if not already
            if (gameObject.activeSelf)
                gameObject.SetActive(false);

            gameObject.GetComponent<PooledObject>().IsPooled = true;
            gameObject.GetComponent<PooledObject>().OnPoolAction?.Invoke();

            UtilsLoggers.ObjectPoolLogger.Log($"Added {gameObject.name} to {prefabName} pool. ({spareObjectsByName[prefabName].Count} spare)");
        }

        public bool IsPooled(GameObject gameObject)
        {
            PooledObject pooledObject = gameObject.GetComponent<PooledObject>();
            return pooledObject != null && pooledObject.IsPooled;
        }
        
        public void RemoveFromPool(GameObject pooledObject)
        {
            string prefabName = pooledObject.name; //if the prefabName is not given, try using the gameobject's name
            if (!spareObjectsByName.ContainsKey(prefabName))
            {
                Debug.LogWarning($"Tried removing {pooledObject.name} from a pool, but there is no pool.");
                return;
            }

            List<GameObject> pool = spareObjectsByName[prefabName];
            if (!pool.Contains(pooledObject)) {
                Debug.LogWarning($"Tried removing {pooledObject.name} from it's pool, but it wasn't in the pool.");
                return;
            }
            
            pool.Remove(pooledObject);

            //if no more spare objects, remove the reference to the poolable prefab
            if (spareObjectsByName[prefabName].Count == 0 && poolablePrefabsByName.ContainsKey(prefabName))
                poolablePrefabsByName.Remove(prefabName);
        }

        protected override void Initialise()
        {
            base.Initialise();

            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (PoolablePrefab poolablePrefab in poolablePrefabs)
            {
                InitialisePoolablePrefab(poolablePrefab);
            }
            stopwatch.Stop();
            UtilsLoggers.ObjectPoolLogger.Log($"Took {stopwatch.ElapsedMilliseconds}ms to prewarm object pooler.");
        }

        private void InitialisePoolablePrefab(PoolablePrefab poolablePrefab)
        {
            String prefabName = poolablePrefab.prefab.name;
            if (poolablePrefabsByName.ContainsKey(prefabName))
            {
                Debug.LogWarning($"Multiple prefabs to pool found with the name '{prefabName}'.");
                return;
            }

            // assign name to poolablePrefabsByName
            poolablePrefabsByName[prefabName] = poolablePrefab;
            
            // pre-pool prefab with prePoolAmount
            for (int count = 0; count < poolablePrefab.prePoolAmount; count++)
            {
                //create the object and pool it
                GameObject newObject = CreateObjectFromPrefab(poolablePrefab.prefab);
                newObject.transform.SetParent(transform, false); //just for pre-pooled objects, set parent to the ObjectPool instance
                Pool(newObject);
            }
        }

        private GameObject CreateObjectFromPrefab(GameObject prefab, Vector3 position = default, Quaternion rotation = default)
        {
            GameObject newObject = Instantiate(prefab, position, rotation);
            newObject.name = prefab.name;
                
            //ensure the new object has PooledObject attached
            if (newObject.GetComponent<PooledObject>() == null)
                newObject.AddComponent<PooledObject>();

            return newObject;
        }
        
        private GameObject GetPrefabByName(string prefabName)
        {
            return poolablePrefabsByName[prefabName].prefab;
        }
        
        [Serializable] 
        private class PoolablePrefab
        {
            public GameObject prefab;
            public ushort prePoolAmount;

            //if constructing at runtime, cannot set a pre-pool amount
            public PoolablePrefab(GameObject prefab)
            {
                this.prefab = prefab;
            }
        };
        
    }

    //helper methods
    public static class GameObjectExtension 
    {
        /// <summary>
        /// A helper method to pool an object.
        /// NOTE: This is the same as disabling the object if it has a PooledObject attached, as it is pooled on disable.
        /// </summary>
        public static void Pool(this GameObject gameObject)
        {
            if (!ObjectPool.ExistsRuntime) return;
            ObjectPool.Instance.Pool(gameObject);
        }

        /// <summary>
        /// Get a spare of this object from the pool or create a new one.
        /// NOTE: You must pass an object prefab that isn't in the scene
        /// </summary>
        public static GameObject GetSpareOrCreate(this GameObject prefab, Transform assignToParent = null, Vector3 position = default, Quaternion rotation = default, bool poolOnDisable = true, Action onPool = null) 
            => ObjectPool.Instance.GetSpareOrCreate(prefab, assignToParent, position, rotation, poolOnDisable, onPool);

        /// <summary>
        /// Get a spare of this object from the pool or create a new one.
        /// NOTE: You must pass an object prefab that isn't in the scene
        /// <returns>The attached component of type T</returns>
        /// </summary>
        public static T GetSpareOrCreate<T>(this GameObject prefab, Transform assignToParent = null, Vector3 position = default, Quaternion rotation = default, bool poolOnDisable = true, Action onPool = null)
            => prefab.GetSpareOrCreate(assignToParent, position, rotation, poolOnDisable, onPool).GetComponent<T>();

        /// <summary>
        /// Gets whether a gameobject is pooled.
        /// </summary>
        public static bool IsPooled(this GameObject gameObject) => ObjectPool.ExistsRuntime && ObjectPool.Instance.IsPooled(gameObject);

        public static void RemoveFromPool(this GameObject gameObject)
        {
            if (!ObjectPool.ExistsRuntime) return;
            ObjectPool.Instance.RemoveFromPool(gameObject);
        }
        
    }
}
