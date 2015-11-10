using System;
using System.Reflection;

[assembly: AssemblyProduct("OtherEngine")]
[assembly: AssemblyTitle("OtherEngine.ES")]
[assembly: AssemblyCopyright("copygirl")]

[assembly: AssemblyVersion("1.0")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: CLSCompliant(true)]

