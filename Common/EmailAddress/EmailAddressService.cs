using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 邮寄地址业务逻辑类
    /// </summary>
    public class MailAddressService
    {
        private IMailAddressRepository mailAddressRepository;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MailAddressService()
            : this(new MailAddressRepository())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mailAddressRepository">邮寄信息仓储实现</param>
        public MailAddressService(IMailAddressRepository mailAddressRepository)
        {
            this.mailAddressRepository = mailAddressRepository;
        }
        #endregion    

        #region 邮寄地址维护
        /// <summary>
        /// 创建邮寄地址
        /// </summary>
        /// <param name="mailAddress">邮寄地址对象</param>
        /// <returns></returns>
        public bool Create(MailAddress mailAddress)
        {
            mailAddressRepository.Insert(mailAddress);
            return mailAddress.AddressId > 0;
        }

        /// <summary>
        /// 编辑邮寄地址
        /// </summary>
        /// <param name="mailAddress">邮寄地址对象</param>
        public void Edit(MailAddress mailAddress)
        {
            mailAddressRepository.Update(mailAddress);
        }

        /// <summary>
        /// 删除邮寄地址
        /// </summary>
        /// <param name="mailAddress"></param>
        public void Delete(MailAddress mailAddress)
        {
            mailAddressRepository.Delete(mailAddress);
        }
        #endregion

        #region 邮寄地址查询
        /// <summary>
        /// 获取邮寄地址对象
        /// </summary>
        /// <param name="addressId">邮寄地址ID</param>
        /// <returns>返回邮寄地址对象</returns>
        public MailAddress Get(long addressId)
        {
            return mailAddressRepository.Get(addressId);
        }

        /// <summary>
        /// 获取邮寄地址
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public IEnumerable<MailAddress> GetsOfUser(long userId)
        {
            return mailAddressRepository.GetEmailAddresssOfUser(userId);
        }
        #endregion
    }
}
