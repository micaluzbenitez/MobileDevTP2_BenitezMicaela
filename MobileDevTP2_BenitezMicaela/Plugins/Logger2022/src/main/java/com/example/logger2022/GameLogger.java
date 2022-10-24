package com.example.logger2022;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;

import android.util.Log;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;

public class GameLogger {
    private static final GameLogger instance = new GameLogger();
    private static final String TAG = "Log22 => ";

    public static Activity activity;
    AlertDialog.Builder builder;

    public static GameLogger GetInstance(String path) {
        Log.d("", "GetInstance()");
        return instance;
    }

    private GameLogger() { MyLog("Logger created"); }
    public void MyLog(String str) { Log.d(TAG, str); }

    // ---------------------------------------- LOGS ----------------------------------------

    public void SendLog(String str)
    {
        Log.d(TAG, "SendLog");
        Log.d(TAG, str);
    }

    public void SaveLog(String str)
    {
        Log.d(TAG, "SaveLog");
        File path = activity.getFilesDir();
        File file = new File(path, "savedLogs.text");
        try
        {
            FileOutputStream stream = new FileOutputStream(file);
            try
            {
                stream.write(str.getBytes());
            }
            finally
            {
                stream.close();
            }
        }
        catch (IOException exp)
        {
            Log.e("Exception","Failed to save log line, File couldnt be saved" + exp.toString());
        }
    }

    public String GetAllLogs()
    {
        Log.d(TAG, "GetAllLogs");
        File path = activity.getFilesDir();
        File file = new File(path, "savedLogs.text");
        if(!file.exists())
            return  null;

        int length = (int)file.length();
        byte[] bytes = new byte[length];

        try
        {
            FileInputStream stream = new FileInputStream(file);
            try
            {
                stream.read(bytes);
            }
            finally
            {
                stream.close();
            }
        }
        catch(IOException exp)
        {
            Log.e("Exception","Error getting logs from file savedLogs.text" + exp.toString());
        }
        String logsGetFromFile = new String(bytes);
        return  logsGetFromFile;
    }

    public void DeleteAll()
    {
        Log.d(TAG, "DeleteAll");
        File path = activity.getFilesDir();
        File file = new File(path, "savedLogs.text");
        file.delete();
    }

    // ---------------------------------------- ALERT ---------------------------------------

    public void CreateAlert(String title, String message, AlertWindow alertWindow)
    {
        builder = new AlertDialog.Builder(activity);
        builder.setTitle(title);
        builder.setMessage(message);
        builder.setCancelable(false);
        builder.setPositiveButton(
                "YES",
                new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        Log.i(TAG, "Clicked from plugin - YES");
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
                        Log.i(TAG, "Clicked from plugin - NO");
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
