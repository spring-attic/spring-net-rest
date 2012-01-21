using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Spring.Rest")]
[assembly: AssemblyDescription("Interfaces and classes that provide REST client API in Spring.NET")]

#if !SILVERLIGHT && !CF_3_5
#if STRONG
[assembly: InternalsVisibleTo("Spring.Rest.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5af6fc0c5a1a93adbee2cb424fe0a23ca6430fe0620dd9a00393be236e5d8ab481c8ad4a68c35a62e474695d63658313e4f35a2f29cd38c072f73227eaec5f0b6fb6f9e0ed6ab4f105a393f709eae6cfd010febebb004dd230d51b5e8aec839b6832c21ec7ac3b6cadba8d9b8870b1ab1507cabea54dcacd2c74ea45231a3bd")]
#else
[assembly: InternalsVisibleTo("Spring.Rest.Tests")]
#endif
#endif