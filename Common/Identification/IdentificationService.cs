//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tunynet.Events;
using Tunynet;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 身份认证Service
    /// </summary>
    public class IdentificationService
    {
        private IIdentificationRepository iIdentificationRepository;
        private IIdentificationTypeRepository identificationTypeRepository;

        #region 构造函数

        /// <summary>
        /// IdentificationService的构造函数
        /// </summary>
        public IdentificationService() : this(new IdentificationRepository(), new IdentificationTypeRepository()) { }

        /// <summary>
        /// IdentificationService的构造函数
        /// </summary>
        /// <param name="identificationRepository"></param>
        /// <param name="identificationTypeRepository"></param>
        public IdentificationService(IdentificationRepository identificationRepository, IdentificationTypeRepository identificationTypeRepository)
        {
            this.iIdentificationRepository = identificationRepository;
            this.identificationTypeRepository = identificationTypeRepository;
        }

        #endregion

        #region 身份认证申请

        /// <summary>
        /// 创建身份认证申请
        /// </summary>
        /// <param name="identification">身份认证实体</param>
        /// <param name="stream">证件扫描件</param>
        /// <returns></returns>
        public bool CreateIdentification(Identification identification, Stream stream = null)
        {
            EventBus<Identification>.Instance().OnBefore(identification, new CommonEventArgs(EventOperationType.Instance().Create()));

            //创建身份认证
            identification.IdentificationLogo = string.Empty;
            iIdentificationRepository.Insert(identification);

            //创建身份认证图片
            if (stream != null)
            {
                //上传Logo
                LogoService logoService = new LogoService(TenantTypeIds.Instance().Identification());
                identification.IdentificationLogo = logoService.UploadLogo(identification.IdentificationId, stream);

                iIdentificationRepository.Update(identification);
            }

            EventBus<Identification>.Instance().OnAfter(identification, new CommonEventArgs(EventOperationType.Instance().Create()));

            return true;
        }

        /// <summary>
        /// 更新身份认证申请
        /// </summary>
        /// <param name="identification">身份认证实体</param>
        /// <param name="stream">证件扫描件</param>
        /// <returns></returns>
        public bool UpdateIdentification(Identification identification, Stream stream = null)
        {
            EventBus<Identification>.Instance().OnBefore(identification, new CommonEventArgs(EventOperationType.Instance().Update()));

            if (stream != null)
            {
                LogoService logoService = new LogoService(TenantTypeIds.Instance().Identification());

                logoService.DeleteLogo(identification.IdentificationId);
                identification.IdentificationLogo = logoService.UploadLogo(identification.IdentificationId, stream);
            }

            iIdentificationRepository.Update(identification);

            EventBus<Identification>.Instance().OnAfter(identification, new CommonEventArgs(EventOperationType.Instance().Update()));

            return true;
        }

        /// <summary>
        /// 删除身份认证申请
        /// </summary>
        /// <param name="identificationId">身份认证ID</param>
        /// <returns></returns>
        public bool DeleteIdentification(long identificationId)
        {
            Identification identification = iIdentificationRepository.Get(identificationId);
            if (identification != null)
            {
                EventBus<Identification>.Instance().OnBefore(identification, new CommonEventArgs(EventOperationType.Instance().Delete()));

                //删除身份认证
                iIdentificationRepository.Delete(identification);

                //删除身份认证图片
                LogoService logoService = new LogoService(TenantTypeIds.Instance().Identification());
                logoService.DeleteLogo(identificationId);

                EventBus<Identification>.Instance().OnAfter(identification, new CommonEventArgs(EventOperationType.Instance().Delete()));
                return true;
            }

            return false;
        }

        
        /// <summary>
        ///分页检索身份认证
        /// </summary>
        ///<param name="query">查询条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public PagingDataSet<Identification> GetIdentifications(IdentificationQuery query, int pageIndex, int pageSize)
        {
            return iIdentificationRepository.GetIdentifications(query, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取某人某项(或所有)认证标识的身份认证
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<Identification> GetUserIdentifications(long userId, long IdentificationTypeId=0)
        {
            return iIdentificationRepository.GetUserIdentifications(userId, IdentificationTypeId);
        }

        /// <summary>
        /// 获取某个身份认证
        /// </summary>
        /// <param name="identificationId">身份认证ID</param>
        /// <returns></returns>
        public Identification GetIdentification(long identificationId)
        {
            return iIdentificationRepository.Get(identificationId);
        }

        /// <summary>
        /// 获取身份认证标识
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="是否只获取通过认证的标识"
        /// <returns></returns>
        public IEnumerable<IdentificationType> GetIdentificationTypes(long userId,bool status=true)
        {
            return iIdentificationRepository.GetIdentificationTypes(userId,status);
        }

        #endregion

        #region 认证标识

        /// <summary>
        /// 创建认证标识
        /// </summary>
        /// <param name="identificationType">认证标识实体</param>
        /// <returns></returns>
        public bool CreateIdentificationType(IdentificationType identificationType, Stream logoStream)
        {
            EventBus<IdentificationType>.Instance().OnBefore(identificationType, new CommonEventArgs(EventOperationType.Instance().Create()));

            identificationType.IdentificationTypeLogo = string.Empty;
            identificationTypeRepository.Insert(identificationType);

            //创建认证标识图片
            if (logoStream != null)
            {
                LogoService logoService = new LogoService(TenantTypeIds.Instance().IdentificationType());
                identificationType.IdentificationTypeLogo = logoService.UploadLogo(identificationType.IdentificationTypeId, logoStream);
                identificationTypeRepository.Update(identificationType);
            }

            EventBus<IdentificationType>.Instance().OnAfter(identificationType, new CommonEventArgs(EventOperationType.Instance().Create()));
            return true;
        }

        /// <summary>
        /// 更新认证标识
        /// </summary>
        /// <param name="identificationType">认证标识实体</param>
        /// <returns></returns>
        public bool UpdateIdentificationType(IdentificationType identificationType, Stream logoStream)
        {
            EventBus<IdentificationType>.Instance().OnBefore(identificationType, new CommonEventArgs(EventOperationType.Instance().Update()));

            if (logoStream != null)
            {
                LogoService logoService = new LogoService(TenantTypeIds.Instance().IdentificationType());
                identificationType.IdentificationTypeLogo = logoService.UploadLogo(identificationType.IdentificationTypeId, logoStream);
            }
            identificationTypeRepository.Update(identificationType);

            EventBus<IdentificationType>.Instance().OnAfter(identificationType, new CommonEventArgs(EventOperationType.Instance().Update()));
            return true;
        }

        /// <summary>
        /// 删除认证标识
        /// </summary>
        /// <param name="identificationTypeId">认证标识ID</param>
        /// <returns></returns>
        public bool DeleteIdentificationType(long identificationTypeId)
        {
            IdentificationType identificationType = identificationTypeRepository.Get(identificationTypeId);
            
            if (identificationType != null)
            {
                EventBus<IdentificationType>.Instance().OnBefore(identificationType, new CommonEventArgs(EventOperationType.Instance().Delete()));
                
                //删除认证标识和该认证标识下的所有申请
                IdentificationTypeRepository typeRepository = new IdentificationTypeRepository();
                typeRepository.DeleteIdentificationTypes(identificationTypeId);

                //删除认证标识图
                LogoService logoService = new LogoService(TenantTypeIds.Instance().IdentificationType());
                logoService.DeleteLogo(identificationTypeId);

                EventBus<IdentificationType>.Instance().OnAfter(identificationType, new CommonEventArgs(EventOperationType.Instance().Delete()));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取身份认证标识
        /// </summary>
        /// <param name="isEnabled">是否启用</param>
        /// <returns></returns>
        public IEnumerable<IdentificationType> GetIdentificationTypes(bool? isEnabled)
        {
            return identificationTypeRepository.GetIdentificationTypes(isEnabled);
        }

        #endregion
    }
}
