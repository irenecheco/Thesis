using NLog;
using UnityEngine;
using VRatPolito.PrattiToolkit.VR;
using VRatPolito.PrattiToolkit;

namespace NLog.Unity {

    [DisallowMultipleComponent]
    public class NLogConfig : MonoBehaviour {
        public LogLevel logLevel;
        public bool catchUnityLogs = true;
        public bool logThreaded = true;
        public HandshakeKind handshakeKind;
        Logger _unityLog;

        void Awake() {
            //DontDestroyOnLoad(gameObject);
            LoggerFactory.globalLogLevel = LogLevel.Off;
        }

        void OnEnable() {
            LoggerFactory.globalLogLevel = logLevel;
        }

        void OnDisable() {
            LoggerFactory.globalLogLevel = LogLevel.Off;
        }

        void Start() {
            if (catchUnityLogs) {
                _unityLog = LoggerFactory.GetLogger("Unity");
                if(!logThreaded)
                    Application.logMessageReceived += onLog;
                else
                    Application.logMessageReceivedThreaded += onLog;
                NLogConfig.LogLine($"VERSION;{handshakeKind}");
            }
        }

        void OnDestroy() {
            if (!logThreaded)
                Application.logMessageReceived -= onLog;
            else
                Application.logMessageReceivedThreaded -= onLog;
        }

        void onLog(string condition, string stackTrace, LogType type) {
            if (type == LogType.Log) {
                _unityLog.Debug(condition);
            } else if (type == LogType.Warning) {
                _unityLog.Warn(condition);
            } else {
                _unityLog.Error(condition + "\n" + stackTrace);
            }
        }

        public static void LogLine(string s)
        {
            Debug.Log($";LOGLINE;{s}");
        }
    }
}