package com.example.logger2022;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.util.Log;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

public class GameLogger
{
    private static final GameLogger instance = new GameLogger();
    private static final String TAG = "Log22 => ";

    private static Activity activity;
    AlertDialog.Builder builder;

    static String logs = "";
    static String filepath;
    static File file;

    public static GameLogger GetInstance(String path)
    {
        Log.d("", "GetInstance()");
        filepath = path;
        return  instance;
    }

    private GameLogger() { MyLog("Logger created"); }
    public void MyLog(String str) { Log.d(TAG, str); }

    public static void ReceiveUnityActivity(Activity _activity)
    {
        activity = _activity;
    }

    // ---------------------------------------- LOGS ----------------------------------------

    public void SendLog(String str)
    {
        Log.d(TAG, "SendLog");
        Log.d(TAG, str);
        logs += str;
    }

    public void SaveLog()
    {
        Log.d(TAG, "SaveLog");

        if(file == null) file = new File(filepath);
        else file.delete();

        try {
            if(file.createNewFile())
            {
                FileOutputStream stream = new FileOutputStream(file);
                try
                {
                    stream.write(logs.getBytes());
                }
                finally
                {
                    stream.close();
                }
            }

        } catch (IOException e)
        {
            e.printStackTrace();
            Log.e("Exception","Failed to save log line, File couldn't be saved" + e.toString());
        }
    }

    public String GetAllLogs()
    {
        Log.d(TAG, "GetAllLogs");

        File file = new File(filepath);
        if (!file.exists()) return  null;

        try
        {
            FileInputStream stream = new FileInputStream(file);
            byte[] bytes = new byte[(int) file.length()];
            try
            {
                stream.read(bytes);
            }
            catch (IOException e)
            {
                e.printStackTrace();
                Log.e("Exception","Error getting logs from file savedLogs.text" + e.toString());
            }
            finally
            {
                try
                {
                    stream.close();
                    logs = new String(bytes);
                }
                catch (IOException e)
                {
                    e.printStackTrace();
                    Log.e("Exception","Error getting logs from file savedLogs.text" + e.toString());
                }
            }

        }
        catch (FileNotFoundException e)
        {
            e.printStackTrace();
            Log.e("Exception","Error getting logs from file savedLogs.text" + e.toString());
        }

        return logs;
    }

    public void DeleteAll()
    {
        Log.d(TAG, "DeleteAll");
        logs = "";
        file.delete();
    }

    // ---------------------------------------- ALERT ---------------------------------------

    public void ShowAlert(String title, String message, AlertCallback alertCallback)
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
                        alertCallback.OnAccept();
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
                        alertCallback.OnDecline();
                        dialog.cancel();
                    }
                }
        );

        AlertDialog alert = builder.create();
        alert.show();
    }
}