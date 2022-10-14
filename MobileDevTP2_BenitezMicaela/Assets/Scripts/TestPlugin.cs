using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlugin : MonoBehaviour
{
    LoggerBase logger;

    private void Start()
    {
        //logger = LoggerBase.
    }
}

public abstract class LoggerBase
{
    public abstract void Log(string str);

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

    AndroidJavaClass loggerClass;
    AndroidJavaObject loggerObject;

    public AndroidLogger()
    {
        loggerClass = new AndroidJavaClass(LOGGER_CLASS_NAME);
        loggerObject = loggerClass.CallStatic<AndroidJavaObject>("GetInstance");
    }

    public override void Log(string str)
    {
        loggerObject.Call("MyLog", "BOTON");
    }
}

public class DefaultLogger : LoggerBase
{
    public override void Log(string str)
    {
        Debug.Log(str);
    }
}