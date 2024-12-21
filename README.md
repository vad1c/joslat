# Project Name
OpenAI.RealtimeApi.Dotnet.SDK

## Project Introduction

The Realtime control is a component for real-time speech recognition and speech synthesis. It can convert speech input into text and generate speech responses from text. This control can be used in desktop applications, integrating OpenAI and other related services, providing a seamless speech interaction experience.

This control is suitable for applications requiring voice interaction, such as virtual assistants, speech recognition systems, and intelligent customer service.

## Table of Contents

- [Feature Overview](#feature-overview)
- [Installation and Configuration](#installation-and-configuration)
- [Usage](#usage)
- [Feature Demonstration](#feature-demonstration)
- [License](#license)

## Feature Overview

The Realtime control provides the following key features:

- Real-time Speech Recognition: Converts user speech into text in real time, supporting continuous speech input and feedback.
- Text-to-Speech: Converts AI or other text information into speech and plays it.
- Multi-language Support: Supports speech recognition in multiple languages.
- OpenAI Integration: Integrates the OpenAI API, supporting intelligent conversation and natural language processing.
- Custom Features: Allows developers to customize API calls and speech-related functionalities.

## Installation and Configuration

### System Requirements

List the basic environment requirements needed to run the project:

- Operating System: Windows 10 or higher
- .NET Version: .NET 6.0 or higher
- Other Dependencies: OpenAI API key, NAudio, etc.

### Usage

### Initialize the Control

In a `Windows Forms` application, you can initialize the control and start using it as follows:

```c#
using Realtime.API.Dotnet.SDK.Core.Model;

public partial class MainForm : Form
{
    private RealtimeApiDesktopControl realtimeApiDesktopControl = new RealtimeApiDesktopControl();

    public MainForm()
    {
        InitializeComponent();
        Init();
    }
    
    public void Init()
    {
        realtimeApiDesktopControl.SpeechTextAvailable += RealtimeApiDesktopControl_SpeechTextAvailable;
        realtimeApiDesktopControl.PlaybackTextAvailable += RealtimeApiDesktopControl_PlaybackTextAvailable;
    }
    
    private void MainFrom_Load(object sender, EventArgs e)
	{
    	string openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
    	realtimeApiDesktopControl.OpenAiApiKey = openAiApiKey;
	}

    private void RealtimeApiDesktopControl_SpeechTextAvailable(object sender, TranscriptEventArgs e)
    {
        // Handle speech recognition result
        Console.WriteLine($"User: {e.Transcript}");
    }

    private void RealtimeApiDesktopControl_PlaybackTextAvailable(object sender, TranscriptEventArgs e)
    {
        // Handle speech playback result
        Console.WriteLine($"AI: {e.Transcript}");
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
        // Start speech recognition
        realtimeApiDesktopControl.StartSpeechRecognition();
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        // Stop speech recognition
        realtimeApiDesktopControl.StopSpeechRecognition();
    }
}

```

### Feature Demonstration

1. **Speech Recognition**: Click the "Start" button to begin listening to the user's speech and convert it into text in real time.
2. **Speech Text**: By calling `RealtimeApiDesktopControl.PlaybackTextAvailable`, the output text information of the AI speech is displayed.
3. ![img](images/sample.png)

## License

This project is licensed under the [MIT License](LICENSE).
