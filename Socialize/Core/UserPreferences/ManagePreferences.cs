using Microsoft.Win32;
using Newtonsoft.Json;
using System;

namespace UnifyMe.Core.UserPreferences
{
    public enum Preferences
    {
        None = 0,
        NotAllowNotify,
        MinimizeOnClose,
        HideTopBar,
        LaunchFullScreen
    }

    public static class ManagePreferences
    {
        private static RegistryKey localSettings;

        public static bool GetPreferences(Preferences preference)
        {
            return GetLocalKey<bool>(preference.ToString());
        }

        public static void SetPreferences(Preferences preference, bool value)
        {
            SetLocalKey(preference.ToString(), value);
        }

        public static M GetModel<M>(string name)
        {
            return (M)GetLocalKey<M>(name);
        }

        public static void SaveModel<M>(string modelName, object Model)
        {
            SetLocalKey(modelName, Model);
        }

        public static R GetPropertyFromModel<M, R>(string modelName, string modelProperty, object defaultValue = null)
        {
            object instanceModel = GetModel<M>(modelName);
            if (instanceModel == null && defaultValue != null)
                return (R)defaultValue;

            if (instanceModel == null)
                return default(R);

            Type model = instanceModel.GetType();
            object val = model.GetProperty(modelProperty)?.GetValue(instanceModel);

            if (val == null && defaultValue != null)
                return (R)defaultValue;

            if (val == null)
                return default(R);

            return (R)val;
        }

        private static T GetLocalKey<T>(string name)
        {
            CheckProperty();
            object localKeyValue = localSettings.GetValue(name);
            if (localKeyValue != null)
                return JsonConvert.DeserializeObject<T>(localKeyValue.ToString());
            return default(T);
        }

        private static void SetLocalKey(string name, object value)
        {
            CheckProperty();
            localSettings.SetValue(name, JsonConvert.SerializeObject(value));
        }

        private static void CheckProperty()
        {
            if (localSettings == null)
            {
                localSettings = Registry.CurrentUser.OpenSubKey("UnifyMe", true);
                if (localSettings == null)
                    localSettings = Registry.CurrentUser.CreateSubKey("UnifyMe");
            }
        }
    }
}
