using System;
using UnityEngine;
using MyBox;

namespace JacksUtils
{
    /// <summary>
    /// Generic singleton class.
    /// </summary>
    /// <remarks>You can add [ExecuteInEditMode] if you need the instance in edit mode (eg. for edit mode tests).</remarks>
    public abstract class Singleton<T> : Updatable where T : MonoBehaviour
    {

        /// <summary>
        /// Whether the instance exists at runtime.
        /// </summary>
        public static bool ExistsRuntime { get; private set; }

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                        throw new NullReferenceException($"Tried lazilly accessing {typeof(T)}, but it couldn't be found in the scene.");
                    if (Application.isPlaying)
                        Debug.LogWarning($"Lazilly accessing {instance.name} singleton, as it is not yet initialised.");
                }

                return instance;
            }
        }

        [Foldout("Singleton"), InitializationField, SerializeField]
        protected bool dontDestroy = true;

        protected override void Awake()
        {
            if (ExistsRuntime)
            {
                //instance already exists, so just remove this new one
                gameObject.SetActive(false); //set inactive so it doesn't trigger colliders etc.
                Destroy(gameObject);
                return;
            }

            Initialise();
        }

        private void OnDestroy()
        {
            if (instance == this)
                OnInstanceDestroyed();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (instance == this)
                OnInstanceDisabled();
        }

        protected override void Initialise()
        {
            base.Initialise();

            instance = this as T;

            if (dontDestroy)
            {
                transform.parent = null; //can't be a child
                DontDestroyOnLoad(gameObject);
            }

            if (Application.isPlaying)
                ExistsRuntime = true;
        }

        /// <summary>
        /// Called when the singleton instance is disabled.
        /// </summary>
        protected virtual void OnInstanceDisabled()
        {

        }
        
        /// <summary>
        /// Called when the singleton instance is destroyed.
        /// </summary>
        protected virtual void OnInstanceDestroyed()
        {
            ExistsRuntime = false;
        }

    }
}