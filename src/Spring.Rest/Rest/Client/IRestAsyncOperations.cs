#region License

/*
 * Copyright 2002-2011 the original author or authors.
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

using Spring.Http;

namespace Spring.Rest.Client
{
    /// <summary>
    /// Interface specifying a basic set of RESTful asynchronous operations. 
    /// </summary>
    /// <remarks>
    /// Not often used directly, but a useful option to enhance testability, 
    /// as it can easily be mocked or stubbed.
    /// </remarks>
    /// <see cref="RestTemplate"/>
    /// <author>Bruno Baia</author>
    public interface IRestAsyncOperations
    {
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
        /// <param name="getCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous GET method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler GetForObjectAsync<T>(string url, Action<RestOperationCompletedEventArgs<T>> getCompleted, params object[] uriVariables) where T : class;

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
        /// <param name="getCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous GET method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler GetForObjectAsync<T>(string url, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<T>> getCompleted) where T : class;

        /// <summary>
        /// Asynchronously retrieve a representation by doing a GET on the specified URL. 
        /// The response (if any) is converted.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="getCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous GET method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler GetForObjectAsync<T>(Uri url, Action<RestOperationCompletedEventArgs<T>> getCompleted) where T : class;

        /// <summary>
        /// Asynchronously retrieve an entity by doing a GET on the specified URL. 
        /// The response is converted and stored in an <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="getCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous GET method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler GetForMessageAsync<T>(string url, Action<RestOperationCompletedEventArgs<HttpResponseMessage<T>>> getCompleted, params object[] uriVariables) where T : class;

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
        /// <param name="getCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous GET method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler GetForMessageAsync<T>(string url, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<HttpResponseMessage<T>>> getCompleted) where T : class;

        /// <summary>
        /// Asynchronously retrieve an entity by doing a GET on the specified URL. 
        /// The response is converted and stored in an <see cref="HttpResponseMessage{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="getCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous GET method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler GetForMessageAsync<T>(Uri url, Action<RestOperationCompletedEventArgs<HttpResponseMessage<T>>> getCompleted) where T : class;

        #endregion

        #region HEAD

        /// <summary>
        /// Asynchronously retrieve all headers of the resource specified by the URI template.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="headCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous HEAD method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler HeadForHeadersAsync(string url, Action<RestOperationCompletedEventArgs<HttpHeaders>> headCompleted, params object[] uriVariables);

        /// <summary>
        /// Asynchronously retrieve all headers of the resource specified by the URI template.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <param name="headCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous HEAD method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler HeadForHeadersAsync(string url, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<HttpHeaders>> headCompleted);

        /// <summary>
        /// Asynchronously retrieve all headers of the resource specified by the URI template.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="headCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous HEAD method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler HeadForHeadersAsync(Uri url, Action<RestOperationCompletedEventArgs<HttpHeaders>> headCompleted);

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
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForLocationAsync(string url, object request, Action<RestOperationCompletedEventArgs<Uri>> postCompleted, params object[] uriVariables);

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
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForLocationAsync(string url, object request, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<Uri>> postCompleted);

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the value of the 'Location' header. 
        /// This header typically indicates where the new resource is stored.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForLocationAsync(Uri url, object request, Action<RestOperationCompletedEventArgs<Uri>> postCompleted);

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
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForObjectAsync<T>(string url, object request, Action<RestOperationCompletedEventArgs<T>> postCompleted, params object[] uriVariables) where T : class;

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
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForObjectAsync<T>(string url, object request, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<T>> postCompleted) where T : class;

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the representation found in the response. 
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForObjectAsync<T>(Uri url, object request, Action<RestOperationCompletedEventArgs<T>> postCompleted) where T : class;

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
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForMessageAsync<T>(string url, object request, Action<RestOperationCompletedEventArgs<HttpResponseMessage<T>>> postCompleted, params object[] uriVariables) where T : class;

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
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForMessageAsync<T>(string url, object request, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<HttpResponseMessage<T>>> postCompleted) where T : class;

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the response as <see cref="HttpResponseMessage{T}"/>. 
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForMessageAsync<T>(Uri url, object request, Action<RestOperationCompletedEventArgs<HttpResponseMessage<T>>> postCompleted) where T : class;

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
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForMessageAsync(string url, object request, Action<RestOperationCompletedEventArgs<HttpResponseMessage>> postCompleted, params object[] uriVariables);

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
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForMessageAsync(string url, object request, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<HttpResponseMessage>> postCompleted);

        /// <summary>
        /// Asynchronously create a new resource by POSTing the given object to the URI template, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>. 
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="postCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous POST method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PostForMessageAsync(Uri url, object request, Action<RestOperationCompletedEventArgs<HttpResponseMessage>> postCompleted);

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
        /// <param name="putCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous PUT method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PutAsync(string url, object request, Action<RestOperationCompletedEventArgs<object>> putCompleted, params object[] uriVariables);

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
        /// <param name="putCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous PUT method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PutAsync(string url, object request, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<object>> putCompleted);

        /// <summary>
        /// Asynchronously create or update a resource by PUTting the given object to the URI.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="request">
        /// The object to be POSTed, may be a <see cref="HttpEntity"/> in order to add additional HTTP headers.
        /// </param>
        /// <param name="putCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous PUT method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler PutAsync(Uri url, object request, Action<RestOperationCompletedEventArgs<object>> putCompleted);

        #endregion

        #region DELETE

        /// <summary>
        /// Asynchronously delete the resources at the specified URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="deleteCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous PUT method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler DeleteAsync(string url, Action<RestOperationCompletedEventArgs<object>> deleteCompleted, params object[] uriVariables);

        /// <summary>
        /// Asynchronously delete the resources at the specified URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <param name="deleteCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous PUT method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler DeleteAsync(string url, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<object>> deleteCompleted);

        /// <summary>
        /// Asynchronously delete the resources at the specified URI.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="deleteCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous PUT method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler DeleteAsync(Uri url, Action<RestOperationCompletedEventArgs<object>> deleteCompleted);

        #endregion

        #region OPTIONS

        /// <summary>
        /// Asynchronously return the value of the Allow header for the given URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given URI variables, if any.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="optionsCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous OPTIONS method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler OptionsForAllowAsync(string url, Action<RestOperationCompletedEventArgs<IList<HttpMethod>>> optionsCompleted, params object[] uriVariables);

        /// <summary>
        /// Asynchronously return the value of the Allow header for the given URI.
        /// </summary>
        /// <remarks>
        /// URI Template variables are expanded using the given dictionary.
        /// </remarks>
        /// <param name="url">The URL.</param>
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <param name="optionsCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous OPTIONS method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler OptionsForAllowAsync(string url, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<IList<HttpMethod>>> optionsCompleted);

        /// <summary>
        /// Asynchronously return the value of the Allow header for the given URI.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="optionsCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous OPTIONS method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler OptionsForAllowAsync(Uri url, Action<RestOperationCompletedEventArgs<IList<HttpMethod>>> optionsCompleted);

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
        /// <param name="methodCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler ExchangeAsync<T>(string url, HttpMethod method, HttpEntity requestEntity, Action<RestOperationCompletedEventArgs<HttpResponseMessage<T>>> methodCompleted, params object[] uriVariables) where T : class;

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
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <param name="methodCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler ExchangeAsync<T>(string url, HttpMethod method, HttpEntity requestEntity, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<HttpResponseMessage<T>>> methodCompleted) where T : class;

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
        /// <param name="methodCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler ExchangeAsync<T>(Uri url, HttpMethod method, HttpEntity requestEntity, Action<RestOperationCompletedEventArgs<HttpResponseMessage<T>>> methodCompleted) where T : class;

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
        /// <param name="methodCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous method completes.
        /// </param>
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler ExchangeAsync(string url, HttpMethod method, HttpEntity requestEntity, Action<RestOperationCompletedEventArgs<HttpResponseMessage>> methodCompleted, params object[] uriVariables);

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
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <param name="methodCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler ExchangeAsync(string url, HttpMethod method, HttpEntity requestEntity, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<HttpResponseMessage>> methodCompleted);

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, writing the given request message to the request, 
        /// and returns the response with no entity as <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestEntity">
        /// The HTTP entity (headers and/or body) to write to the request, may be <see langword="null"/>.
        /// </param>
        /// <param name="methodCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler ExchangeAsync(Uri url, HttpMethod method, HttpEntity requestEntity, Action<RestOperationCompletedEventArgs<HttpResponseMessage>> methodCompleted);

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
        /// <param name="methodCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous method completes.
        /// </param>       
        /// <param name="uriVariables">The variables to expand the template.</param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler ExecuteAsync<T>(string url, HttpMethod method, IRequestCallback requestCallback, IResponseExtractor<T> responseExtractor, Action<RestOperationCompletedEventArgs<T>> methodCompleted, params object[] uriVariables) where T : class;

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
        /// <param name="uriVariables">The dictionary containing variables for the URI template.</param>
        /// <param name="methodCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous method completes.
        /// </param>
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler ExecuteAsync<T>(string url, HttpMethod method, IRequestCallback requestCallback, IResponseExtractor<T> responseExtractor, IDictionary<string, object> uriVariables, Action<RestOperationCompletedEventArgs<T>> methodCompleted) where T : class;

        /// <summary>
        /// Asynchronously execute the HTTP method to the given URI template, preparing the request with the 
        /// <see cref="IRequestCallback"/>, and reading the response with an <see cref="IResponseExtractor{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the response value.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="requestCallback">Object that prepares the request.</param>
        /// <param name="responseExtractor">Object that extracts the return value from the response.</param>
        /// <param name="methodCompleted">
        /// The <code>Action&lt;T&gt;</code> to perform when the asynchronous method completes.
        /// </param>  
        /// <returns>
        /// A <see cref="RestOperationCanceler"/> instance that allows to cancel the asynchronous operation.
        /// </returns>
        RestOperationCanceler ExecuteAsync<T>(Uri url, HttpMethod method, IRequestCallback requestCallback, IResponseExtractor<T> responseExtractor, Action<RestOperationCompletedEventArgs<T>> methodCompleted) where T : class;

        #endregion
    }
}
