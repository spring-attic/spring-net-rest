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
using System.Drawing;

using Spring.Http;
using Spring.Http.Converters;

namespace Spring.HttpMessageConverterQuickStart.Converters
{
    /// <summary>
    /// Implementation of <see cref="IHttpMessageConverter"/> that can read images 
    /// using .NET Framework <see cref="Image"/> class and derived classes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// By default, this converter supports 'image/bmp', 'image/jpg', 'image/jpeg', 
    /// 'image/gif', 'image/png' and 'image/tiff' media types. 
    /// This can be overridden by setting the <see cref="P:SupportedMediaTypes"/> property.
    /// </para>
    /// </remarks>
    public class ImageHttpMessageConverter : AbstractHttpMessageConverter
    {
        public ImageHttpMessageConverter() :
            base(new MediaType("image", "bmp"), new MediaType("image", "jpg"), new MediaType("image", "jpeg"),
            new MediaType("image", "gif"), new MediaType("image", "png"), new MediaType("image", "tiff"))
        {
        }

        protected override bool Supports(Type type)
        {
            return typeof(Image).IsAssignableFrom(type);
        }

        protected override T ReadInternal<T>(IHttpInputMessage message)
        {
            return Image.FromStream(message.Body) as T;
        }

        protected override void WriteInternal(object content, IHttpOutputMessage message)
        {
            throw new NotSupportedException();
        }
    }
}