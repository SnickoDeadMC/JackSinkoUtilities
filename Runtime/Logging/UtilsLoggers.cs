using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JacksUtils
{
    [CreateAssetMenu(menuName = "Logging/Utils Loggers")]
    public class UtilsLoggers : SingletonScriptable<UtilsLoggers>
    {

        public static Logger PanelLogger => Instance.panelLogger;

        [SerializeField] private Logger panelLogger;

    }
}
