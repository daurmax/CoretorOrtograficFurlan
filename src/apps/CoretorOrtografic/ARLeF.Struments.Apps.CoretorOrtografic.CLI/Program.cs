using ARLeF.Struments.Base.Core.Development;
using ARLeF.Struments.Base.Core.Input;
using ARLeF.Struments.Base.Core.Output;
using Autofac;
using System;
using System.Threading;

namespace ARLeF.Struments.Apps.CoretorOrtografic.CLI
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
#if DEBUG
            Container = CoretorOrtograficCliDependencyContainer.Configure(true);
#else
            _container = CoretorOrtograficCliDependencyContainer.Configure(false);
#endif
            ReadContentAndReturnContent();
        }

        public static void ReadContentAndReturnContent()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var reader = scope.Resolve<IContentReader>();
                string text = reader.Read();

                var writer = scope.Resolve<IContentWriter>();
                writer.Write(text);
            }
        }
    }
}
