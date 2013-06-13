//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using Tunynet.Common;
using Tunynet.UI;
using System;
using Spacebuilder.Common;
using System.Web.Routing;
using Tunynet.Mvc;
using System.Linq;

namespace Spacebuilder.Blog
{
    public class BlogApplication : ApplicationBase
    {
        protected BlogApplication(ApplicationModel model, ApplicationConfig config)
            : base(model, config)
        { }

        /// <summary>
        /// 删除用户在应用中的数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="takeOverUserName">用于接管删除用户时不能删除的内容(例如：用户创建的群组)</param>
        /// <param name="isTakeOver">是否接管被删除用户可被接管的内容</param>
        protected override void DeleteUser(long userId, string takeOverUserName, bool isTakeOver)
        {
            BlogService blogService = new BlogService();
            blogService.DeleteUser(userId, takeOverUserName, isTakeOver);
        }

        protected override bool Install(string presentAreaKey, long ownerId)
        {
            return true;
        }

        protected override bool UnInstall(string presentAreaKey, long ownerId)
        {
            return true;
        }

        protected override IEnumerable<Navigation> GetDynamicNavigations(string presentAreaKey, long ownerId = 0)
        {
            List<Navigation> navigations = new List<Navigation>();

            if (presentAreaKey != PresentAreaKeysOfBuiltIn.Channel)
                return navigations;

            CategoryService categoryService = new CategoryService();
            IEnumerable<Category> categories = categoryService.GetRootCategories(TenantTypeIds.Instance().BlogThread(), ownerId);

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    string url = SiteUrls.Instance().BlogListByCategory(category.CategoryId.ToString());

                    int navigationId = NavigationService.GenerateDynamicNavigationId(category.CategoryId);
                    Navigation navigation = new Navigation()
                    {
                        ApplicationId = ApplicationId,
                        Depth = category.Depth + 1,
                        NavigationId = navigationId,
                        NavigationText = category.CategoryName,
                        ParentNavigationId = 10100201,
                        IsEnabled = true,
                        NavigationTarget = "_self",
                        NavigationUrl = url,
                        PresentAreaKey = PresentAreaKeysOfBuiltIn.Channel,
                        DisplayOrder = (int)category.DisplayOrder+90000000
                    };

                    navigations.Add(navigation);
                }
            }

            return navigations;
        }
    }
}