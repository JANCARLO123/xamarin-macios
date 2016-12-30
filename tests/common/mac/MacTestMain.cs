using System;
using System.Collections.Generic;
#if XAMCORE_2_0
using AppKit;
using Foundation;
#else
using MonoMac.AppKit;
using MonoMac.Foundation;
#endif
using GuiUnit;
using NUnit.Framework;

// HACK
namespace NUnit.Framework.SyntaxHelpers
{
}

namespace Xamarin.Mac.Tests
{	
	static class MainClass
	{
		static void Main(string[] args)
		{
			NSApplication.Init();
			NSRunLoop.Main.InvokeOnMainThread(RunTests);
			NSApplication.Main(args);
		}

		static void RunTests()
		{
			TestRunner.MainLoop = new NSRunLoopIntegration();
			List<string> args = new List<string> () { typeof(MainClass).Assembly.Location, "-labels", "-noheader" };

			string testName = System.Environment.GetEnvironmentVariable ("XM_TEST_NAME");
			if (testName != null)
				args.Add ($"-test={testName}");

			if (System.Environment.GetEnvironmentVariable ("XM_BCL_TEST") != null)
				args.Add ("-exclude=MobileNotWorking,NotOnMac,NotWorking,ValueAdd,CAS,InetAccess,NotWorkingInterpreter");
			Console.WriteLine (args);
			TestRunner.Main (args.ToArray ());
		}

		class NSRunLoopIntegration : NSObject, IMainLoopIntegration
		{
			public void InitializeToolkit()
			{
			}

			public void RunMainLoop()
			{
			}

			public void InvokeOnMainLoop(InvokerHelper helper)
			{
				NSApplication.SharedApplication.InvokeOnMainThread(helper.Invoke);
			}

			public void Shutdown()
			{
				Environment.Exit(TestRunner.ExitCode);
			}
		}
	}
}
