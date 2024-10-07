using Blockchain.Models;

namespace Blockchain.Server
{
    public class RegistrationServer
    {
        private readonly Dictionary<string, string> _authorizedDevices = new Dictionary<string, string>();

        public RegistrationServer()
        {
            // Populate the list of authorized devices
            _authorizedDevices.Add("Device1", "PublicKey1");
            _authorizedDevices.Add("Device2", "PublicKey1");
            _authorizedDevices.Add("Device3", "PublicKey1");
        }

        public bool ValidateDevice(Device device, string registrationRequest, string signature)
        {
            if (_authorizedDevices.TryGetValue(device.DeviceId, out var expectedPublicKey))
            {
                // Verify the request signature
                return device.VerifySignature(registrationRequest, signature);
            }
            return false;
        }

        public bool IsDeviceRegistered(string deviceId)
        {
            return _authorizedDevices.ContainsKey(deviceId);
        }

        public void RegisterDevice(Device device)
        {
            if (!_authorizedDevices.ContainsKey(device.DeviceId))
            {
                _authorizedDevices[device.DeviceId] = device.PublicKey;
                Console.WriteLine($"Device {device.DeviceId} registered successfully.");
            }
            else
            {
                Console.WriteLine($"Device {device.DeviceId} is already registered.");
            }
        }
    }

}
