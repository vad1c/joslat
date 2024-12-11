using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Realtime.API.Dotnet.SDK.Core.Model.Function;
using Realtime.API.Dotnet.SDK.Core.Model.Request;
using Realtime.API.Dotnet.SDK.Core.Model.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Realtime.API.Dotnet.SDK.WPF.Sample
{
   public class FucationCall
    {
        public bool HandleWeatherFunctionCall(FuncationCallArgument argument, ClientWebSocket clientWebSocket)
        {
            try
            {
                var name = argument.Name;
                var arguments = argument.Arguments;
                if (!string.IsNullOrEmpty(arguments))
                {

                    var functionCallArgs = JObject.Parse(arguments);
                    var city = functionCallArgs["city"]?.ToString();
                    if (!string.IsNullOrEmpty(city))
                    {
                        var weatherResult = GetWeatherFake(city);
                        SendFunctionCallResult(weatherResult, argument.CallId, clientWebSocket);
                    }
                    else
                    {
                        Console.WriteLine("City not provided for get_weather function.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid arguments.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing function call arguments: {ex.Message}");
            }

            return true;
        }

        public bool HandleNotepadFunctionCall(FuncationCallArgument argument, ClientWebSocket clientWebSocket)
        {
            try
            {
                var name = argument.Name;
                var callId = argument.CallId;
                var arguments = argument.Arguments;
                if (!string.IsNullOrEmpty(arguments))
                {
                    var functionCallArgs = JObject.Parse(arguments);
                    var content = functionCallArgs["content"]?.ToString();
                    var date = functionCallArgs["date"]?.ToString();
                    if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(date))
                    {
                        WriteToNotepad(date, content);
                        SendFunctionCallResult("Write to notepad successful.", callId, clientWebSocket);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid arguments.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing function call arguments: {ex.Message}");
            }
            return true;
        }

        private string GetWeatherFake(string city)
        {
            var weatherResponse = new WeatherResponse
            {
                City = city,
                Temperature = "30°C"
            };

            return JsonConvert.SerializeObject(weatherResponse);
        }

        private void WriteToNotepad(string date, string content)
        {
            try
            {
                string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "temp.txt");

                // Write the date and content to a text file
                File.AppendAllText(filePath, $"Date: {date}\nContent: {content}\n\n");

                // Open the text file in Notepad
                Process.Start("notepad.exe", filePath);
                Console.WriteLine("Content written to Notepad.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to Notepad: {ex.Message}");
            }
        }

        private void SendFunctionCallResult(string result, string callId, ClientWebSocket webSocketClient)
        {
            var functionCallResult = new FunctionCallResult
            {
                Type = "conversation.item.create",
                Item = new FunctionCallItem
                {
                    Type = "function_call_output",
                    Output = result,
                    CallId = callId
                }
            };

            string resultJsonString = JsonConvert.SerializeObject(functionCallResult);

            webSocketClient.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(resultJsonString)), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine("Sent function call result: " + resultJsonString);

            ResponseCreate responseJson = new ResponseCreate();
            string rpJsonString = JsonConvert.SerializeObject(responseJson);

            webSocketClient.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(rpJsonString)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

    }
}
