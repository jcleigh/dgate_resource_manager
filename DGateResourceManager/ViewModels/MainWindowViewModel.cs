using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using DGateResourceManager.Models;

namespace DGateResourceManager.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IResourceManager _resourceManager;
    private string _selectedDirectory = string.Empty;
    private IResourceModel? _selectedResource;
    private string _statusMessage = "Ready";

    public MainWindowViewModel(IResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
        Resources = new ObservableCollection<IResourceModel>();
        
        OpenDirectoryCommand = new RelayCommand(() => OpenDirectory());
        RefreshCommand = new RelayCommand(async () => await RefreshResourcesAsync());
    }

    public ObservableCollection<IResourceModel> Resources { get; }

    public string SelectedDirectory
    {
        get => _selectedDirectory;
        set
        {
            if (_selectedDirectory != value)
            {
                _selectedDirectory = value;
                OnPropertyChanged();
            }
        }
    }

    public IResourceModel? SelectedResource
    {
        get => _selectedResource;
        set
        {
            if (_selectedResource != value)
            {
                _selectedResource = value;
                OnPropertyChanged();
                OnResourceSelected();
            }
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            if (_statusMessage != value)
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand OpenDirectoryCommand { get; }
    public ICommand RefreshCommand { get; }

    private Task OpenDirectory()
    {
        try
        {
            // This would need to be called from the view with proper storage provider
            StatusMessage = "Opening directory...";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        return Task.CompletedTask;
    }

    public async Task OpenDirectoryAsync(string directoryPath)
    {
        try
        {
            if (!_resourceManager.IsValidResourceDirectory(directoryPath))
            {
                StatusMessage = "Selected directory does not contain Death Gate resource files.";
                return;
            }

            SelectedDirectory = directoryPath;
            StatusMessage = "Loading resources...";

            var resources = await _resourceManager.LoadDirectoryAsync(directoryPath);
            
            Resources.Clear();
            foreach (var resource in resources)
            {
                Resources.Add(resource);
            }

            StatusMessage = $"Loaded {resources.Count} resources from {Path.GetFileName(directoryPath)}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading directory: {ex.Message}";
        }
    }

    private async Task RefreshResourcesAsync()
    {
        if (!string.IsNullOrEmpty(SelectedDirectory))
        {
            await OpenDirectoryAsync(SelectedDirectory);
        }
    }

    private void OnResourceSelected()
    {
        if (SelectedResource != null)
        {
            StatusMessage = $"Selected: {SelectedResource.Name} ({SelectedResource.Type}, {SelectedResource.Size} bytes)";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class RelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    public async void Execute(object? parameter)
    {
        await _execute();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}