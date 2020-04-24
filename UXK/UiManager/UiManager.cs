using System;
using System.Collections.Generic;
using UnityEngine;

namespace UXK.UiManager
{
    public interface IUiWindow
    {
        bool Enabled { get; }
        void Hide();
    }

    public interface IUiWindowSimple : IUiWindow
    {
        void Show();
    }

    public interface IUiWindowWithParam<TParam> : IUiWindow
    {
        void ShowFor(TParam param);
    }
    
    public class UiManager : MonoBehaviour
    {
        public static Dictionary<Type, IUiWindowSimple> simpleWindows = new Dictionary<Type, IUiWindowSimple>();
        public static Dictionary<Type, IUiWindow> windowsWithParams = new Dictionary<Type, IUiWindow>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            foreach (Transform child in transform)
            {
                var windowComponents = child.GetComponents<IUiWindow>();
                foreach (var window in windowComponents)
                {
                    var typeOfWindow = window.GetType();
                    if (window is IUiWindowSimple windowSimple)
                        simpleWindows[typeOfWindow] = windowSimple;
                    if (IsSubclassOfRawGeneric(typeof(IUiWindowWithParam<>), typeOfWindow))
                        windowsWithParams[typeOfWindow] = window;
                }
            }
            
            bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
                while (toCheck != null && toCheck != typeof(object)) {
                    var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                    if (generic == cur) {
                        return true;
                    }

                    var interfaces = cur.GetInterfaces();
                    foreach (var @interface in interfaces)
                    {
                        var curInterface = @interface.IsGenericType ? @interface.GetGenericTypeDefinition() : @interface;
                        if (generic == curInterface) return true;
                    }
                    toCheck = toCheck.BaseType;
                }
                return false;
            }
        }

        public static void Show<T>() where T : IUiWindowSimple
        {
            var type = typeof(T);
            if (simpleWindows.TryGetValue(type, out var founded))
                founded.Show();
            else
                Debug.LogError($"Cant find simple window of type {type.Name}");
        }

        public static void Switch<TWindow, TParam>(TParam param) where TWindow : IUiWindowWithParam<TParam>
        {
            var window = Get<TWindow, TParam>();
            if (window.Enabled)
                window.Hide();
            else
                window.ShowFor(param);
        }

        public static void Show<TWindow, TParam>(TParam param) where TWindow : IUiWindowWithParam<TParam> => 
            Get<TWindow, TParam>().ShowFor(param);

        public static TWindow Get<TWindow, TParam>() where TWindow : IUiWindowWithParam<TParam>
        {
            var type = typeof(TWindow);
            if (windowsWithParams.TryGetValue(type, out var founded))
                return ((TWindow) founded);
            else
                throw new ArgumentOutOfRangeException($"Cant find window <b>{type.FullName}</b>");
        }
    }
}