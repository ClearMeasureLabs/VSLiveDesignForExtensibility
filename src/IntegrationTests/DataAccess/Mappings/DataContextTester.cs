using System;
using System.Collections.Generic;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Mappings
{
    [TestFixture]
    public class DataContextTester
    {
        [Test, Explicit]
        public void CreateSchema()
        {
            var context = DataContextFactory.GetEfContext();
            context.Database.Migrate();
            
        }
    }
}