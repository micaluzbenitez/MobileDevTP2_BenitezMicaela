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

    #region UI_Events
    public void SendLog()
    {
        logger.Log("Time: " + Time.time);
        UpdateLogs();
    }

    public void ClearLog()
    {
        // Llamar a la alerta con el callback de ConfirmDeleteAll()
        logger.ShowAlert(ConfirmDeleteAll);
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
    public abstract void ShowAlert(System.Action<bool> callback);

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
    static string ALERT_INTERFACE_NAME = LOGGER_PLUGIN + ".AlertWindow";
    const char SEP = ';';
    AndroidJavaClass loggerClass;
    AndroidJavaObject loggerObject;

    public class AlertWindow : AndroidJavaProxy
    {
        public Action acceptAction;
        public Action declineAction;

        public AlertWindow() : base(ALERT_INTERFACE_NAME) { }
        public void OnAccept() => acceptAction?.Invoke();
        public void OnDecline() => declineAction?.Invoke();    }

    public AndroidLogger()
    {
        loggerClass = new AndroidJavaClass(LOGGER_CLASS_NAME);
        loggerObject = loggerClass.CallStatic<AndroidJavaObject>("GetInstance");
    }

    public override void Log(string str)
    {
        loggerObject.Call("MyLog", str);
    }

    public override void DeleteAll()
    {
        loggerObject.Call("DeleteAll");
    }

    public override List<string> GetAllLogs()
    {
        List<string> allLogs = new List<string>();
        string text = loggerObject.Call<string>("GetAllLogs");
        var logsArray = text.Split(SEP);
        allLogs.AddRange(logsArray);
        return allLogs;
    }

    public override void ShowAlert(Action<bool> callback)
    {
        // Llama al plugn para mostrar la alerta
        CreateAlert("Clear Logs and delete Logs.txt", "Are you sure?", () => { callback.Invoke(true); });
        loggerObject.Call("ShowAlert");
    }

    private void CreateAlert(string title, string message, Action accept = null, Action decline = null)
    {
        AlertWindow alertWindow = new AlertWindow();
        alertWindow.acceptAction = accept;
        alertWindow.declineAction = decline;

        loggerObject.Call("CreateAlert", new object[] { title, message, alertWindow });
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

    public override void ShowAlert(Action<bool> callback)
    {
        Debug.Log("ShowAlert");
    }
}