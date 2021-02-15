using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileOpenPickerWinUIPreview4
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void fileOpenPickerSingleButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = GetFileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".txt");

            StorageFile storageFile = await picker.PickSingleFileAsync();
            string text = string.Empty;
            if (storageFile != null)
            {
                text += storageFile.Path + Environment.NewLine;
            }
            fileOpenPickerSingleTextBlock.Text = text;
        }

        private async void fileOpenPickerMultipleButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = GetFileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".txt");

            IReadOnlyList<StorageFile> filesToOpen = await picker.PickMultipleFilesAsync();
            string text = string.Empty;
            foreach (StorageFile storageFile in filesToOpen)
            {
                text += storageFile.Path + Environment.NewLine;
            }
            fileOpenPickerMultipleTextBlock.Text = text;
        }

        #region Initialize With Window

        public static FileOpenPicker GetFileOpenPicker()
        {
            FileOpenPicker picker = new FileOpenPicker();
            InitializeWithWindow(picker);
            return picker;
        }

        public static FolderPicker GetFolderPicker()
        {
            FolderPicker picker = new FolderPicker();
            InitializeWithWindow(picker);
            return picker;
        }

        public static FileSavePicker GetFileSavePicker()
        {
            FileSavePicker picker = new FileSavePicker();
            InitializeWithWindow(picker);
            return picker;
        }

        public static void InitializeWithWindow(Object obj)
        {
            // When running on win32, FileOpenPicker needs to know the top-level hwnd via IInitializeWithWindow::Initialize.
            if (Window.Current == null)
            {
                IInitializeWithWindow initializeWithWindowWrapper = obj.As<IInitializeWithWindow>();
                IntPtr hwnd = GetActiveWindow();
                initializeWithWindowWrapper.Initialize(hwnd);
            }
        }

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto, PreserveSig = true, SetLastError = false)]
        public static extern IntPtr GetActiveWindow();

        #endregion
    }

    [ComImport, System.Runtime.InteropServices.Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithWindow
    {
        void Initialize([In] IntPtr hwnd);
    }
}
