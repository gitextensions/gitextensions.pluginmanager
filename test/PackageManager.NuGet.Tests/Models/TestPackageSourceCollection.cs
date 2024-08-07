﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Configuration;
using System;
using System.IO;
using System.Linq;

namespace PackageManager.Models
{
    [TestClass]
    public class TestPackageSourceCollection
    {
        public const string ConfigFilePath = "PackageSource.config";

        private static NuGetPackageSourceCollection CreateSourceCollection() 
            => new NuGetPackageSourceCollection(new PackageSourceProvider(new Settings(Environment.CurrentDirectory, ConfigFilePath)));

        private static void EnsureConfigDeleted()
        {
            string path = Path.Combine(Environment.CurrentDirectory, ConfigFilePath);

            if (File.Exists(path))
                File.Delete(path);
        }

        [TestMethod]
        public void CreateNewConfig()
        {
            EnsureConfigDeleted();

            var sources = CreateSourceCollection();
            Assert.IsNull(sources.Primary);
            Assert.AreEqual(1, sources.All.Count);
        }

        [TestMethod]
        public void Crud()
        {
            EnsureConfigDeleted();

            var sources = CreateSourceCollection();
            Assert.IsNull(sources.Primary);
            Assert.AreEqual(1, sources.All.Count);

            // Because NuGet.Client by default creates a config with nuget.org source.
            sources.Remove(sources.All.First());
            Assert.AreEqual(0, sources.All.Count);

            // Create.
            var sourceUri = new Uri("https://wwww.nuget.org", UriKind.Absolute);
            var sourceName = "NuGet.org";
            var source = sources.Add().Name(sourceName).Uri(sourceUri).Save();
            Assert.AreEqual(sourceName, source.Name);
            Assert.AreEqual(sourceUri, source.Uri);
            Assert.AreEqual(1, sources.All.Count);

            // Mark As Primary.
            sources.MarkAsPrimary(source);
            Assert.IsNotNull(sources.Primary);

            sources = CreateSourceCollection();
            Assert.IsNotNull(sources.Primary);
            Assert.AreEqual(1, sources.All.Count);

            source = sources.All.First();
            Assert.AreEqual(sourceName, source.Name);
            Assert.AreEqual(sourceUri, source.Uri);

            // "Remove" As Primary.
            sources.MarkAsPrimary(null);
            Assert.IsNull(sources.Primary);

            // Edit
            source = sources.Edit(sources.All.First()).Name("NuGet").Save();
            Assert.AreEqual(1, sources.All.Count);
            Assert.AreEqual("NuGet", source.Name);
            Assert.AreEqual(sourceUri, source.Uri);

            sources = CreateSourceCollection();
            Assert.AreEqual("NuGet", source.Name);
            Assert.AreEqual(sourceUri, source.Uri);

            // Remove.
            source = sources.All.First();
            sources.Remove(source);
            Assert.AreEqual(0, sources.All.Count);

            sources = CreateSourceCollection();
            Assert.IsNull(sources.Primary);
            Assert.AreEqual(0, sources.All.Count);
        }

        [TestMethod]
        public void Move()
        {
            EnsureConfigDeleted();

            var sources = CreateSourceCollection();
            sources.Remove(sources.All.First());
            var test1 = sources.Add().Name("Test1").Uri(new Uri("http://test1")).Save();
            var test2 = sources.Add().Name("Test2").Uri(new Uri("http://test2")).Save();

            sources = CreateSourceCollection();
            test1 = sources.All.First();
            test2 = sources.All.Last();
            Assert.AreEqual("Test1", test1.Name);
            Assert.AreEqual("Test2", test2.Name);

            // Failed move down.
            int index = sources.MoveDown(test2);
            Assert.AreEqual(1, index);

            // Success move up.
            index = sources.MoveUp(test2);
            Assert.AreEqual(0, index);

            sources = CreateSourceCollection();
            test1 = sources.All.Last();
            test2 = sources.All.First();
            Assert.AreEqual("Test1", test1.Name);
            Assert.AreEqual("Test2", test2.Name);

            // Failed move up.
            index = sources.MoveUp(test2);
            Assert.AreEqual(0, index);

            // Success move up.
            index = sources.MoveDown(test2);
            Assert.AreEqual(1, index);

            sources = CreateSourceCollection();
            test1 = sources.All.First();
            test2 = sources.All.Last();
            Assert.AreEqual("Test1", test1.Name);
            Assert.AreEqual("Test2", test2.Name);
        }
    }
}
