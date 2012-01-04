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

namespace Spring.Rest.Client.Testing
{
    /// <summary>
    /// Main entry point for client-side REST testing. 
    /// Typically used to test a <see cref="RestTemplate"/>, set up expectations on request messages, and create response messages.
    /// </summary>
    /// <remarks>
    /// The typical usage of this class is:
    /// <list type="bullet">
    /// <item>
    /// Create a <see cref="MockRestServiceServer"/> instance by calling <see cref="M:MockRestServiceServer.CreateServer(RestTemplate)"/> method.
    /// </item>
    /// <item>
    /// Set up request expectation by calling <see cref="M:MockRestServiceServer.ExpectNewRequest()"/>. 
    /// More request expectations can be set up by chaining <see cref="M:IRequestActions.AndExpect(RequestMatcher)"/> calls, 
    /// possibly by using the default method extensions provided.
    /// </item>
    /// <item>
    /// Create an appropriate response message by calling <see cref="M:IRequestActions.AndRespond(ResponseCreator)"/>, 
    /// possibly by using the default method extensions provided.
    /// </item>
    /// <item>
    /// Call <see cref="M:MockRestServiceServer.Verify()"/>.
    /// </item>
    /// </list>
    /// Note that because of the 'fluent' API offered by this class (and related classes), 
    /// you can typically use the Code Completion features (i.e. ctrl-space) in your IDE to set up the mocks.
    /// </remarks>
    /// <author>Arjen Poutsma</author>
    /// <author>Lukas Krecan</author>
    /// <author>Craig Walls</author>
    /// <author>Bruno Baia (.NET)</author>
    public class MockRestServiceServer
    {
	    private MockClientHttpRequestFactory mockRequestFactory;

	    private MockRestServiceServer(MockClientHttpRequestFactory mockRequestFactory) 
        {
            // TODO: AssertUtils
            //AssertUtils.ArgumentNotNull(mockRequestFactory, "'mockRequestFactory' must not be null");
		    this.mockRequestFactory = mockRequestFactory;
	    }

        /// <summary>
        /// Creates a <see cref="MockRestServiceServer"/> instance based on the given <see cref="RestTemplate"/>.
        /// </summary>
        /// <param name="restTemplate">The RestTemplate.</param>
        /// <returns>The created server.</returns>
	    public static MockRestServiceServer CreateServer(RestTemplate restTemplate) 
        {
            // TODO: AssertUtils
            //AssertUtils.ArgumentNotNull(restTemplate, "'restTemplate' must not be null");

		    MockClientHttpRequestFactory mockRequestFactory = new MockClientHttpRequestFactory();
		    restTemplate.RequestFactory = mockRequestFactory;

		    return new MockRestServiceServer(mockRequestFactory);
	    }

        /// <summary>
        /// Returns a <see cref="IRequestActions"/> object that allows for creating the response, or to set up request expectations.
        /// </summary>
        /// <returns>The response actions.</returns>
        public IRequestActions ExpectNewRequest() 
        {
		    MockClientHttpRequest request = this.mockRequestFactory.ExpectNewRequest();
		    return request;
	    }

        /// <summary>
        /// Verifies that all request expectations were met.
        /// </summary>
        /// <exception cref="AssertionException">In case of unmet expectations.</exception>
	    public void Verify() 
        {
            this.mockRequestFactory.VerifyRequests();
	    }
    }
}
