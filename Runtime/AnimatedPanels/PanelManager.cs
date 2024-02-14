using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JacksUtils
{
    public class PanelManager : Singleton<PanelManager>
    {
        
        [SerializeField, ReadOnly] private List<AnimatedPanel> panelStack = new();

        private readonly Dictionary<Type, AnimatedPanel> panelLookup = new();

        public ReadOnlyCollection<AnimatedPanel> PanelStack => panelStack.AsReadOnly();

        protected override void Initialise()
        {
            base.Initialise();

            panelLookup.Clear();
        
            SceneManager.activeSceneChanged -= OnSceneChange;
            SceneManager.activeSceneChanged += OnSceneChange;
            
            CreatePanelLookup();
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            CreatePanelLookup();
        }
        
        public static T GetPanel<T>() where T : AnimatedPanel
        {
            return GetPanel(typeof(T)) as T;
        }

        public static AnimatedPanel GetPanel(Type panelType)
        {
            if (!Instance.panelLookup.ContainsKey(panelType))
                throw new NullReferenceException($"Could not find panel {panelType} in lookup. Scene is: {SceneManager.GetActiveScene().name}");
        
            return Instance.panelLookup[panelType];
        }
        
        public static bool PanelExists<T>() where T : AnimatedPanel
        {
            return PanelExists(typeof(T));
        }
    
        public static bool PanelExists(Type panelType)
        {
            return Instance.panelLookup.ContainsKey(panelType);
        }
        
        public void AddToStack(AnimatedPanel animatedPanel)
        {
            panelStack.Add(animatedPanel);
        
            animatedPanel.OnAddToStack();
            UtilsLoggers.PanelLogger.Log($"Added {animatedPanel.gameObject.name} to stack.");
        }

        public void RemoveFromStack(AnimatedPanel animatedPanel)
        {
            //show the previous panel if this was the last panel
            if (panelStack.Count >= 2 && panelStack[^1] == animatedPanel)
                panelStack[^2].Show();
        
            panelStack.Remove(animatedPanel);

            animatedPanel.OnRemoveFromStack();
            UtilsLoggers.PanelLogger.Log($"Removed {animatedPanel.gameObject.name} from stack.");
        }

        /// <summary>
        /// Clear the panel lookup and find and add all the panels in the current scene.
        /// </summary>
        private void CreatePanelLookup()
        {
            string sceneName = SceneManager.GetActiveScene().name;
        
            Stopwatch stopwatch = Stopwatch.StartNew();

            //remove null panels (if scene was unloaded etc.)
            List<Type> keys = new List<Type>(panelLookup.Keys);
            foreach (Type key in keys)
            {
                if (panelLookup[key] == null)
                {
                    UtilsLoggers.PanelLogger.Log($"Removing {key} from panel lookup");
                    panelLookup.Remove(key);
                }
            }
        
            foreach (AnimatedPanel panel in SceneUtils.GetAllComponentsInActiveScene<AnimatedPanel>(true))
            {
                UtilsLoggers.PanelLogger.Log($"Adding {panel.GetType()} to panel lookup");
                panelLookup[panel.GetType()] = panel;
                if (panel.gameObject.activeInHierarchy)
                    panel.Show(); //starts showing
                else
                    panel.Hide(instant: true);
            }
        
            stopwatch.Stop();
            UtilsLoggers.PanelLogger.Log($"Took {stopwatch.Elapsed.ToPrettyString(true)} to create the panel lookup for {sceneName}.");
        }
    }
}