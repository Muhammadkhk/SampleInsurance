using CoreInspect.Core.Framework.Utils;

namespace CoreInspect.Core.Framework.Tests;

public class IpExtensionTests
{
    [Fact]
    public void GetIPRange_should_return_list()
    {
        var list = IpExtensions.GetIPRange("10.0.0.0/29");
        Assert.NotNull(list);
        //list.Count
    }
}