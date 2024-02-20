using System.Net;

namespace Sample.Framework.Utils;

public static class IpExtensions
{
    public static List<string> GetIPRange(string cidr)
    {
        var ipList = new List<string>();
        if (cidr.IndexOfAny(new[] { '/' }) ==-1)
        {
            ipList.Add(cidr);
            return ipList;
        }
        
        var cidrParts = cidr.Split('/');
        var baseIP = cidrParts[0];
        var subnetMaskLength = int.Parse(cidrParts[1]);


        var baseIPNumeric = BitConverter.ToUInt32(IPAddress.Parse(baseIP).GetAddressBytes().Reverse().ToArray(), 0);

        var numberOfAddresses = (int)Math.Pow(2, 32 - subnetMaskLength);

        for (var i = 0; i < numberOfAddresses; i++)
        {
            var currentIPNumeric = baseIPNumeric + (uint)i;

            var currentIPAddress = ToIPAddress(currentIPNumeric);
            ipList.Add(currentIPAddress.ToString());
        }

        return ipList;
    }

    public static IPAddress ToIPAddress(uint address)
    {
        var buffer = new[]
        {
            (byte) ((address >> 24) & 0xFF),
            (byte) ((address >> 16) & 0xFF),
            (byte) ((address >> 8) & 0xFF),
            (byte) (address & 0xFF)
        };
        return new IPAddress(buffer);
    }

    public static IPAddress ToIPAddress(UInt128 address)
    {
        var buffer = new[]
        {
            (byte) ((address >> 120) & 0xFF),
            (byte) ((address >> 112) & 0xFF), (byte) ((address >> 104) & 0xFF),
            (byte) ((address >> 96) & 0xFF), (byte) ((address >> 88) & 0xFF),
            (byte) ((address >> 80) & 0xFF), (byte) ((address >> 72) & 0xFF),
            (byte) ((address >> 64) & 0xFF), (byte) ((address >> 56) & 0xFF),
            (byte) ((address >> 48) & 0xFF), (byte) ((address >> 40) & 0xFF),
            (byte) ((address >> 32) & 0xFF),
            (byte) ((address >> 24) & 0xFF), (byte) ((address >> 16) & 0xFF),
            (byte) ((address >> 8) & 0xFF), (byte) (address & 0xFF)
        };
        return new IPAddress(buffer);
    }

    public static uint ToUInt32(IPAddress address)
    {
        var buffer = address.GetAddressBytes();
        return ((uint)buffer[0] << 24) |
               ((uint)buffer[1] << 16) | ((uint)buffer[2] << 8) |
               buffer[3];
    }

    public static UInt128 ToUInt128(IPAddress address)
    {
        var buffer = address.GetAddressBytes();
        return ((UInt128)buffer[0] << 120) |
               ((UInt128)buffer[1] << 112) | ((UInt128)buffer[2] << 104) |
               ((UInt128)buffer[3] << 96) | ((UInt128)buffer[4] << 88) |
               ((UInt128)buffer[5] << 80) | ((UInt128)buffer[6] << 72) |
               ((UInt128)buffer[7] << 64) | ((UInt128)buffer[8] << 56) |
               ((UInt128)buffer[9] << 48) | ((UInt128)buffer[10] << 40) |
               ((UInt128)buffer[11] << 32) | ((UInt128)buffer[12] << 24) |
               ((UInt128)buffer[13] << 16) | ((UInt128)buffer[14] << 8) |
               buffer[15];
    }
}
