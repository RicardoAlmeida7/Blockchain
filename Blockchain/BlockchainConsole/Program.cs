using Blockchain;
using Blockchain.Models;
using Blockchain.Server;

internal class Program
{
    private static void Main(string[] args)
    {
        // Initialize the registry server
        var registrationServer = new RegistrationServer();

        // Set mining difficulty and create blockchain handler
        int difficulty = 4; 
        var blockchainHandler = new BlockchainHandler(registrationServer, difficulty);

        // Create and register the first device
        var device1 = new Device("Device1");
        var registrationRequest1 = $"Register: {device1.DeviceId}";
        var signature1 = device1.SignData(registrationRequest1);
        blockchainHandler.AddDeviceToBlockchain(device1, registrationRequest1, signature1);

        // Add a block for device1
        var data1 = "Data for Device1";
        var signatureData1 = device1.SignData(data1);
        blockchainHandler.AddBlock(data1, device1, signatureData1);

        // Create and register a second device
        var device2 = new Device("Device2");
        var registrationRequest2 = $"Register: {device2.DeviceId}";
        var signature2 = device2.SignData(registrationRequest2);
        blockchainHandler.AddDeviceToBlockchain(device2, registrationRequest2, signature2);

        // Add a block for device2
        var data2 = "Data for Device2";
        var signatureData2 = device2.SignData(data2);
        blockchainHandler.AddBlock(data2, device2, signatureData2);

        var device3 = new Device("Device3");
        var registrationRequest3 = $"Register: {device3.DeviceId}";
        var signature3 = device3.SignData(registrationRequest3);
        blockchainHandler.AddDeviceToBlockchain(device3, registrationRequest3, signature3);

        // Validate the blockchain
        bool isChainValid = blockchainHandler.IsChainValid();
        Console.WriteLine($"Blockchain is valid: {isChainValid}");
    }
}