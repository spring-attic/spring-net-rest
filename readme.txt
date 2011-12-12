The Spring.NET REST Client project, Release 1.1.0 GA (TO BE DEFINED)
--------------------------------------------------------------------
http://www.springframework.net/rest/


1. INTRODUCTION

The 1.1.0 release of Spring.NET REST Client contains

	* A RestTemplate class for client-side access to RESTful services.
	* A set of HTTP message converters used to marshal objects into the HTTP request body 
	  and to unmarshal any response back into an object.


2. SUPPORTED FRAMEWORK VERSIONS

Spring.NET REST Client supports

	* .NET 2.0
	* .NET Client Profile 3.5 and 4.0
	* .NET Compact Framework 3.5
	* Silverlight 3.0 and 4.0
	* Windows Phone 7.0 and 7.1


3. KNOWN ISSUES

None


4. RELEASE INFO

Release contents:

	* "bin" contains the distribution dll files.
	* "doc" contains reference documentation and MSDN-style API help.
	* "examples" contains sample applications.
	* "lib" contains common libraries needed for building and running the framework.
	* "src" contains the C# source files for the framework.
	* "test" contains the C# source files for the test suite.

The VS.NET solution for the framework and examples are provided.

Latest info is available at the public website: http://www.springframework.net/

Spring.NET REST Client is released under the terms of the Apache Software License (see license.txt).


5. DISTRIBUTION DLLs

The "bin" directory contains the following distinct dll files for use in applications. 
Dependencies are those other than on the .NET BCL.

	* "Spring.Rest" for all supported Framework versions
	* Dependencies: Common.Logging for .NET 2.0, 3.5 and 4.0

	* "Spring.Http.Converters.NJson" for all supported Framework versions
	* Dependencies: Newtonsoft.Json

	* "Spring.Http.Converters.Xml.Linq" for Silverlight
	* "Spring.Http.Converters.Feed" for Silverlight

Debug build is done using /DEBUG:full and release build using /DEBUG:pdbonly flags.


6. WHERE TO START?

Documentation can be found in the "docs" directory.
Samples can be found int the "examples" directory.


7. HOW TO BUILD

There is a solution file for different version of VS.NET
	* Spring.Rest.2005-NET20.sln for use with VS.NET 2005 and .NET 2.0
	* Spring.Rest.2008-NET35.sln for use with VS.NET 2008 and .NET 3.5
	* Spring.Rest.2008-CF35.sln  for use with VS.NET 2008 and Compact Framework 3.5
	* Spring.Rest.2008-SL30.sln  for use with VS.NET 2008 and Silverlight 3.0 (Requires Silverlight tools)
	* Spring.Rest.2010-NET40.sln for use with VS.NET 2010 and .NET 4.0
	* Spring.Rest.2010-SL40.sln  for use with VS.NET 2010 and Silverlight 4.0 (Requires Silverlight tools)
	* Spring.Rest.2010-WP70.sln  for use with VS.NET 2010 and Windows Phone 7.0 (Requires Windows Phone tools)
	* Spring.Rest.2010-WP71.sln  for use with VS.NET 2010 and Windows Phone 7.1 (Requires Windows Phone tools)


8. SUPPORT

The user forums at http://forum.springframework.net/ are available for you to submit questions, support requests, and interact with other Spring.NET users.

Bug, issue and project tracking can be found at https://jira.springsource.org/browse/SPRNETREST

A source repository browser is located at https://fisheye.springsource.org/changelog/spring-net-rest

A continuous intergration and build server is located at http://build.springframework.org/browse/SPRNETREST

To get the sources, check them out at the git repository at https://github.com/SpringSource/spring-net-rest using a git client to deal with line endings.

We are always happy to receive your feedback on the forums. If you think you found a bug, have an improvement suggestion
or feature request, please submit a ticket in JIRA (see link above).


9. Contributing to Spring.NET REST Client

Github is for social coding: if you want to write code, we encourage contributions through pull requests from forks of this repository (see http://help.github.com/forking/). 
Before we accept a non-trivial patch or pull request we will need you to sign the contributor's agreement (see https://support.springsource.com/spring_committer_signup). 
Signing the contributor's agreement does not grant anyone commit rights to the main repository, but it does mean that we can accept your contributions, and you will get an author credit if we do. 
Active contributors might be asked to join the core team, and given the ability to merge pull requests.
