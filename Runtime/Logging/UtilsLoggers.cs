using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JacksUtils
{
    [CreateAssetMenu(menuName = "Logging/Utils Loggers")]
    public class UtilsLoggers : SingletonScriptable<UtilsLoggers>
    {
        
        public static Logger PanelLogger => Instance.panelLogger;
        public static Logger ObjectPoolLogger => Instance.objectPoolLogger;
        public static Logger InputLogger => Instance.inputLogger;
        
        [SerializeField] private Logger panelLogger;
        [SerializeField] private Logger objectPoolLogger;
        [SerializeField] private Logger inputLogger;
        
    }
}
