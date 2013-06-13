//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace Spacebuilder.Group
{
    /// <summary>
    /// 通过群组数据仓储实现查询
    /// </summary>
    public class DefaultGroupIdToGroupKeyDictionary : GroupIdToGroupKeyDictionary
    {
        private IGroupRepository groupRepository;
        /// <summary>
        /// 构造器
        /// </summary>
        public DefaultGroupIdToGroupKeyDictionary()
            : this(new GroupRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public DefaultGroupIdToGroupKeyDictionary(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        /// <summary>
        /// 根据群组Id获取群组Key
        /// </summary>
        /// <returns>
        /// 群组Id
        /// </returns>
        protected override string GetGroupKeyByGroupId(long groupId)
        {
            GroupEntity group = groupRepository.Get(groupId);
            if (group != null)
                return group.GroupKey;
            return null;
        }

        /// <summary>
        /// 根据群组Key获取群组Id
        /// </summary>
        /// <param name="groupKey">群组Key</param>
        /// <returns>
        /// 群组Id
        /// </returns>
        protected override long GetGroupIdByGroupKey(string groupKey)
        {
            return groupRepository.GetGroupIdByGroupKey(groupKey);
        }
    }
}
