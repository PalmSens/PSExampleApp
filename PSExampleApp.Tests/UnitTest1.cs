using Xunit;
using PSHeavyMetal.Core;
using PSHeavyMetal.Common.Models;
using System;
using System.Linq;
using PSHeavyMetal.Core.DataAccess;

namespace PSHeavyMetal.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var datastorage = new LiteDbDataOperations();

            var user = new User { Id = Guid.NewGuid(), Name = "testName", Password = "blah" };
            await datastorage.SaveAsync(user);

            var user2 = new User { Id = Guid.NewGuid(), Name = "testName2", Password = "blah" };
            await datastorage.SaveAsync(user2);
        }
    }
}