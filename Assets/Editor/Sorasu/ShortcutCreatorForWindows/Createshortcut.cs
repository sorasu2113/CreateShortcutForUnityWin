using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

/// <summary>
/// 直接起動できるShortcutを作るやつ
/// 
/// 青木そらす
/// 
/// Twitter:@oepn_sorasu
/// 
/// 参考
/// https://dobon.net/vb/dotnet/file/createshortcut.html
//  https://qiita.com/y-takano/items/b94312abc17159dce8be
//
/// </summary>

namespace SORASU.EditorOnly
{
    public class Createshortcut : MonoBehaviour
    {

        static System.Diagnostics.Process exProcess;

        // Use this for initialization
        [MenuItem("Shortcut/Create")]
        static void CreateShortcut()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                EditorUtility.DisplayDialog("Error", "It works only on Windows.", "ok");
                return;
            }

            if (exProcess != null)
            {
                EditorUtility.DisplayDialog("Error", "It is being processed.", "ok");
                return;
            }

            //EditorのPath
            string ExecutablePath = UnityEditor.EditorApplication.applicationPath;

            //Assetsを取得するので、消す
            string topDirectoryPath = Application.dataPath.Replace("/Assets", "");

            //.linkが作成される場所
            string linkPath = topDirectoryPath + "/" + Application.productName + ".lnk";

            exProcess = new System.Diagnostics.Process();

            exProcess.StartInfo.UseShellExecute = false;

            exProcess.StartInfo.FileName = Application.dataPath + "/Editor/Sorasu/ShortcutCreatorForWindows/shortcut.bat";//バッチファイル
            exProcess.StartInfo.Arguments = returnQuotation(linkPath) + " " + returnQuotation(ExecutablePath) + " " + returnQuotation(" -projectPath " + topDirectoryPath);

            // 標準出力を読み取り可.
            exProcess.StartInfo.RedirectStandardOutput = true;
            exProcess.OutputDataReceived += OutputHandler;

            exProcess.StartInfo.RedirectStandardError = true;
            exProcess.ErrorDataReceived += ErrorOutputHanlder;

            //外部プロセスの終了を検知してイベントを発生させます.
            exProcess.EnableRaisingEvents = true;
            exProcess.Exited += exProcess_Exited;

            exProcess.StartInfo.RedirectStandardInput = true;

            //外部のプロセスを実行する
            exProcess.Start();

            // プロセス標準出力.
            exProcess.BeginOutputReadLine();

            // プロセスエラー出力.
            exProcess.BeginErrorReadLine();

            //出力先を開く
            System.Diagnostics.Process.Start(topDirectoryPath);
        }

        static string returnQuotation(string s)
        {
            return "\"" + s + "\"";
        }

        // 標準出力時.
        static void OutputHandler(object sender, System.Diagnostics.DataReceivedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                Debug.Log(args.Data);
            }
        }

        // エラー出力時.
        static void ErrorOutputHanlder(object sender, System.Diagnostics.DataReceivedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                Debug.Log(args.Data);
            }
        }

        static void exProcess_Exited(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process proc = (System.Diagnostics.Process)sender;

            Debug.LogError(proc);

            exProcess.Dispose();
            exProcess = null;
        }
    }
}