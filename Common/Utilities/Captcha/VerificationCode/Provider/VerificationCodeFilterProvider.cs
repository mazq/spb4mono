using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Drawing;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 干扰提供者基类
    /// </summary>
    public abstract class VerificationCodeFilterProvider : ProviderBase, IDisposable
    {
        //	过滤器允许用户增加随机的图像的AIP生成和改变外观形象
        #region Public Properties
        /// <summary>
        /// When implemented by a derived type, gets whether preprocessing is supported.
        /// </summary>
        /// <remarks>
        /// When a derived type returns <see langword="false"/> the <see cref="PreProcess"/> method will not be invoked by AIP.
        /// </remarks>
        /// <seealso cref="PreProcess"/>
        /// <value>Indicates whether preprocessing is supported by the filter.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "PreProcess")]
        public abstract bool CanPreProcess { get; }

        /// <summary>
        /// When implemented by a derived type, gets whether post-processing is supported.
        /// </summary>
        /// <remarks>
        /// When a derived type returns <see langword="false"/> the <see cref="PostProcess"/> method will not be invoked by AIP.
        /// </remarks>
        /// <seealso cref="PostProcess"/>
        /// <value>Indicates whether post-processing is supported by the filter.</value>
        public abstract bool CanPostProcess { get; }
        #endregion

        #region Private / Protected
        /// <summary>
        /// Gets whether the class is disposed.
        /// </summary>
        protected bool IsDisposed
        {
            get
            {
                return disposed;
            }
        }

        private volatile bool disposed;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new instance of the <see cref="AutoInputProtectionFilterProvider" /> class for derived classes.
        /// </summary>
        protected VerificationCodeFilterProvider()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// When implemented by a derived type, applies the filter to the specified <paramref name="graphics" /> object before the text is rendered.
        /// </summary>
        /// <remarks>
        /// When this method is invoked by AIP the specified <paramref name="graphics"/> object does not yet contain the random text but it does contain the 
        /// background image and the results from all of the preprocessing filters that have executed before this one.
        /// </remarks>
        /// <seealso cref="CanPreProcess"/>
        /// <param name="graphics"><see cref="Graphics" /> object that is the surface on which to draw.</param>
        /// <param name="imageSize">The dimensions of the image.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "PreProcess")]
        public abstract void PreProcess(Graphics graphics, Size imageSize);

        /// <summary>
        /// When implemented by a derived type, applies the filter to the specified <paramref name="graphics" /> object after the text is rendered.
        /// </summary>
        /// <remarks>
        /// When this method is invoked by AIP the specified <paramref name="graphics"/> object already has random text, the background image and the results 
        /// from all of the preprocessing filters already rendered.  The results from all of the post-processing filters that have executed before this one 
        /// will already be rendered as well.
        /// </remarks>
        /// <seealso cref="CanPostProcess"/>
        /// <param name="graphics"><see cref="Graphics" /> object that is the surface on which to draw.</param>
        /// <param name="imageSize">The dimensions of the image.</param>
        public abstract void PostProcess(Graphics graphics, Size imageSize);
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases all resources used by an instance of the <see cref="AutoInputProtectionFilterProvider" /> class.
        /// </summary>
        /// <remarks>
        /// This method calls the virtual <see cref="Dispose(bool)" /> method, passing in <strong>true</strong>, and then suppresses 
        /// finalization of the instance.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources before an instance of the <see cref="AutoInputProtectionFilterProvider" /> class is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// This method releases unmanaged resources by calling the virtual <see cref="Dispose(bool)" /> method, passing in <strong>false</strong>.
        /// </remarks>
        ~VerificationCodeFilterProvider()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by an instance of the <see cref="AutoInputProtectionFilterProvider" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><strong>true</strong> to release both managed and unmanaged resources; <strong>false</strong> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                disposed = true;
        }
        #endregion
    }

    public sealed class AutoInputProtectionFilterProviderCollection : ProviderCollection, ICollection<VerificationCodeFilterProvider>
    {
        #region Public Properties
        /// <summary>
        /// Gets the <see cref="AutoInputProtectionFilterProvider"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the <see cref="AutoInputProtectionFilterProvider"/> to be retrieved.</param>
        /// <returns>The <see cref="AutoInputProtectionFilterProvider"/> with the specified <paramref name="name"/>.</returns>
        public new VerificationCodeFilterProvider this[string name]
        {
            get
            {
                return (VerificationCodeFilterProvider)base[name];
            }
        }

        /// <summary>
        /// Gets the <see cref="AutoInputProtectionFilterProvider"/> instance at the specified <paramref name="index"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The specified <paramref name="index"/> is less than zero or is 
        /// greater than or equal to the size of the collection.</exception>
        /// <param name="index">The zero-based index of the <see cref="AutoInputProtectionFilterProvider"/> instance to be retrieved.</param>
        /// <returns>The <see cref="AutoInputProtectionFilterProvider"/> instance at the specified <paramref name="index"/>.</returns>
        public VerificationCodeFilterProvider this[int index]
        {
            get
            {
                int i = 0;
                foreach (VerificationCodeFilterProvider provider in this)
                {
                    if (i++ == index)
                        return provider;
                }

                throw new ArgumentOutOfRangeException("index");
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new instance of the <see cref="AutoInputProtectionFilterProviderCollection" /> class.
        /// </summary>
        public AutoInputProtectionFilterProviderCollection()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the specified <paramref name="provider"/> to the collection.
        /// </summary>
        /// <exception cref="ArgumentNullException">The specified <paramref name="provider"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The specified <paramref name="provider"/> does not derive from <see cref="AutoInputProtectionFilterProvider"/>.</exception>
        /// <param name="provider">An <see cref="AutoInputProtectionFilterProvider"/> implementation to be added to the collection.</param>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            VerificationCodeFilterProvider item = provider as VerificationCodeFilterProvider;

            if (item == null)
                throw new ArgumentException("Errors.AIPFilterProviderCollection_InvalidType", "provider");

            Add(item);
        }
        #endregion

        #region ICollection<AutoInputProtectionFilterProvider> Members
        /// <summary>
        /// Adds the specified <paramref name="item"/> to the collection.
        /// </summary>
        /// <param name="item">The <see cref="AutoInputProtectionFilterProvider"/> implementation to be added to the collection.</param>
        public void Add(VerificationCodeFilterProvider item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            base.Add(item);
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        void ICollection<VerificationCodeFilterProvider>.Clear()
        {
            Clear();
        }

        /// <summary>
        /// Returns whether the collection contains the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The <see cref="AutoInputProtectionFilterProvider"/> implementation to check.</param>
        /// <returns><see langword="true">True</see> if the specified <paramref name="item"/> exists in the collection; otherwise, <see langword="false"/>.</returns>
        public bool Contains(VerificationCodeFilterProvider item)
        {
            if (item == null)
                return false;

            foreach (VerificationCodeFilterProvider provider in this)
                if (provider == item)
                    return true;

            return false;
        }

        /// <summary>
        /// Copies the entire collection to the specified <paramref name="array"/> starting at the specified <paramref name="arrayIndex"/>.
        /// </summary>
        /// <param name="array">An array of <see cref="AutoInputProtectionFilterProvider"/> to which the collection will be copied.</param>
        /// <param name="arrayIndex">The index at which copying will begin.</param>
        public void CopyTo(VerificationCodeFilterProvider[] array, int arrayIndex)
        {
            base.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets whether the collection is read-only.
        /// </summary>
        /// <value>Indicates whether the collection is read-only.  This property always returns <see langword="false"/>.</value>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the specified <paramref name="item"/> from the collection.
        /// </summary>
        /// <param name="item">The <see cref="AutoInputProtectionFilterProvider"/> implementation to be removed.</param>
        /// <returns><see langword="true">True</see> if the specified <paramref name="item"/> is removed from the collection; otherwise, <see langword="false"/>.</returns>
        public bool Remove(VerificationCodeFilterProvider item)
        {
            if (item == null)
                return false;

            bool contained = Contains(item);

            if (contained)
                base.Remove(item.Name);

            return contained;
        }
        #endregion

        #region IEnumerable<AutoInputProtectionFilterProvider> Members
        /// <summary>
        /// Gets an object that can iterate over the entire collection of <see cref="AutoInputProtectionFilterProvider"/> instances.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="AutoInputProtectionFilterProvider"/> that can iterate over the entire collection.</returns>
        public new IEnumerator<VerificationCodeFilterProvider> GetEnumerator()
        {
            foreach (VerificationCodeFilterProvider provider in (System.Collections.IEnumerable)this)
                yield return provider;
        }
        #endregion
    }
}
