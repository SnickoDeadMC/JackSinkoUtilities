using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace JacksUtils
{
    public class PanelManager : Singleton<PanelManager>
    {

        [Tooltip("Panels that aren't under this object, that should be a part of the panel lookup.")]
        [SerializeField] private AnimatedPanel[] additionalPanels;

        [SerializeField, ReadOnly] private List<AnimatedPanel> panelStack = new();

        private readonly Dictionary<Type, AnimatedPanel> panelLookup = new();

        public ReadOnlyCollection<AnimatedPanel> PanelStack => panelStack.AsReadOnly();

        protected override void Initialise()
        {
            base.Initialise();

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

        private void CreatePanelLookup()
        {
            AnimatedPanel[] panels = GetComponentsInChildren<AnimatedPanel>(true);
            foreach (AnimatedPanel panel in panels)
            {
                TryAddPanelToLookup(panel);
            }

            foreach (AnimatedPanel panel in additionalPanels)
            {
                TryAddPanelToLookup(panel);
            }
        }

        private void TryAddPanelToLookup(AnimatedPanel panel)
        {
            var panelType = panel.GetType();
            if (panelLookup.ContainsKey(panelType))
            {
                Debug.LogError($"Multiple panels detected of type: {panelType.Name}");
                return;
            }

            panel.gameObject.SetActive(false);
            panelLookup.Add(panelType, panel);

            UtilsLoggers.PanelLogger.Log($"Added {panel.GetType()} to panel lookup");
        }

    }
}