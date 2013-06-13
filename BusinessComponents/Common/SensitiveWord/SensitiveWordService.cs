//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using Tunynet.Repositories;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// ��д�ҵ���߼���
    /// </summary>
    public class SensitiveWordService
    {
        private ISensitiveWordRepository sensitiveWordRepository;
        private IRepository<SensitiveWordType> sensitiveWordTypeRepository;

        /// <summary>
        /// ����������
        /// </summary>
        public SensitiveWordService()
            : this(new SensitiveWordRepository(), new SensitiveWordTypeRepository())
        {

        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="sensitiveWordRepository">��д����ݷ���</param>
        /// <param name="sensitiveWordTypeRepository">��д��������ݷ���</param>
        public SensitiveWordService(ISensitiveWordRepository sensitiveWordRepository, IRepository<SensitiveWordType> sensitiveWordTypeRepository)
        {
            this.sensitiveWordRepository = sensitiveWordRepository;
            this.sensitiveWordTypeRepository = sensitiveWordTypeRepository;
        }

        #region SensitiveWord

        /// <summary>
        /// �����д�
        /// </summary>
        /// <param name="sensitiveWord">��д�ʵ������</param>
        public int Create(SensitiveWord sensitiveWord)
        {
            EventBus<SensitiveWord>.Instance().OnBefore(sensitiveWord, new CommonEventArgs(EventOperationType.Instance().Create()));
            int judge = sensitiveWordRepository.Create(sensitiveWord);
            EventBus<SensitiveWord>.Instance().OnAfter(sensitiveWord, new CommonEventArgs(EventOperationType.Instance().Create()));
            return judge;
        }


        /// <summary>
        /// �����д�
        /// </summary>
        /// <param name="stream">��д��ļ���</param>
        /// <param name="typeId">��д�����Id</param>
        public void BatchCreate(Stream stream, int? typeId)
        {
            List<string> words = new List<string>();

            StreamReader sr = new StreamReader(stream, System.Text.Encoding.Default);
            string word = string.Empty;

            while (!string.IsNullOrEmpty(word = sr.ReadLine()))
            {
                if (string.IsNullOrEmpty(word))
                    continue;
                words.Add(word);
            }

            BatchCreate(words, typeId);
        }


        /// <summary>
        /// �����д�
        /// </summary>
        /// <param name="words">��дʼ���</param>
        /// <param name="typeId">��д�����Id</param>
        public void BatchCreate(IList<string> words, int? typeId)
        {
            if (words == null)
            {
                return;
            }

            words = words.Distinct().ToList();

            List<SensitiveWord> sensitiveWords = new List<SensitiveWord>();
            foreach (string word in words)
            {
                SensitiveWord sensitiveWord = SensitiveWord.New();
                if (word.Contains("="))
                {
                    string[] parts = word.Split('=');

                    if (parts.Count() == 2)
                    {
                        sensitiveWord.Word = parts[0];
                        sensitiveWord.Replacement = parts[1];
                    }
                }
                else
                {
                    sensitiveWord.Word = word;
                    sensitiveWord.Replacement = "*";
                }

                if (typeId.HasValue)
                {
                    sensitiveWord.TypeId = typeId.Value;
                }

                sensitiveWords.Add(sensitiveWord);

            }

            EventBus<SensitiveWord>.Instance().OnBatchBefore(sensitiveWords, new CommonEventArgs(EventOperationType.Instance().Create()));
            sensitiveWordRepository.BatchInsert(sensitiveWords);
            EventBus<SensitiveWord>.Instance().OnBatchAfter(sensitiveWords, new CommonEventArgs(EventOperationType.Instance().Create()));
        }


        /// <summary>
        /// ������д�
        /// </summary>
        /// <param name="sensitiveWord">��������д�</param>
        /// <returns></returns>
        public int Update(SensitiveWord sensitiveWord)
        {
            EventBus<SensitiveWord>.Instance().OnBefore(sensitiveWord, new CommonEventArgs(EventOperationType.Instance().Update()));
            int judge = sensitiveWordRepository.Update(sensitiveWord);
            EventBus<SensitiveWord>.Instance().OnAfter(sensitiveWord, new CommonEventArgs(EventOperationType.Instance().Update()));
            return judge;
        }

        /// <summary>
        /// ɾ����д�
        /// </summary>
        /// <param name="id">��д�Id</param>
        public void Delete(int id)
        {
            SensitiveWord word = sensitiveWordRepository.Get(id);

            EventBus<SensitiveWord>.Instance().OnBefore(word, new CommonEventArgs(EventOperationType.Instance().Update()));
            sensitiveWordRepository.Delete(word);
            EventBus<SensitiveWord>.Instance().OnBefore(word, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// ��ȡ��д�ʵ��
        /// </summary>
        /// <param name="id">��д�Id</param>
        public SensitiveWord Get(int id)
        {
            return sensitiveWordRepository.Get(id);
        }

        /// <summary>
        /// ��ȡ��дʼ��ϣ���̨�ã�
        /// </summary>
        /// <param name="keyword">��дʹؼ���</param>
        /// <param name="typeId">����Id</param>
        public IEnumerable<SensitiveWord> Gets(string keyword = "", int? typeId = null)
        {
            return sensitiveWordRepository.GetSensitiveWords(keyword, typeId);
        }

        /// <summary>
        /// ������д�
        /// </summary>
        /// <returns></returns>
        public byte[] Export()
        {
            IEnumerable<SensitiveWord> sensitiveWords = Gets();
            if (sensitiveWords == null)
                return null;
            
            byte[] bytes = new byte[65535];

            int i = 0;
            foreach (SensitiveWord sensitiveWord in sensitiveWords)
            {
                foreach (byte b in System.Text.ASCIIEncoding.Default.GetBytes(sensitiveWord.Word + "=" + sensitiveWord.Replacement + "\r\n"))
                {
                    bytes[i] = b;
                    i++;
                }
            }

            return bytes;
        }

        #endregion

        #region SensitiveWordType

        /// <summary>
        /// ������д�����
        /// </summary>
        public void CreateType(SensitiveWordType type)
        {
            sensitiveWordTypeRepository.Insert(type);
        }

        /// <summary>
        /// ɾ����д�����
        /// </summary>
        /// <param name="typeId">��д�����Id</param>
        public void DeleteType(object typeId)
        {
            sensitiveWordTypeRepository.DeleteByEntityId(typeId);
        }

        /// <summary>
        /// ������д�����
        /// </summary>
        /// <param name="type">��д�����ʵ��</param>
        public void UpdateType(SensitiveWordType type)
        {
            sensitiveWordTypeRepository.Update(type);
        }

        /// <summary>
        /// ��ȡ��д����ͼ���
        /// </summary>
        public IEnumerable<SensitiveWordType> GetAllSensitiveWordTypes()
        {
            return sensitiveWordTypeRepository.GetAll();
        }

        /// <summary>
        /// ��ȡ��д����ͼ���
        /// </summary>
        /// <param name="id">��д�����Id</param>
        public SensitiveWordType GetSensitiveWordType(int id)
        {
            return sensitiveWordTypeRepository.Get(id);
        }


        #endregion
    }
}
