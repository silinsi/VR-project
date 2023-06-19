using System;
using UnityEditor;

namespace ReadyPlayerMe.Core.Editor
{
    /// <summary>
    ///     This class serves as a way of tracking when the Unity project (editor) is initially opened so that functions can be run at this point.
    /// </summary>
    [InitializeOnLoad]
    public static class EntryPoint
    {
        private const string SESSION_STARTED_KEY = "SessionStarted";

        ///     Event for when package is imported or when project with package is opened.
        public static Action Startup;

        /// <summary>
        ///     This constructor is used to subscribe to the <see cref="EditorApplication.update"/> event.
        /// </summary>
        static EntryPoint()
        {
            EditorApplication.update += Update;
        }

        /// <summary>
        ///     This function is called on every Editor <see cref="EditorApplication.update"/>.
        ///     It is used to trigger moduleUpdater to check for updates when the Unity Project launches.
        /// </summary>
        private static void Update()
        {
            if (SessionState.GetBool(SESSION_STARTED_KEY, false)) return;
            SessionState.SetBool(SESSION_STARTED_KEY, true);

            Startup?.Invoke();
            ModuleUpdater.CheckForUpdates();
            EditorApplication.update -= Update;
        }
    }
}
