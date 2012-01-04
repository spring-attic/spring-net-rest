#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using Spring.Http.Client;

namespace Spring.Rest.Client.Testing
{
    /// <summary>
    /// Matches the given request message against the expectations.
    /// </summary>
    /// <param name="request">The request to make assertions on.</param>
    /// <exception cref="AssertionException">If expectations are not met.</exception>
    /// <author>Bruno Baia (.NET)</author>
    /// <author>Lukas Krecan</author>
    /// <author>Arjen Poutsma</author>
    /// <author>Craig Walls</author>
    public delegate void RequestMatcher(IClientHttpRequest request);
}
