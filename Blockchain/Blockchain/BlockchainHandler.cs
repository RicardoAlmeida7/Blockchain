using Blockchain.Models;
using Blockchain.Server;
using System;
using System.Collections.Generic;

namespace Blockchain
{
    public class BlockchainHandler
    {
        public List<Block> Chain { get; private set; }
        public int Difficulty { get; private set; }

        private readonly RegistrationServer _registrationServer;

        public BlockchainHandler(RegistrationServer registrationServer, int difficulty)
        {
            Chain = new List<Block> { CreateGenesisBlock() };
            Difficulty = difficulty;
            _registrationServer = registrationServer;
        }

        private Block CreateGenesisBlock() => new (0, "0", "Genesis Block", "System");

        public void AddDeviceToBlockchain(Device device, string registrationRequest, string signature)
        {
            if (_registrationServer.ValidateDevice(device, registrationRequest, signature))
            {
                _registrationServer.RegisterDevice(device);
                var block = new Block(Chain.Count, GetLatestBlock().Hash, "Device Registration", device.DeviceId);
                block.MineBlock(Difficulty);
                Chain.Add(block);
                Console.WriteLine($"Device added to blockchain: {block}");
            }
            else
            {
                Console.WriteLine($"Device {device.DeviceId} failed validation.");
            }
        }

        public Block GetLatestBlock() => Chain[^1];

        public bool AuthenticateDevice(Device device, string data, string signature)
        {
            // Check if the device is registered
            var isRegistered = _registrationServer.IsDeviceRegistered(device.DeviceId);

            if (!isRegistered)
            {
                Console.WriteLine($"Device {device.DeviceId} is not registered.");
                return false;
            }

            // Verify the device signature for the given data
            return device.VerifySignature(data, signature);
        }

        public void AddBlock(string data, Device device, string signature)
        {
            if (AuthenticateDevice(device, data, signature))
            {
                var block = new Block(Chain.Count, GetLatestBlock().Hash, data, device.DeviceId);
                block.MineBlock(Difficulty);
                Chain.Add(block);
                Console.WriteLine($"Block added: {block}");
            }
            else
            {
                Console.WriteLine($"Device {device.DeviceId} is not authenticated.");
            }
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    Console.WriteLine($"Block {i} has been tampered with.");
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    Console.WriteLine($"Block {i} has invalid previous hash.");
                    return false;
                }
            }
            return true;
        }
    }
}
