using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MSMQ
{
    internal static class LocalAppContext
    {
        private static Dictionary<string, bool> s_switchMap = new Dictionary<string, bool>();
        private static readonly object s_syncLock = new object();
        private static bool s_canForwardCalls = LocalAppContext.SetupDelegate();
        private static LocalAppContext.TryGetSwitchDelegate TryGetSwitchFromCentralAppContext;

        private static bool DisableCaching { get; set; }

        static LocalAppContext()
        {
            AppContextDefaultValues.PopulateDefaultValues();
            LocalAppContext.DisableCaching = LocalAppContext.IsSwitchEnabled("TestSwitch.LocalAppContext.DisableCaching");
        }

        public static bool IsSwitchEnabled(string switchName)
        {
            if (LocalAppContext.s_canForwardCalls && LocalAppContext.TryGetSwitchFromCentralAppContext(switchName, out var flag))
                return flag;
            return LocalAppContext.IsSwitchEnabledLocal(switchName);
        }

        private static bool IsSwitchEnabledLocal(string switchName)
        {
            bool flag1;
            bool flag2;
            lock (LocalAppContext.s_switchMap)
                flag2 = LocalAppContext.s_switchMap.TryGetValue(switchName, out flag1);
            if (flag2)
                return flag1;
            return false;
        }

        private static bool SetupDelegate()
        {
            Type type = typeof(object).Assembly.GetType("System.AppContext");
            if (type == (Type)null)
                return false;
            MethodInfo method = type.GetMethod("TryGetSwitch", BindingFlags.Static | BindingFlags.Public, (Binder)null, new Type[2]
            {
        typeof (string),
        typeof (bool).MakeByRefType()
            }, (ParameterModifier[])null);
            if (method == (MethodInfo)null)
                return false;
            LocalAppContext.TryGetSwitchFromCentralAppContext = (LocalAppContext.TryGetSwitchDelegate)Delegate.CreateDelegate(typeof(LocalAppContext.TryGetSwitchDelegate), method);
            return true;
        }

        [MethodImpl((MethodImplOptions)256)]
        internal static bool GetCachedSwitchValue(string switchName, ref int switchValue)
        {
            if (switchValue < 0)
                return false;
            if (switchValue > 0)
                return true;
            return LocalAppContext.GetCachedSwitchValueInternal(switchName, ref switchValue);
        }

        private static bool GetCachedSwitchValueInternal(string switchName, ref int switchValue)
        {
            if (LocalAppContext.DisableCaching)
                return LocalAppContext.IsSwitchEnabled(switchName);
            bool flag = LocalAppContext.IsSwitchEnabled(switchName);
            switchValue = flag ? 1 : -1;
            return flag;
        }

        internal static void DefineSwitchDefault(string switchName, bool initialValue)
        {
            LocalAppContext.s_switchMap[switchName] = initialValue;
        }

        private delegate bool TryGetSwitchDelegate(string switchName, out bool value);
    }
}
