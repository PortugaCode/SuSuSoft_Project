#if UNITY_EDITOR && UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

using TheBackend.ToolKit.GoogleLogin.Settings;

public class PostProcessBuildForBackendGoogleLogin
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
            AddURLSchemeToXcode(pathToBuiltProject);
        }
    }

    private static void AddURLSchemeToXcode(string path)
    {
        var settings = (TheBackendGoogleSettings)Resources.Load(nameof(TheBackendGoogleSettings), typeof(TheBackendGoogleSettings));

        if (settings == null || string.IsNullOrEmpty(settings.iosURLSchema))
        {
            return;
        }

        string projPath = PBXProject.GetPBXProjectPath(path);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string targetGUID = proj.GetUnityFrameworkTargetGuid();
        string plistPath = Path.Combine(path, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        PlistElementDict rootDict = plist.root;
        PlistElementArray urlTypes = rootDict.CreateArray("CFBundleURLTypes");
        PlistElementDict urlTypeDict = urlTypes.AddDict();
        urlTypeDict.SetString("CFBundleTypeRole", "Editor");
        PlistElementArray urlSchemes = urlTypeDict.CreateArray("CFBundleURLSchemes");



        urlSchemes.AddString(settings.iosURLSchema); // 여기에 원하는 URL Scheme을 입력합니다.

        plist.WriteToFile(plistPath);
    }
}


#endif