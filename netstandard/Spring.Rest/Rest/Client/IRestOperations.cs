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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


using Spring.Http;

namespace Spring.Rest.Client
{
    /// <summary>
    /// Interface specifying a basic set of RESTful operations. 
    /// </summary>
    /// <remarks>
    /// Not often used directly, but a useful option to enhance testability, 
    /// as it can easily be mocked or stubbed.
    /// </remarks>
    /// <see cref="RestTemplate"/>
    /// <author>Arjen Poutsma</author>
    /// <author>Juergen Hoeller</author>
    /// <author>Bruno Baia (.NET)</author>
    public partial interface IRestOperations
    {
        #region Synchronous operations

        #region GET

        /// <summary>
        /// Retrieve a representation by doing a GET on the specified URL. 
        /// The response (if any) is converted and returned.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>The converted object</returns>
        T GetForObject<T>(string url, params object[] uriVariables) where T : class;

        /// <summary>
        /// Retrieve a representation by doing a GET on the specified URL. 
        /// The response (if any) is converted and returned.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>The converted object</returns>
        T GetForObject<T>(string url, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Retrieve a representation by doing a GET on the specified URL. 
        /// The response (if any) is converted and returned.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <returns>The converted object</returns>
        T GetForObject<T>(Uri url) where T : class;

        /// <summary>
        /// Retrieve an entity by doing a GET on the specified URL. 
        /// The response is converted and stored in an <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>The HTTP response message.</returns>
        HttpResponseMessage<T> GetForMessage<T>(string url, params object[] uriVariables) where T : class;

        /// <summary>
        /// Retrieve an entity by doing a GET on the specified URL. 
        /// The response is converted and stored in an <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>The HTTP response message.</returns>
        HttpResponseMessage<T> GetForMessage<T>(string url, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Retrieve an entity by doing a GET on the specified URL. 
        /// The response is converted and stored in an <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <returns>The HTTP response message.</returns>
        HttpResponseMessage<T> GetForMessage<T>(Uri url) where T : class;

        #endregion

        #region HEAD

        /// <summary>
        /// Retrieve all headers of the resource specified by the URI template.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>All HTTP headers of that resource</returns>
        HttpHeaders HeadForHeaders(string url, params object[] uriVariables);

        /// <summary>
        /// Retrieve all headers of the resource specified by the URI template.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>All HTTP headers of that resource</returns>
        HttpHeaders HeadForHeaders(string url, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Retrieve all headers of the resource specified by the URI template.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>All HTTP headers of that resource</returns>
        HttpHeaders HeadForHeaders(Uri url);

        #endregion

        #region POST

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the value of the 'Location' header. 
        /// This header typically indicates where the new resource is stored.
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>The value for the Location header.</returns>
        Uri PostForLocation(string url, object request, params object[] uriVariables);

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the value of the 'Location' header. 
        /// This header typically indicates where the new resource is stored.
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given dictionary.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>The value for the Location header.</returns>
        Uri PostForLocation(string url, object request, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the value of the 'Location' header. 
        /// This header typically indicates where the new resource is stored.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <returns>The value for the Location header.</returns>
        Uri PostForLocation(Uri url, object request);

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the representation found in the response. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>The converted object.</returns>
        T PostForObject<T>(string url, object request, params object[] uriVariables) where T : class;

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the representation found in the response. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given dictionary.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>The converted object.</returns>
        T PostForObject<T>(string url, object request, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the representation found in the response. 
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <returns>The converted object.</returns>
        T PostForObject<T>(Uri url, object request) where T : class;

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>The HTTP response message.</returns>
        HttpResponseMessage<T> PostForMessage<T>(string url, object request, params object[] uriVariables) where T : class;

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given dictionary.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>The HTTP response message.</returns>
        HttpResponseMessage<T> PostForMessage<T>(string url, object request, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>. 
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <returns>The HTTP response message.</returns>
        HttpResponseMessage<T> PostForMessage<T>(Uri url, object request) where T : class;

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>The HTTP response message with no entity.</returns>
        HttpResponseMessage PostForMessage(string url, object request, params object[] uriVariables);

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given dictionary.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>The HTTP response message with no entity.</returns>
        HttpResponseMessage PostForMessage(string url, object request, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Create a new resource by POSTing the given object to the URI template, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>. 
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <returns>The HTTP response message with no entity.</returns>
        HttpResponseMessage PostForMessage(Uri url, object request);

        #endregion

        #region PUT

        /// <summary>
        /// Create or update a resource by PUTting the given object to the URI.
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        void Put(string url, object request, params object[] uriVariables);

        /// <summary>
        /// Create or update a resource by PUTting the given object to the URI.
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given dictionary.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        void Put(string url, object request, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Create or update a resource by PUTting the given object to the URI.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        void Put(Uri url, object request);

        #endregion

        #region DELETE

        /// <summary>
        /// Delete the resources at the specified URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        void Delete(string url, params object[] uriVariables);

        /// <summary>
        /// Delete the resources at the specified URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        void Delete(string url, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Delete the resources at the specified URI.
        /// </summary>
        /// <param name="url">The URL.</param>
        void Delete(Uri url);

        #endregion

        #region OPTIONS

        /// <summary>
        /// Return the value of the Allow header for the given URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>The value of the allow header.</returns>
        IList<HttpMethod> OptionsForAllow(string url, params object[] uriVariables);

        /// <summary>
        /// Return the value of the Allow header for the given URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>The value of the allow header.</returns>
        IList<HttpMethod> OptionsForAllow(string url, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Return the value of the Allow header for the given URI.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The value of the allow header.</returns>
        IList<HttpMethod> OptionsForAllow(Uri url);

        #endregion


        #region Exchange

        /// <summary>
        /// Execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>The HTTP response message.</returns>
        HttpResponseMessage<T> Exchange<T>(string url, HttpMethod method, HttpEntity requestEntity, params object[] uriVariables) where T : class;

        /// <summary>
        /// Execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>The HTTP response message.</returns>
        HttpResponseMessage<T> Exchange<T>(string url, HttpMethod method, HttpEntity requestEntity, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <returns>The HTTP response message.</returns>
        HttpResponseMessage<T> Exchange<T>(Uri url, HttpMethod method, HttpEntity requestEntity) where T : class;

        /// <summary>
        /// Execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>The HTTP response message with no entity.</returns>
        HttpResponseMessage Exchange(string url, HttpMethod method, HttpEntity requestEntity, params object[] uriVariables);

        /// <summary>
        /// Execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>The HTTP response message with no entity.</returns>
        HttpResponseMessage Exchange(string url, HttpMethod method, HttpEntity requestEntity, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <returns>The HTTP response message with no entity.</returns>
        HttpResponseMessage Exchange(Uri url, HttpMethod method, HttpEntity requestEntity);

        #endregion

        #region General execution

        /// <summary>
        /// Execute the HTTP method to the given URI template, preparing the request with the 
        /// <see cref="IRequestCallback"/>, and reading the response with an <see cref="IResponseExtractor{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestCallback">Object that prepares the request.</param>
        /// <param name="responseExtractor">Object that extracts the return value from the response.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>An arbitrary object, as returned by the <see cref="IResponseExtractor{T}"/>.</returns>        
        T Execute<T>(string url, HttpMethod method, IRequestCallback requestCallback, IResponseExtractor<T> responseExtractor, params object[] uriVariables) where T : class;

        /// <summary>
        /// Execute the HTTP method to the given URI template, preparing the request with the 
        /// <see cref="IRequestCallback"/>, and reading the response with an <see cref="IResponseExtractor{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestCallback">Object that prepares the request.</param>
        /// <param name="responseExtractor">Object that extracts the return value from the response.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>An arbitrary object, as returned by the <see cref="IResponseExtractor{T}"/>.</returns>   
        T Execute<T>(string url, HttpMethod method, IRequestCallback requestCallback, IResponseExtractor<T> responseExtractor, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Execute the HTTP method to the given URI template, preparing the request with the 
        /// <see cref="IRequestCallback"/>, and reading the response with an <see cref="IResponseExtractor{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestCallback">Object that prepares the request.</param>
        /// <param name="responseExtractor">Object that extracts the return value from the response.</param>
        /// <returns>An arbitrary object, as returned by the <see cref="IResponseExtractor{T}"/>.</returns>   
        T Execute<T>(Uri url, HttpMethod method, IRequestCallback requestCallback, IResponseExtractor<T> responseExtractor) where T : class;

        #endregion

        #endregion

        

        #region Asynchronous operations (TPL)

        #region GET

        /// <summary>
        /// Asynchronously retrieve a representation by doing a GET on the specified URL. 
        /// The response (if any) is converted.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<T> GetForObjectAsync<T>(string url, params object[] uriVariables) where T : class;

        /// <summary>
        /// Asynchronously retrieve a representation by doing a GET on the specified URL. 
        /// The response (if any) is converted.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<T> GetForObjectAsync<T>(string url, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Asynchronously retrieve a representation by doing a GET on the specified URL. 
        /// The response (if any) is converted.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<T> GetForObjectAsync<T>(Uri url) where T : class;

        /// <summary>
        /// Asynchronously retrieve an entity by doing a GET on the specified URL. 
        /// The response is converted and stored in an <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage<T>> GetForMessageAsync<T>(string url, params object[] uriVariables) where T : class;

        /// <summary>
        /// Asynchronously retrieve an entity by doing a GET on the specified URL. 
        /// The response is converted and stored in an <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage<T>> GetForMessageAsync<T>(string url, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Asynchronously retrieve an entity by doing a GET on the specified URL. 
        /// The response is converted and stored in an <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage<T>> GetForMessageAsync<T>(Uri url) where T : class;

        #endregion

        #region HEAD

        /// <summary>
        /// Asynchronously retrieve all headers of the resource specified by the URI template.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpHeaders> HeadForHeadersAsync(string url, params object[] uriVariables);

        /// <summary>
        /// Asynchronously retrieve all headers of the resource specified by the URI template.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpHeaders> HeadForHeadersAsync(string url, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Asynchronously retrieve all headers of the resource specified by the URI template.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpHeaders> HeadForHeadersAsync(Uri url);

        #endregion

        #region POST

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the value of the 'Location' header. 
        /// This header typically indicates where the new resource is stored.
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<Uri> PostForLocationAsync(string url, object request, params object[] uriVariables);

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the value of the 'Location' header. 
        /// This header typically indicates where the new resource is stored.
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given dictionary.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<Uri> PostForLocationAsync(string url, object request, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the value of the 'Location' header. 
        /// This header typically indicates where the new resource is stored.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<Uri> PostForLocationAsync(Uri url, object request);

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the representation found in the response. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<T> PostForObjectAsync<T>(string url, object request, params object[] uriVariables) where T : class;

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the representation found in the response. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given dictionary.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<T> PostForObjectAsync<T>(string url, object request, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the representation found in the response. 
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<T> PostForObjectAsync<T>(Uri url, object request) where T : class;

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage<T>> PostForMessageAsync<T>(string url, object request, params object[] uriVariables) where T : class;

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given dictionary.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage<T>> PostForMessageAsync<T>(string url, object request, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>. 
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage<T>> PostForMessageAsync<T>(Uri url, object request) where T : class;

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage> PostForMessageAsync(string url, object request, params object[] uriVariables);

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given dictionary.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage> PostForMessageAsync(string url, object request, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>. 
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage> PostForMessageAsync(Uri url, object request);

        #endregion

        #region PUT

        /// <summary>
        /// Asynchronously create or update a resource by PUTting the given object to the URI.
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task PutAsync(string url, object request, params object[] uriVariables);

        /// <summary>
        /// Asynchronously create or update a resource by PUTting the given object to the URI.
        /// </summary>
        /// <remarks>
        /// <para>
        /// URI Template variables are expanded using the given dictionary.
        /// </para>
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task PutAsync(string url, object request, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Asynchronously create or update a resource by PUTting the given object to the URI.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task PutAsync(Uri url, object request);

        #endregion

        #region DELETE

        /// <summary>
        /// Asynchronously delete the resources at the specified URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task DeleteAsync(string url, params object[] uriVariables);

        /// <summary>
        /// Asynchronously delete the resources at the specified URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task DeleteAsync(string url, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Asynchronously delete the resources at the specified URI.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task DeleteAsync(Uri url);

        #endregion

        #region OPTIONS

        /// <summary>
        /// Asynchronously return the value of the Allow header for the given URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<IList<HttpMethod>> OptionsForAllowAsync(string url, params object[] uriVariables);

        /// <summary>
        /// Asynchronously return the value of the Allow header for the given URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<IList<HttpMethod>> OptionsForAllowAsync(string url, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Asynchronously return the value of the Allow header for the given URI.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<IList<HttpMethod>> OptionsForAllowAsync(Uri url);

        #endregion


        #region Exchange

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the task.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage<T>> ExchangeAsync<T>(string url, HttpMethod method, HttpEntity requestEntity, CancellationToken cancellationToken, params object[] uriVariables) where T : class;

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the task.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage<T>> ExchangeAsync<T>(string url, HttpMethod method, HttpEntity requestEntity, CancellationToken cancellationToken, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the task.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage<T>> ExchangeAsync<T>(Uri url, HttpMethod method, HttpEntity requestEntity, CancellationToken cancellationToken) where T : class;

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the task.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage> ExchangeAsync(string url, HttpMethod method, HttpEntity requestEntity, CancellationToken cancellationToken, params object[] uriVariables);

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the task.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage> ExchangeAsync(string url, HttpMethod method, HttpEntity requestEntity, CancellationToken cancellationToken, IDictionary<string, object> uriVariables);

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the task.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<HttpResponseMessage> ExchangeAsync(Uri url, HttpMethod method, HttpEntity requestEntity, CancellationToken cancellationToken);

        #endregion

        #region General execution

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, preparing the request with the 
        /// <see cref="IRequestCallback"/>, and reading the response with an <see cref="IResponseExtractor{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestCallback">Object that prepares the request.</param>
        /// <param name="responseExtractor">Object that extracts the return value from the response.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the task.</param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<T> ExecuteAsync<T>(string url, HttpMethod method, IRequestCallback requestCallback, IResponseExtractor<T> responseExtractor, CancellationToken cancellationToken, params object[] uriVariables) where T : class;

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, preparing the request with the 
        /// <see cref="IRequestCallback"/>, and reading the response with an <see cref="IResponseExtractor{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestCallback">Object that prepares the request.</param>
        /// <param name="responseExtractor">Object that extracts the return value from the response.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the task.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<T> ExecuteAsync<T>(string url, HttpMethod method, IRequestCallback requestCallback, IResponseExtractor<T> responseExtractor, CancellationToken cancellationToken, IDictionary<string, object> uriVariables) where T : class;

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, preparing the request with the 
        /// <see cref="IRequestCallback"/>, and reading the response with an <see cref="IResponseExtractor{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestCallback">Object that prepares the request.</param>
        /// <param name="responseExtractor">Object that extracts the return value from the response.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the task.</param>
        /// <returns>A <code>Task&lt;T&gt;</code> that represents the asynchronous operation.</returns>
        Task<T> ExecuteAsync<T>(Uri url, HttpMethod method, IRequestCallback requestCallback, IResponseExtractor<T> responseExtractor, CancellationToken cancellationToken) where T : class;

        #endregion

        #endregion
    }
}
