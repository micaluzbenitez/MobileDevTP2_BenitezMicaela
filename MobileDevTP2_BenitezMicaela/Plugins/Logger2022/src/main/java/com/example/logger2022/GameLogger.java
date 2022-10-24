package com.example.logger2022;

import android.util.Log;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

public class GameLogger {
    private static final GameLogger instance = new GameLogger();

    private static final String TAG = "Log22 => ";

    private static Activity unityActivity;
    AlertDialog.Builder builder;

    static String filepath;
    static String logs = "";
    static File file;

    public static GameLogger GetInstance(String path) {
        Log.d("", "GetInstance()");
        filepath = path;
        LoadLog();
        return instance;
    }

    private GameLogger() { MyLog("Logger created"); }
    public void MyLog(String str) { Log.d(TAG, str); }

    public static void ReceiveUnityActivity(Activity activity)
    {
        unityActivity = activity;
    }

    // ---------------------------------------- LOGS ----------------------------------------

    public void SendLog(String message)
    {
        Log.d(TAG, message);
        logs += message;
    }

    private static void LoadLog()
    {
        File file = new File(filepath);

        if(file.exists())
        {
            try {
                FileInputStream stream = new FileInputStream(file);
                byte[] bytes = new byte[(int) file.length()];
                try {
                    stream.read(bytes);
                }
                catch (IOException e)
                {
                    e.printStackTrace();
                }
                finally
                {
                    try
                    {
                        stream.close();
                        logs = new String(bytes);
                        Log.d(TAG, "File loaded!");
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                }

            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }
        }
    }

    public void SaveLog()
    {
        if(file == null)
            file = new File(filepath);
        else
            file.delete();

        try {
            if(file.createNewFile()) {
                FileOutputStream stream = new FileOutputStream(file);
                Log.d(TAG, "File created!");
                try {
                    stream.write(logs.getBytes());
                    Log.d(TAG, "File saved!");
                } finally {
                    stream.close();
                    Log.d(TAG, "File closed!");
                }
            }

        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public String GetAllLogs()
    {
        Log.d(TAG, "GetAllLogs");
        return logs;
    }

    public void DeleteAll()
    {
        logs = "";
        file.delete();
        Log.d(TAG, "DeleteAll");
    }

    // ------------------------------------ ALERT WINDOW ------------------------------------

    public void CreateAlert(String title, String message, AlertWindow alertWindow)
    {
        builder = new AlertDialog.Builder(unityActivity);
        builder.setTitle(title);
        builder.setMessage(message);
        builder.setCancelable(false);
        builder.setPositiveButton(
                "YES",
                new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        Log.d(TAG, "Clicked from plugin - YES");
                        alertWindow.OnAccept();
                        dialog.cancel();
                    }
                }
        );

        builder.setNegativeButton(
                "NO",
                new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        Log.d(TAG, "Clicked from plugin - NO");
                        alertWindow.OnDecline();
                        dialog.cancel();
                    }
                }
        );
    }

    public void ShowAlert()
    {
        AlertDialog alert = builder.create();
        alert.show();
    }
}
