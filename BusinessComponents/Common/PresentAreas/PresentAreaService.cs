//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 呈现区域业务逻辑
    /// </summary>
    public class PresentAreaService
    {

        //Repository
        private IRepository<PresentArea> repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PresentAreaService()
            : this(new Repository<PresentArea>())
        {
        }

        /// <summary>
        /// 可设置repository的构造函数（主要用于测试用例）
        /// </summary>
        /// <param name="repository">PresentArea仓储</param>
        public PresentAreaService(IRepository<PresentArea> repository)
        {
            this.repository = repository;
        }


        /// <summary>
        /// 根据PresentAreaKey获取呈现区域
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <returns>返回presentAreaKey对应的PresentArea，如果不存在返回null</returns>
        public PresentArea Get(string presentAreaKey)
        {
            return repository.Get(presentAreaKey);
        }

        /// <summary>
        /// 获取所有呈现区域
        /// </summary>
        /// <returns>返回所有呈现区域</returns>
        public IEnumerable<PresentArea> GetAll()
        {
            return repository.GetAll();
        }


        /// <summary>
        /// 更新呈现区域
        /// </summary>
        /// <param name="presentArea">PresentArea</param>        
        public void Update(PresentArea presentArea)
        {
            EventBus<PresentArea>.Instance().OnBefore(presentArea, new CommonEventArgs(EventOperationType.Instance().Update()));
            repository.Update(presentArea);
            EventBus<PresentArea>.Instance().OnAfter(presentArea, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

    }
}
