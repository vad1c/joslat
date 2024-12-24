## **Getting Started**

### **Step 1: Import the Core and WinForms SDK**

```c#
using Navbot.RealtimeApi.Dotnet.SDK.Core;
using Navbot.RealtimeApi.Dotnet.SDK.WinForm;
```

### **Step 2: Add the RealtimeApiWinFormControl Control**

Drag and drop the `realtimeApiWinFormControl` onto your form or add it programmatically:

```c#
var realtimeApiWinFormControl = new RealtimeApiWinFormControl();
this.Controls.Add(realtimeApiWinFormControl);
```

### **Step 3: Get OPENAI_API_KEY**

```c#
private void MainFrom_Load(object sender, EventArgs e)
{
    string openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
    realtimeApiDesktopControl.OpenAiApiKey = openAiApiKey;
}
```


### **Step 4: Start Processing Audio**

```c#
realtimeApiDesktopControl.StartSpeechRecognition()
```

### **Step 5: End Processing Audio**

```c#
realtimeApiDesktopControl.StopSpeechRecognition();
```

------

## **License**

Licensed under the [MIT](LICENSE) License.