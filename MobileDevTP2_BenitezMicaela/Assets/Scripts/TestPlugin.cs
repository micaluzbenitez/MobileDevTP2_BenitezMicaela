using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlugin : MonoBehaviour
{
    LoggerBase logger;

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
        var logs = logger.GetAllLogs();
        // Updatear logs en la pantalla
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
        return new defaultLogger();
#endif
    }
}

public class AndroidLogger : LoggerBase
{
    const string LOGGER_CLASS_NAME = "com.example.logger2022.GameLogger";
    const char SEP = ';';
    AndroidJavaClass loggerClass;
    AndroidJavaObject loggerObject;

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
        loggerObject.Call("ShowAlert");
        callback.Invoke(true);
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
        throw new NotImplementedException();
    }
}