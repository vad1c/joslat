## **Getting Started**

### **Step 1: Import the Core and WPF SDK**

```c#
using Navbot.RealtimeApi.Dotnet.SDK.Core;
using Navbot.RealtimeApi.Dotnet.SDK.WPF;
```

### Step 2: Use RealtimeApiWpfControl in XAML

```xml
<Window x:Class="YourNamespace.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:wpf="clr-namespace:Navbot.RealtimeApi.Dotnet.SDK.WPF"
        Title="MainWindow" Height="450" Width="800">
    <Grid>
        <wpf:RealtimeApiWpfControl x:Name="realtimeApiWpfControl" />
    </Grid>
</Window>

```

### Step 3: Get OPENAI_API_KEY

```c#
private void Window_Loaded(object sender, RoutedEventArgs e)
{
    string openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
    realtimeApiWpfControl.OpenAiApiKey = openAiApiKey;
}
```

### **Step 4: Start Processing Audio**

```c#
realtimeApiWpfControl.StartSpeechRecognition();
```

### **Step 5: End Processing Audio**

```c#
realtimeApiWpfControl.StopSpeechRecognition();
```

------

## **License**

Licensed under the [MIT](LICENSE) License.