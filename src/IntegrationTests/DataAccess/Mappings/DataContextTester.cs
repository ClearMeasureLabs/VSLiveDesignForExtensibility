﻿using System;
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
            var context = new StubbedDataContextFactory().GetContext();
            context.Database.Migrate();
        }

        [Test, Explicit]
        public void ShouldDeleteAllDataFromDatabase()
        {
            new DatabaseEmptier().DeleteAllData();
        }
    }
}