using System;
using System.IO;

using UnityEditor;

#if UNITY_ANDROID && UNITY_EDITOR
using System.Collections.Generic;

[InitializeOnLoad]
public class ADXLibraryEditorScriptAndroid : AssetPostprocessor {

	/// <summary>Instance of the PlayServicesSupport resolver</summary>
	public static object svcSupport;

	private static readonly string PluginName = "ADXLibrary";

	private static readonly string PLAY_SERVICES_VERSION = "+";
	private static readonly string ANDROID_SUPPORT_VERSION = "25.2.0";

	static ADXLibraryEditorScriptAndroid() {
		createADXLibraryAndroidManifest();
		//addGMSLibrary();
	}

	private static void addGMSLibrary() {
        
		Type playServicesSupport = Google.VersionHandler.FindClass(
			"Google.JarResolver", "Google.JarResolver.PlayServicesSupport");
		if (playServicesSupport == null)
			return;

		svcSupport = svcSupport ?? Google.VersionHandler.InvokeStaticMethod(
			playServicesSupport, "CreateInstance",
			new object[] {
				PluginName,
				EditorPrefs.GetString("AndroidSdkRoot"),
				"ProjectSettings"
			});
            
		Google.VersionHandler.InvokeInstanceMethod(
			svcSupport, "DependOn",
			new object[] {
				"com.google.android.gms",
				"play-services-ads",
				PLAY_SERVICES_VERSION
			},
			namedArgs: new Dictionary<string, object>() {
				{"packageIds", new string[] { "extra-google-m2repository" } }
			});

		Google.VersionHandler.InvokeInstanceMethod(
			svcSupport, "DependOn",
			new object[] {
				"com.android.support",
				"palette-v7",
				ANDROID_SUPPORT_VERSION
			},
			namedArgs: new Dictionary<string, object>() {
				{"packageIds", new string[] { "extra-android-m2repository" } }
			});
		
	}

	// Copies `AndroidManifestTemplate.xml` to `AndroidManifest.xml`
	//   then replace `${applicationId}` with current packagename in the Unity settings.
	private static void createADXLibraryAndroidManifest() {
		string adxLibraryConfigPath = "Assets/Plugins/Android/ADXLibraryConfig/";
		string manifestFullPath = adxLibraryConfigPath + "AndroidManifest.xml";

		File.Copy(adxLibraryConfigPath + "AndroidManifestTemplate.xml", manifestFullPath, true);

		StreamReader streamReader = new StreamReader(manifestFullPath);
		string body = streamReader.ReadToEnd();
		streamReader.Close();

#if UNITY_5_6_OR_NEWER
		body = body.Replace("${applicationId}", PlayerSettings.applicationIdentifier);
#else
body = body.Replace("${manifestApplicationId}", PlayerSettings.bundleIdentifier);
#endif
		using (var streamWriter = new StreamWriter(manifestFullPath, false)) {
			streamWriter.Write(body);
		}
	}
}
#endif