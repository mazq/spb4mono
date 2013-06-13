//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;

namespace Spacebuilder.Common
{
    /// <summary>
    /// ModelBinder从Cookie中取值
    /// </summary>
    public class CookieValueProviderFactory : ValueProviderFactory
    {

        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            HttpCookieCollection cookies = controllerContext.HttpContext.Request.Cookies;

            Dictionary<string, string> backingStore = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < cookies.Count; i++)
            {
                HttpCookie cookie = cookies[i];
                if (!String.IsNullOrEmpty(cookie.Name))
                {
                    backingStore[cookie.Name] = cookie.Value;
                }
            }

            return new DictionaryValueProvider<string>(backingStore, CultureInfo.InvariantCulture);
        }

    }
}
