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


namespace Tunynet.Common
{
    /// <summary>
    /// ����ҵ���߼���
    /// </summary>
    public class AreaService
    {
        private IAreaRepository areaRepository;

        /// <summary>
        /// ����������
        /// </summary>
        public AreaService()
            : this(new AreaRepository())
        {

        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="areaRepository"></param>
        public AreaService(IAreaRepository areaRepository)
        {
            this.areaRepository = areaRepository;
        }

        #region Create/Update/Delete

        /// <summary>
        /// ��ӵ���
        /// </summary>
        public void Create(Area area)
        {
            EventBus<Area>.Instance().OnBefore(area, new CommonEventArgs(EventOperationType.Instance().Create()));
            areaRepository.Insert(area);
            EventBus<Area>.Instance().OnAfter(area, new CommonEventArgs(EventOperationType.Instance().Create()));
        }

        /// <summary>
        /// ���µ���
        /// </summary>
        /// <param name="area">Ҫ���µĵ���</param>
        /// <returns></returns>
        public void Update(Area area)
        {
            EventBus<Area>.Instance().OnBefore(area, new CommonEventArgs(EventOperationType.Instance().Update()));
            areaRepository.Update(area);
            EventBus<Area>.Instance().OnAfter(area, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="areaCode">��������</param>
        public void Delete(string areaCode)
        {
            Area area = Get(areaCode);
            EventBus<Area>.Instance().OnBefore(area, new CommonEventArgs(EventOperationType.Instance().Delete()));
            areaRepository.Delete(areaCode);
            EventBus<Area>.Instance().OnAfter(area, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        #endregion

        #region Get & Gets

        /// <summary>
        /// ��ȡ����ͳ����Ϣ
        /// </summary>
        public Area Get(string areaCode)
        {
            return areaRepository.Get(areaCode);
        }


        /// <summary>
        /// ��ȡ������������
        /// </summary>
        public IEnumerable<Area> GetRoots()
        {
            return areaRepository.GetRoots();
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="parentAreaCode">������������</param>
        public IEnumerable<Area> GetDescendants(string parentAreaCode)
        {
            return areaRepository.GetDescendants(parentAreaCode);
        }

        /// <summary>
        /// �жϵ����Ƿ񸸼�����
        /// </summary>
        /// <param name="area"></param>
        /// <param name="parentAreaCode"></param>
        /// <returns></returns>
        public bool IsChildArea(string area, string parentAreaCode)
        {
            List<Area> areas = new List<Area>();
            RecursiveGetAllParentArea(Get(area), ref areas);
            return areas.Any(n => n.AreaCode.Equals(parentAreaCode));
        }

        /// <summary>
        /// ��ȡ���и�������
        /// </summary>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public List<Area> GetAllParentAreas(string areaCode)
        {
            List<Area> areas = new List<Area>();
            RecursiveGetAllParentArea(Get(areaCode), ref areas);
            return areas;
        }

        /// <summary>
        /// ��ȡ���еĸ�������
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areas"></param>
        private void RecursiveGetAllParentArea(Area area, ref List<Area> areas)
        {
            if (area == null || string.IsNullOrEmpty(area.ParentCode.Trim()))
                return;
            Area parentArea = areaRepository.Get(area.ParentCode);
            areas.Add(parentArea);
            RecursiveGetAllParentArea(parentArea, ref areas);
        }

        #endregion
    }
}
