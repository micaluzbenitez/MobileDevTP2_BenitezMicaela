package com.example.logger2022;

import android.util.Log;

public class GameLogger {
    private static final GameLogger instance = new GameLogger();
    private static final String TAG = "Log22 => ";
    public static GameLogger GetInstance() {
        Log.d("", "GetInstance()");
        return instance;
    }

    private GameLogger() { MyLog("Logger created"); }

    public void MyLog(String str) { Log.d(TAG, str); }
}
