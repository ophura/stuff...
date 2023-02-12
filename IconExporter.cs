namespace ophura.jp
{
    using System.Runtime.InteropServices;
    using static WindowsNativeMethods;
    using System.Drawing.Imaging;
    using System.Drawing;
    using UnityEngine;
    using UnityEditor;
    using System;
    
    
    // umm... it's in the name...
    internal sealed class IconExporter : EditorWindow
    {
        [MenuItem("Tools/Procat")]
        private static void FocusOrDisplayWindow() =>
            GetWindow<IconExporter>(false, "Icon Exporter", true);
        
        
        private void OnGUI()
        {
            path = EditorGUILayout.TextField("icon path", path);
            if (string.IsNullOrEmpty(path)) return;
            
            destination = EditorGUILayout.TextField("export path", destination);
            if (string.IsNullOrEmpty(destination)) return;
            
            format = (ImageFormatType)EditorGUILayout.EnumPopup("image format", format);
            
            hasIndex = EditorGUILayout.Toggle("by index", hasIndex);
            
            if (hasIndex) index = EditorGUILayout.IntField(index);
            
            if (GUILayout.Button("extract icon"))
            {
                Icon originalIcon = GetIconFromDll(path, hasIndex ? index : 0);

                Icon resizedIcon = new Icon(originalIcon, 64, 64);

                resizedIcon.ToBitmap().Save(destination, GetImageFormat(format));
            }
        }
        
        
        private int index;
        private string path;
        private bool hasIndex;
        private string destination;
        private ImageFormatType format;
        
        
        private ImageFormat GetImageFormat(ImageFormatType formatType)
        {
            switch (formatType)
            {
                case ImageFormatType.Bmp:
                    return ImageFormat.Bmp;

                case ImageFormatType.Icon:
                    return ImageFormat.Icon;

                case ImageFormatType.Jpeg:
                    return ImageFormat.Jpeg;

                default: return ImageFormat.Png;
            }
        }
        
        
        private Icon GetIconFromDll(string path, int index)
        {
            IntPtr iconHandle = ExtractIcon(IntPtr.Zero, path, index);
            
            Icon icon = Icon.FromHandle(iconHandle);
            
            _ = DestroyIcon(iconHandle);
            
            return icon;
        }
        
        
        private enum ImageFormatType
        {
            Png, Bmp, Jpeg, Icon
        }
    }
    
    
    internal class WindowsNativeMethods
    {
        [DllImport("shell32.dll")]
        internal static extern IntPtr ExtractIcon(IntPtr hInst, string path, int index);
        
        
        [DllImport("user32.dll")]
        internal static extern bool DestroyIcon(IntPtr hIcon);
    }
}
