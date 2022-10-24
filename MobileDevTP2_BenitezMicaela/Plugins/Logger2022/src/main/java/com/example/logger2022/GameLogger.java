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

    public static GameLogger GetInstance() {
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
            Log.e("Exception","Failed to save log line, File couldn't be saved" + exp.toString());
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
    public interface AlertViewCallback
    {
        public void OnButtonTapped(int id);
    }

    public void ShowAlertView(String[] strings, final AlertViewCallback callback)
    {
        if (strings.length < 3)
        {
            Log.i(TAG, "Error - expected at least 3 strings, got " + strings.length);
            return;
        }

        DialogInterface.OnClickListener myClickListener = new DialogInterface.OnClickListener()
        {
            @Override
            public void onClick(DialogInterface dialogInterface, int id)
            {
                dialogInterface.dismiss();
                Log.i(TAG, "Tapped: " + id);
                callback.OnButtonTapped(id);
            }
        };

        AlertDialog alertDialog = new AlertDialog.Builder(activity)
                .setTitle(strings[0])
                .setMessage(strings[1])
                .setCancelable(false)
                .create();
        alertDialog.setButton(alertDialog.BUTTON_NEUTRAL, strings[2], myClickListener);
        if (strings.length > 3)
            alertDialog.setButton(alertDialog.BUTTON_NEGATIVE, strings[3], myClickListener);
        if (strings.length > 4)
            alertDialog.setButton(alertDialog.BUTTON_POSITIVE, strings[4], myClickListener);
        alertDialog.show();
    }
}
