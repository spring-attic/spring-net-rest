#if NET_3_5 || SILVERLIGHT
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

using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Spring.Http.Converters.Json
{
    /// <summary>
    /// Implementation of <see cref="IHttpMessageConverter"/> that can read and write JSON 
    /// using <see cref="DataContractJsonSerializer"/>.
    /// </summary>
    /// <remarks>
    /// By default, this converter supports 'application/json' media type. 
    /// This can be overridden by setting the <see cref="P:SupportedMediaTypes"/> property.
    /// </remarks>
    /// <author>Bruno Baia</author>
    [Obsolete("This class has been renamed to DataContractJsonHttpMessageConverter for better consistency.")]
    public class JsonHttpMessageConverter : DataContractJsonHttpMessageConverter
    {
        /// <summary>
        /// Creates a new instance of the <see cref="JsonHttpMessageConverter"/> 
        /// with the media type 'application/json'. 
        /// </summary>
        public JsonHttpMessageConverter() :
            base()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="JsonHttpMessageConverter"/> 
        /// with the media type 'application/json'. 
        /// </summary>
        /// <param name="requiresAttribute">
        /// If <c>true</c>, supports only classes decorated with 
        /// <see cref="DataContractAttribute"/> and <see cref="CollectionDataContractAttribute"/>. 
        /// </param>
        public JsonHttpMessageConverter(bool requiresAttribute) :
            base(requiresAttribute)
        {
        }
    }
}
#endif