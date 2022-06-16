using System;
using System.Diagnostics;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.Struments.CoretorOrtografic.Core.Entities.RadixTree;
using NUnit.Framework;
using Autofac.Core;
using Autofac;
using ARLeF.Struments.CoretorOrtografic.Core.Input;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTreeReader;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.RadixTree
{
    public class KeyValueDatabaseFixture
    {
        private static IContainer Container { get; set; }

        [SetUp]
        public void Setup() { }
    }
}
