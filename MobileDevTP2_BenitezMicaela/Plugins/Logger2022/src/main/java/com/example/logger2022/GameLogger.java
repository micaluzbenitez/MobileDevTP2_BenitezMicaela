package com.example.logger2022;

import android.util.Log;

public class GameLogger {
    private static final GameLogger instance = new GameLogger();

    private static final String LOCAL_TAG = "l22 => ";
    private static final String TAG = "Log22 => ";

    public static GameLogger GetInstance() {
        Log.d("", "GetInstance()");
        return instance;
    }

    private GameLogger() { MyLog("Logger created"); }

    public void MyLog(String str) { Log.d(TAG, str); }

    public void DeleteAll()
    {
        // Borra el archivo
        Log.d(LOCAL_TAG, "DeleteAll");
    }

    public String GetAllLogs()
    {
        Log.d(LOCAL_TAG, "GetAllLogs");
        // Lee el archivo y devuelve el contenido
        return "";
    }

    public void ShowAlert()
    {

    }
}
