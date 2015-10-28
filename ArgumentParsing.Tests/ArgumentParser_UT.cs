using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ArgumentParsing.Tests
{
    [TestFixture]
    public class ArgumentParser_UT
    {
        [Test]
        public void ArgumentParser_Calls_TrySetArgument_On_Each_Argument_Given()
        {
            var argument = new Argument<int>(new List<string> { "someInt" });
        }
    }
}
