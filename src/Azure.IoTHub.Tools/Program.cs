using System;
using System.Net;
using Microsoft.Azure.Devices.Common.Security;

namespace Azure.IoTHub.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("*** Azure IoT Hub SAS Token Generator ***\n");

                    Console.WriteLine("IoT Hub host name? (e.g. abcdefg-azure.devices.net)");
                    var iotHubHostName = Console.ReadLine();

                    Console.WriteLine("\nDevice Id?");
                    var deviceId = Console.ReadLine();
                    var keyResult = "";
                    do
                    {
                        Console.WriteLine(
                            "\nDo you want to sign the SAS token with a (D)evice Symmetric Key or a (S)hared Accesss Policy Key?");
                        keyResult = Console.ReadKey().Key.ToString().ToLower();
                    } while (!(keyResult == "d" || keyResult == "s"));


                    var sharedAccessPolicyName = "";
                    if (keyResult == "s")
                    {
                        Console.WriteLine("\n\nName of the Shared Access Policy?");
                        sharedAccessPolicyName = Console.ReadLine();
                    }

                    Console.WriteLine(
                        "\nKey that will be used sign the SAS token? This value should be the Device Symmetric Key or the Shared Access Policy Key");
                    var key = Console.ReadLine();

                    Console.WriteLine("\nHow many minutes do you want the SAS token to be valid for?");
                    var ttlValue = Console.ReadLine();

                    var sasBuilder = new SharedAccessSignatureBuilder()
                    {
                        Key = key,
                        Target = $"{iotHubHostName}/devices/{WebUtility.UrlEncode(deviceId)}",
                        TimeToLive = TimeSpan.FromMinutes(Convert.ToDouble(ttlValue)),
                    };

                    if (!string.IsNullOrEmpty(sharedAccessPolicyName))
                        sasBuilder.KeyName = sharedAccessPolicyName;
                    Console.WriteLine("\nSAS Token:\n");
                    var sasToken = sasBuilder.ToSignature();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(sasToken);
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to create another SAS Token");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Something went wrong. Exception: {ex.Message}");
                }
                finally
                {
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}
