using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AndroidPlugin : MonoBehaviour
{
    [SerializeField] private TMP_Text text = null;

    private LoggerBase logger;

    private void Start()
    {
        logger = LoggerBase.CreateLogger();
        UpdateLogs();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            logger.ShowAlertView(new string[] { "Alert Title", "Alert Message", "Button 1", "Button 2" }, (int obj) => {
                Debug.Log("Local Handler called: " + obj);
            });
        }
    }

    #region UI_Events
    public void SendLog()
    {
        logger.Log("Time: " + Time.time);
        UpdateLogs();
    }

    public void ClearLog()
    {
        // Llamar a la alerta con el callback de ConfirmDeleteAll()
        //logger.ShowAlertView(ConfirmDeleteAll);
    }
    #endregion

    private void ConfirmDeleteAll(bool confirm)
    {
        if (!confirm) return;

        logger.DeleteAll();
        UpdateLogs();
    }
    
    private void UpdateLogs()
    {
        // Updatear logs en la pantalla
        List<string> logs = logger.GetAllLogs();
        text.text = "";

        foreach (string log in logs)
        {
            text.text += log + " ";
        }
    }
}

public abstract class LoggerBase
{
    public abstract void Log(string str);
    public abstract void DeleteAll();
    public abstract List<string> GetAllLogs();
    public abstract void ShowAlertView(string[] strings, System.Action<int>handler = null);

    public static LoggerBase CreateLogger()
    {
#if UNITY_ANDROID
        return new AndroidLogger();
#else
        return new DefaultLogger();
#endif
    }
}

public class AndroidLogger : LoggerBase
{
    const string LOGGER_PLUGIN = "com.example.logger2022";
    static string LOGGER_CLASS_NAME = LOGGER_PLUGIN + ".GameLogger";
    const char SEP = ';';

    public class AlertViewCallback : AndroidJavaProxy
    {
        private Action<int> alertHandler;
        
        public AlertViewCallback(Action<int>alertHandlerIn) : base (LOGGER_CLASS_NAME + "$AlertViewCallback")
        {
            alertHandler = alertHandlerIn;
        }

        public void OnButtonTapped(int index)
        {
            Debug.Log("Button tapped: " + index);
            if (alertHandler != null)
                alertHandler(index);
        }
    }

    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;

    public static AndroidJavaClass PluginClass
    {
        get
        {
            if (_pluginClass == null)
            {
                _pluginClass = new AndroidJavaClass(LOGGER_CLASS_NAME);
                AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("activity");
                _pluginClass.SetStatic<AndroidJavaObject>("activity", activity);
            }
            return _pluginClass;
        }
    }

    public static AndroidJavaObject PluginInstance
    {
        get
        {
            if (_pluginInstance == null)
            {
                _pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("GetInstance");
            }
            return _pluginInstance;
        }
    }

    public override void Log(string str)
    {
        PluginInstance.Call("MyLog", str);
    }

    public override void DeleteAll()
    {
        PluginInstance.Call("DeleteAll");
    }

    public override List<string> GetAllLogs()
    {
        List<string> allLogs = new List<string>();
        string text = PluginInstance.Call<string>("GetAllLogs");
        var logsArray = text.Split(SEP);
        allLogs.AddRange(logsArray);
        return allLogs;
    }

    public override void ShowAlertView(string[] strings, System.Action<int> handler = null)
    {
        // Llama al plugn para mostrar la alerta
        if (strings.Length < 3)
        {
            Debug.LogError("AlertView requires at least 3 strings");
            return;
        }

        PluginInstance.Call("ShowAlertView", new object[] { strings, new AlertViewCallback(handler) });
    }
}

public class DefaultLogger : LoggerBase
{
    public override void Log(string str)
    {
        Debug.Log(str);
    }

    public override void DeleteAll()
    {
        Debug.Log("DeleteAll");
    }

    public override List<string> GetAllLogs()
    {
        Debug.Log("GetAllLogs");
        return new List<string>();
    }

    public override void ShowAlertView(string[] strings, System.Action<int> handler = null)
    {
        Debug.Log("ShowAlertView");
    }
}