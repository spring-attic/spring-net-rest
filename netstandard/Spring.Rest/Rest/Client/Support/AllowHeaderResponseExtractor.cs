﻿#region License

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

using System.Collections.Generic;

using Spring.Http;
using Spring.Http.Client;

namespace Spring.Rest.Client.Support
{
    /// <summary>
    /// Response extractor that extracts the response HTTP headers 'Allow'.
    /// </summary>
    /// <author>Bruno Baia</author>
    public class AllowHeaderResponseExtractor : IResponseExtractor<IList<HttpMethod>>
    {
        /// <summary>
        /// Gets called by <see cref="RestTemplate"/> with an opened <see cref="IClientHttpResponse"/> to extract data. 
        /// Does not need to care about closing the request or about handling errors: 
        /// this will all be handled by the <see cref="RestTemplate"/> class.
        /// </summary>
        /// <param name="response">The active HTTP request.</param>
        public IList<HttpMethod> ExtractData(IClientHttpResponse response)
        {
            return new List<HttpMethod>(response.Headers.Allow);
        }
    }
}
