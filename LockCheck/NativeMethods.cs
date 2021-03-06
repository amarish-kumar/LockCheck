﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LockCheck
{
    internal class NativeMethods
    {
        // ReSharper disable InconsistentNaming

        private const string RestartManagerDll = "rstrtmgr.dll";

        [DllImport(RestartManagerDll, CharSet = CharSet.Unicode)]
        internal static extern int RmRegisterResources(uint pSessionHandle,
            uint nFiles,
            string[] rgsFilenames,
            uint nApplications,
            [In] RM_UNIQUE_PROCESS[] rgApplications,
            uint nServices,
            string[] rgsServiceNames);

        [DllImport(RestartManagerDll, CharSet = CharSet.Unicode)]
        internal static extern int RmStartSession(out uint pSessionHandle, 
            int dwSessionFlags, StringBuilder strSessionKey);

        [DllImport(RestartManagerDll)]
        internal static extern int RmEndSession(uint pSessionHandle);

        [DllImport(RestartManagerDll, CharSet = CharSet.Unicode)]
        internal static extern int RmGetList(uint dwSessionHandle,
            out uint pnProcInfoNeeded,
            ref uint pnProcInfo,
            [In, Out] RM_PROCESS_INFO[] rgAffectedApps,
            ref uint lpdwRebootReasons);

        [StructLayout(LayoutKind.Sequential)]
        internal struct FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RM_UNIQUE_PROCESS
        {
            public uint dwProcessId;
            public FILETIME ProcessStartTime;
        }

        internal const int RM_INVALID_SESSION = -1;
        internal const int RM_INVALID_PROCESS = -1;
        internal const int CCH_RM_MAX_APP_NAME = 255;
        internal const int CCH_RM_MAX_SVC_NAME = 63;
        internal const int ERROR_SEM_TIMEOUT = 121;
        internal const int ERROR_BAD_ARGUMENTS = 160;
        internal const int ERROR_MAX_SESSIONS_REACHED = 353;
        internal const int ERROR_WRITE_FAULT = 29;
        internal const int ERROR_OUTOFMEMORY = 14;
        internal const int ERROR_MORE_DATA = 234;
        internal const int ERROR_ACCESS_DENIED = 5;
        internal const int ERROR_INVALID_HANDLE = 6;
        internal const int ERROR_CANCELLED = 1223;


        internal static readonly int RM_SESSION_KEY_LEN = Guid.Empty.ToByteArray().Length; // 16-byte
        internal static readonly int CCH_RM_SESSION_KEY = RM_SESSION_KEY_LEN * 2;

        internal enum RM_APP_TYPE
        {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000
        }

        internal enum RM_APP_STATUS
        {
            RmStatusUnknown = 0x0,
            RmStatusRunning = 0x1,
            RmStatusStopped = 0x2,
            RmStatusStoppedOther = 0x4,
            RmStatusRestarted = 0x8,
            RmStatusErrorOnStop = 0x10,
            RmStatusErrorOnRestart = 0x20,
            RmStatusShutdownMasked = 0x40,
            RmStatusRestartMasked = 0x80
        }

        internal enum RM_REBOOT_REASON
        {
            RmRebootReasonNone = 0x0,
            RmRebootReasonPermissionDenied = 0x1,
            RmRebootReasonSessionMismatch = 0x2,
            RmRebootReasonCriticalProcess = 0x4,
            RmRebootReasonCriticalService = 0x8,
            RmRebootReasonDetectedSelf = 0x10
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct RM_PROCESS_INFO
        {
            public RM_UNIQUE_PROCESS Process;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            public string strAppName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
            public string strServiceShortName;
            public RM_APP_TYPE ApplicationType;
            public uint AppStatus;
            public uint TSSessionId;
            [MarshalAs(UnmanagedType.Bool)]
            public bool bRestartable;
        }

        // ReSharper restore InconsistentNaming
    }
}