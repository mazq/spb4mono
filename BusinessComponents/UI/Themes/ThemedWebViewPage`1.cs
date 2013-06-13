//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Tunynet.UI
{
    /// <summary>
    /// 重写WebViewPage用于支持皮肤机制
    /// </summary>
    public abstract class ThemedWebViewPage<TModel> : ThemedWebViewPage
    {
        private ViewDataDictionary<TModel> _viewData;
        /// <summary>
        /// AjaxHelper
        /// </summary>
        public new AjaxHelper<TModel> Ajax
        {
            get;
            set;
        }

        /// <summary>
        /// HtmlHelper
        /// </summary>
        public new HtmlHelper<TModel> Html
        {
            get;
            set;
        }

        /// <summary>
        /// Model
        /// </summary>
        public new TModel Model
        {
            get
            {
                return ViewData.Model;
            }
        }
        /// <summary>
        /// ViewData
        /// </summary>
        public new ViewDataDictionary<TModel> ViewData
        {
            get
            {
                if (_viewData == null)
                {
                    SetViewData(new ViewDataDictionary<TModel>());
                }
                return _viewData;
            }
            set
            {
                SetViewData(value);
            }
        }

        /// <summary>
        /// InitHelpers
        /// </summary>
        public override void InitHelpers()
        {
            base.InitHelpers();

            Ajax = new AjaxHelper<TModel>(ViewContext, this);
            Html = new HtmlHelper<TModel>(ViewContext, this);
        }

        /// <summary>
        /// SetViewData
        /// </summary>
        /// <param name="viewData"></param>
        protected override void SetViewData(ViewDataDictionary viewData)
        {
            _viewData = new ViewDataDictionary<TModel>(viewData);

            base.SetViewData(_viewData);
        }
    }
}
