using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Managers;
using Toolbox;

public class AndroidPlugin : MonoBehaviourSingleton<AndroidPlugin>
{
    [SerializeField] private TMP_Text contentText = null;
    [SerializeField] private GameObject UI = null;

    [Header("Scenes")]
    public string mainMenuSceneName = "";

    private LoggerBase logger = null;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        CloseWindow();
        logger = LoggerBase.CreateLogger();
        logger.logs = contentText;
        UpdateLogs();
        Application.logMessageReceived += OnLogReceived;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= OnLogReceived;
    }

    #region UI_Events
    public void SendLog()
    {
        Debug.Log("Time: " + Time.time);
        UpdateLogs();
    }

    public void ClearLog()
    {
        // Llamar a la alerta con el callback de ConfirmDeleteAll()
        ConfirmDeleteAll();
    }
    #endregion

    private void ConfirmDeleteAll()
    {
        logger.DeleteAll();
        UpdateLogs();
    }

    private void UpdateLogs()
    {
        // Updatear logs en la pantalla
        logger.GetAllLogs();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentText.transform.parent);
    }

    private void OnLogReceived(string condition, string stackTrace, LogType type)
    {
        logger.Log(
            $"<b><color=#{ColorUtility.ToHtmlStringRGB(Color.white)}>[{type.ToString()}]</color></b>: {condition}."
        );

        UpdateLogs();
    }

    public void OpenWindow()
    {
        UI.SetActive(true);
    }

    public void CloseWindow()
    {
        UI.SetActive(false);
    }
}

public abstract class LoggerBase
{
    public TMP_Text logs = null;
    public abstract void Log(string message);
    public abstract void DeleteAll();
    public abstract void GetAllLogs();
    public abstract void ShowAlertView(string title, string message, Action positive = null, Action negative = null);

    public static LoggerBase CreateLogger()
    {
#if UNITY_ANDROID
        return new AndroidLogger($"{Application.persistentDataPath}/Logs.txt");
#else
        return new DefaultLogger();
#endif
    }
}

public class AndroidLogger : LoggerBase
{
    const string pluginName = "com.example.logger2022.GameLogger";
    const string interfaceName = "com.example.logger2022.AlertCallback";
    AndroidJavaClass loggerClass;
    AndroidJavaObject loggerObject;
    string filepath = "";

    public class AlertCallback : AndroidJavaProxy
    {
        public Action acceptAction;
        public Action declineAction;

        public AlertCallback() : base(interfaceName) { }
        public void OnAccept() => acceptAction?.Invoke();
        public void OnDecline() => declineAction?.Invoke();
    }

    public AndroidLogger(string filepath)
    {
        this.filepath = filepath;

        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");

        loggerClass = new AndroidJavaClass(pluginName);
        loggerObject = loggerClass.CallStatic<AndroidJavaObject>("GetInstance", filepath);
        loggerObject.CallStatic("ReceiveUnityActivity", activity);

        Debug.Log($"Activity: {activity}");
    }

    public override void Log(string message)
    {
        loggerObject.Call("SendLog", $"{message}\n");
        loggerObject.Call("SaveLog");
    }

    public override void DeleteAll() 
    {
        ShowAlertView("Delete all logs", "Confirm", () =>
        {
            loggerObject.Call("DeleteAll");
            logs.text = "";
        });
    }

    public override void GetAllLogs()
    {
        logs.text = loggerObject.Call<string>("GetAllLogs");
    }

    public override void ShowAlertView(string title, string message, Action positive = null, Action negative = null)
    {
        AlertCallback alertCallback = new AlertCallback();
        alertCallback.acceptAction = positive;
        alertCallback.declineAction = negative;

        loggerObject.Call("ShowAlert", new object[] { title, message, alertCallback });
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

    public override void GetAllLogs()
    {
        Debug.Log("GetAllLogs");
    }

    public override void ShowAlertView(string title, string message, Action positive = null, Action negative = null)
    {
        Debug.Log("ShowAlertView");
    }
}