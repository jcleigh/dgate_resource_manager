using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using DGateResourceManager.ViewModels;

namespace DGateResourceManager.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OpenDirectory_Click(object sender, RoutedEventArgs e)
    {
        var storageProvider = StorageProvider;
        
        var folder = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Death Gate directory",
            AllowMultiple = false
        });

        if (folder.Count > 0 && DataContext is MainWindowViewModel viewModel)
        {
            var path = folder[0].Path.LocalPath;
            await viewModel.OpenDirectoryAsync(path);
        }
    }

    private void ResourceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel && e.AddedItems.Count > 0)
        {
            // Selection is handled by binding
        }
    }
}