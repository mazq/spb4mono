//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-06-25</createdate>
//<author>zhangyq</author>
//<email>zhangyq@tunynet.com</email>
//<log date="2012-06-25" version="0.5">创建</log>
//--------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户资料
    /// </summary>
    [TableName("spb_Profiles")]
    [PrimaryKey("UserId", autoIncrement = false)]
    //[CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Usual)]
    [Serializable]
    public class UserProfile : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static UserProfile New()
        {
            UserProfile userProfile = new UserProfile()
            {
                Birthday = DateTime.UtcNow,
                LunarBirthday = DateTime.UtcNow,
                BirthdayType = BirthdayType.Birthday,
                Email = string.Empty,
                Mobile = string.Empty,
                QQ = string.Empty,
                Msn = string.Empty,
                Skype = string.Empty,
                Fetion = string.Empty,
                Aliwangwang = string.Empty,
                CardID = string.Empty,
                Introduction = string.Empty,
                CardType = CertificateType.Residentcard,
                HomeAreaCode = string.Empty,
                NowAreaCode = string.Empty,
                Integrity = 0,
                Gender = GenderType.NotSet,
                UserId = 0
            };
            return userProfile;
        }

        #region 需持久化属性

        /// <summary>
        ///UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///性别1=男,2=女,0=未设置
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        ///生日类型1=公历,2=阴历
        /// </summary>
        public BirthdayType BirthdayType { get; set; }

        /// <summary>
        ///公历生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        ///阴历生日
        /// </summary>
        public DateTime LunarBirthday { get; set; }
        
        /// <summary>
        ///所在地
        /// </summary>
        public string NowAreaCode { get; set; }
        
        /// <summary>
        ///家乡
        /// </summary>
        public string HomeAreaCode { get; set; }
        
        /// <summary>
        ///联系邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///手机号码
        /// </summary>
        public string Mobile { get; set; }
        
        /// <summary>
        ///QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        ///MSN
        /// </summary>
        public string Msn { get; set; }

        /// <summary>
        ///Skype
        /// </summary>
        public string Skype { get; set; }

        /// <summary>
        ///飞信
        /// </summary>
        public string Fetion { get; set; }

        /// <summary>
        ///阿里旺旺
        /// </summary>
        public string Aliwangwang { get; set; }

        /// <summary>
        ///证件类型
        /// </summary>
        public CertificateType CardType { get; set; }

        /// <summary>
        ///证件号码
        /// </summary>
        public string CardID { get; set; }

        /// <summary>
        ///自我介绍
        /// </summary>
        public string Introduction { get; set; }

        ///// <summary>
        /////头像名称
        ///// </summary>
        //public string AvatarImage { get; set; }

        ///// <summary>
        ///// 判断是否有头像
        ///// </summary>
        //[Ignore]
        //public bool HasAvatarImage
        //{
        //    get
        //    {
        //        return !string.IsNullOrEmpty(AvatarImage) && !string.IsNullOrEmpty(AvatarImage.Trim());
        //    }
        //}

        /// <summary>
        /// 资料完整度（0至100）
        /// </summary>
        public int Integrity { get; set; }

        #endregion 需持久化属性


        #region 序列化属性

        /// <summary>
        /// 下次登陆是否需要向导(true为不需要向导)
        /// </summary>
        [Ignore]
        public bool IsNeedGuide
        {
            get { return GetExtendedProperty<bool>("IsNeedGuide"); }
            set { SetExtendedProperty("IsNeedGuide", value); }
        }

        #endregion

        #region 扩展属性

        /// <summary>
        /// 家乡是否存在
        /// </summary>
        [Ignore]
        public bool HasHomeAreaCode { get { return !string.IsNullOrEmpty(HomeAreaCode); } }

        /// <summary>
        /// 居住地是否存在
        /// </summary>
        [Ignore]
        public string HomeAreaName
        {
            get
            {
                if (!string.IsNullOrEmpty(HomeAreaCode))
                {
                    return @Formatter.FormatArea(HomeAreaCode, false);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 是否含有手机号
        /// </summary>
        [Ignore]
        public bool HasMobile { get { return !string.IsNullOrEmpty(Mobile); } }



        /// <summary>
        /// 是都存在IM
        /// </summary>
        [Ignore]
        public bool HasIM
        {
            get
            {
                return !string.IsNullOrEmpty(QQ)
                | !string.IsNullOrEmpty(Msn)
                | !string.IsNullOrEmpty(Skype)
                | !string.IsNullOrEmpty(Fetion)
                | !string.IsNullOrEmpty(Aliwangwang);
            }
        }

        /// <summary>
        /// 检查所在地是否存在
        /// </summary>
        [Ignore]
        public bool HasNowAreaCode { get { return !string.IsNullOrEmpty(NowAreaCode); } }


        /// <summary>
        /// 自我介绍是否存在
        /// </summary>
        [Ignore]
        public bool HasIntroduction { get { return !string.IsNullOrEmpty(Introduction); } }

        #endregion


        #region 导航属性

        /// <summary>
        /// 获取用户所有的教育经历
        /// </summary>
        [Ignore]
        public IEnumerable<EducationExperience> EducationExperience
        {
            get { return new UserProfileService().GetEducationExperiences(this.UserId); }
        }

        /// <summary>
        /// 获取用户所有的工作经历
        /// </summary>
        [Ignore]
        public IEnumerable<WorkExperience> WorkExperience
        {
            get { return new UserProfileService().GetWorkExperiences(this.UserId); }
        }

        #endregion







        #region IEntity 成员

        object IEntity.EntityId { get { return this.UserId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }

    /// <summary>
    /// 教育经历
    /// </summary>

    [TableName("spb_EducationExperiences")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class EducationExperience : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        
        public static EducationExperience New()
        {
            EducationExperience educationExperience = new EducationExperience()
            {
                School = string.Empty,
                Department = string.Empty,
                Degree = DegreeType.Undergraduate,
                StartYear = 0,
                UserId = 0
            };
            return educationExperience;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///学历
        /// </summary>
        public DegreeType Degree { get; set; }

        /// <summary>
        ///学校名称
        /// </summary>
        public string School { get; set; }

        /// <summary>
        ///入学年份
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        ///院系/班级
        /// </summary>
        public string Department { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }

    /// <summary>
    /// 工作经历
    /// </summary>
    [TableName("spb_WorkExperiences")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class WorkExperience : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        
        public static WorkExperience New()
        {
            WorkExperience workExperience = new WorkExperience()
            {
                CompanyName = string.Empty,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                JobDescription = string.Empty,
                CompanyAreaCode = string.Empty,
                UserId = 0,
            };
            return workExperience;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        ///所在地
        /// </summary>
        public string CompanyAreaCode { get; set; }

        /// <summary>
        ///开始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///截止时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///部门/职位
        /// </summary>
        public string JobDescription { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员


    }
}