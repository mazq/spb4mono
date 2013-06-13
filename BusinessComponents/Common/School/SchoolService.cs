//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Tunynet.Caching;
using System.Linq;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using NPinyin;

namespace Tunynet.Common
{
    /// <summary>
    /// ѧУҵ���߼���
    /// </summary>
    public class SchoolService
    {
        private ISchoolRepository schoolRepository;

        /// <summary>
        /// ����������
        /// </summary>
        public SchoolService()
            : this(new SchoolRepository())
        {

        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="schoolRepository"></param>
        public SchoolService(ISchoolRepository schoolRepository)
        {
            this.schoolRepository = schoolRepository;
        }

        #region Create/Update/Delete

        /// <summary>
        /// ���ѧУ
        /// </summary>
        public void Create(School school)
        {
            EventBus<School>.Instance().OnBefore(school, new CommonEventArgs(EventOperationType.Instance().Create()));
            school.PinyinName = Pinyin.GetPinyin(school.Name);
            school.ShortPinyinName = Pinyin.GetInitials(school.Name);
            schoolRepository.Insert(school);
            school.DisplayOrder = school.Id;
            schoolRepository.Update(school);
            EventBus<School>.Instance().OnAfter(school, new CommonEventArgs(EventOperationType.Instance().Create()));
        }

        /// <summary>
        /// ����ѧУ
        /// </summary>
        /// <param name="school">Ҫ���µ�ѧУ</param>
        /// <returns></returns>
        public void Update(School school)
        {
            EventBus<School>.Instance().OnBefore(school, new CommonEventArgs(EventOperationType.Instance().Update()));
            school.PinyinName = Pinyin.GetPinyin(school.Name);
            school.ShortPinyinName = Pinyin.GetInitials(school.Name);
            schoolRepository.Update(school);
            EventBus<School>.Instance().OnAfter(school, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// ɾ��ѧУ
        /// </summary>
        /// <param name="schoolId">ѧУ����</param>
        public void Delete(long schoolId)
        {
            School school = Get(schoolId);
            EventBus<School>.Instance().OnBefore(school, new CommonEventArgs(EventOperationType.Instance().Delete()));
            schoolRepository.Delete(school);
            EventBus<School>.Instance().OnAfter(school, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        /// <summary>
        /// ���ѧУ������˳��
        /// </summary>
        /// <param name="id">��������Id</param>
        /// <param name="referenceId">����Id</param>        
        public void ChangeDisplayOrder(long id, long referenceId)
        {
            schoolRepository.ChangeDisplayOrder(id, referenceId);
        }

        #endregion


        #region Get & Gets

        /// <summary>
        /// ��ȡѧУͳ����Ϣ
        /// </summary>
        public School Get(long schoolId)
        {
            return schoolRepository.Get(schoolId);
        }

        /// <summary>
        /// ��ѯѧУ
        /// </summary>
        /// <param name="areaCode">��������</param>
        /// <param name="keyword">�ؼ��ʣ�֧��ƴ��������</param>
        /// <param name="schoolType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagingDataSet<School> Gets(string areaCode,  string keyword , SchoolType? schoolType , int pageSize, int pageIndex)
        {

            return schoolRepository.Gets(areaCode, keyword, schoolType, pageSize, pageIndex);



            //���Ҫ�㣺
            //1.������ԣ���KeywordΪnullʱ��ʹ��AreaCode�������棬����ʹ�û��棻
            //2.�������ڣ��ȶ����ݣ�
            //3.keyword:Name��PinyinName��ShortPinyinName,֧��ģ��������
            //4.���򣺰���DisplayOrder��������


            
        }

        #endregion
    }
}
