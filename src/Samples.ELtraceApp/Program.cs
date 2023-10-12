using System;
using System.Diagnostics;
using System.Threading;
using System.Security.Principal;
using System.IO;
using System.Text;

namespace Samples.ELtraceApp
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			try
			{

				WindowsPrincipal winprincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
				//Válidamos que sea Administrator
				if (winprincipal.IsInRole(WindowsBuiltInRole.Administrator))
				{
					string logSource = "EventLogtraceApp";
					string logName = "Event Log Test App";
					if (!EventLog.SourceExists("EventLogtraceApp"))
						EventLog.CreateEventSource(logSource, logName);
					//quitamos el log de texto
					Trace.Listeners.Remove("myTextLogListener");
					Trace.Listeners.Add(new EventLogTraceListener("myEventLogListener"));
				}
				else {
					//quitamos el log eventviewer
					Trace.Listeners.Remove("myEventLogListener");
					string fname = "log.txt";
					FileStream stream = new FileStream(fname, FileMode.OpenOrCreate
					| FileMode.Append, FileAccess.Write);
					Trace.Listeners.Add(new TextWriterTraceListener(stream, "myTextLogListener"));
				}
				Console.WriteLine("Iniciar la aplicación...");
				PrintfLog("Iniciar la aplicación...");
				Console.WriteLine("Ejecutando la aplicación");
				PrintfLog("Ejecutando la aplicación....");
				Thread.Sleep(1000);
				Console.Write("Presione cualquier tecla para continuar. . . ");
				Console.ReadKey(true);
				PrintfLog("Saliendo de la aplicación...");
				Thread.Sleep(500);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);

			}
		}

		static void PrintfLog(string msg)
		{
			Trace.WriteLine(msg);
			Trace.Flush();
		}
	}
}
