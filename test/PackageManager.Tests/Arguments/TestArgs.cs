﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PackageManager
{
    [TestClass]
    public class TestArgs
    {
        private Args SerializeAndDeserialize(Args args)
        {
            string raw = args.ToString();
            string[] parts = raw.Split(' ');
            parts = parts.Select(p => p.Trim('"')).ToArray();

            return new Args(parts);
        }

        private void TestAndCompare(Func<Args, Args> handler)
        {
            string path = @"C:\Temp";
            string moniker1 = "A";
            string moniker2 = "B";
            var dependency1 = new Args.Dependency("A", "v1.0.0");
            var dependency2 = new Args.Dependency("B", "v2.0.0");
            string selfPackageId = "C";
            string processToKill1 = "D.exe";
            string processToKill2 = "E.exe";
            string selfOriginalPath = @"C:\F.exe";
            bool isSelfUpdate = true;
            string selfUpdateVersion = "1.0.0";

            var args = new Args();
            args.Path = path;
            args.Monikers = new List<string>() { moniker1, moniker2 };
            args.Dependencies = new List<Args.Dependency>() { dependency1, dependency2 };
            args.SelfPackageId = selfPackageId;
            args.ProcessNamesToKillBeforeChange = new List<string>() { processToKill1, processToKill2 };
            args.SelfOriginalPath = selfOriginalPath;
            args.IsSelfUpdate = isSelfUpdate;
            args.SelfUpdateVersion = selfUpdateVersion;

            args = handler(args);
            Assert.AreEqual(path, args.Path);

            Assert.AreEqual(2, args.Monikers.Count);
            Assert.AreEqual(moniker1, args.Monikers.First());
            Assert.AreEqual(moniker2, args.Monikers.Last());

            Assert.AreEqual(2, args.Dependencies.Count);
            Assert.AreEqual(dependency1, args.Dependencies.First());
            Assert.AreEqual(dependency2, args.Dependencies.Last());

            Assert.AreEqual(selfPackageId, args.SelfPackageId);

            Assert.AreEqual(2, args.ProcessNamesToKillBeforeChange.Count);
            Assert.AreEqual(processToKill1, args.ProcessNamesToKillBeforeChange.First());
            Assert.AreEqual(processToKill2, args.ProcessNamesToKillBeforeChange.Last());

            Assert.AreEqual(selfOriginalPath, args.SelfOriginalPath);
            Assert.AreEqual(isSelfUpdate, args.IsSelfUpdate);
            Assert.AreEqual(selfUpdateVersion, args.SelfUpdateVersion);
        }

        [TestMethod]
        public void All() => TestAndCompare(SerializeAndDeserialize);

        [TestMethod]
        public void Clone() => TestAndCompare(args => args.Clone());
    }
}
