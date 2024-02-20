using System.Linq.Dynamic.Core;
using CoreInspect.Core.Framework.Utils;

namespace CoreInspect.Core.Framework.Tests;

public class JoinBuilderTests
{
    [Fact]
    public void CustomJoin_of_JoinBuilder()
    {
        IQueryable foos = new Foo[] { new Foo { ID = 1, Text = "Phoeey" } }.AsQueryable();

        IQueryable bars = new Bar[] { new Bar { ID = 5, FooID = 1, Text = "Barnacles" } }.AsQueryable();


        var  query = foos.CustomJoin("f", bars, "b", "f.ID", "b.FooID", "New(f as Foo, b as Bar)");
        var result = query.ToDynamicList();
        result[0].Bar.ID = 5;
        result[0].Foo.ID = 1;
    }
}


public class Foo
{
    public int ID;

    public string Text;
}

public class Bar
{

    public int ID;

    public int? FooID;

    public string Text;

}