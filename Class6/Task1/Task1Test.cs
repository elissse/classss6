using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task1.Task1;

namespace Task1;

public class Tests
{
    
    [Test]
    public void ApplyOperationTest()
    {
        That(ApplyOperation('*', 5, 2), Is.EqualTo(10));
        That(ApplyOperation('/', 10, 5), Is.EqualTo(2));
        Catch(() => { ApplyOperation('/', 1, 0); });
        Catch<DivisionByZero>(() => { ApplyOperation('/', 1, 0); });
    }


}