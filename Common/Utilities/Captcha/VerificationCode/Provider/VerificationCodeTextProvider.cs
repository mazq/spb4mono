using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Drawing;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 文字提供者基类
    /// </summary>
    public abstract class VerificationCodeTextProvider : ProviderBase
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets whether a case-sensitive comparison is used when validating user input.
        /// </summary>
        /// <value>Indicates whether validation is case-sensitive.</value>
        public bool CaseSensitiveComparison
        {
            get
            {
                return caseSensitive;
            }
            set
            {
                caseSensitive = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters to be generated at random.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The specified value is not a positive integer.</exception>
        /// <value>The maximum number of characters allowed for the random text that the provider will generate.</value>
        public int MaximumCharacters
        {
            get
            {
                return maximumCharacters;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, "Errors.AIPTextProvider_MaxCharactersOutOfRange");

                maximumCharacters = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of characters to be generated at random.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The specified value is not a positive integer.</exception>
        /// <value>The minimum number of characters allowed for the random text that the provider will generate.</value>
        public int MinimumCharacters
        {
            get
            {
                return minimumCharacters;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, "Errors.AIPTextProvider_MinCharactersOutOfRange");

                minimumCharacters = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum <see cref="Font"/> size, in pixels, to be generated at random.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The specified value is not a positive integer.</exception>
        /// <value>The minimum <see cref="Font"/> size, in pixels, to be generated at random.</value>
        public int MinimumFontSize
        {
            get
            {
                return minFontSize;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, "Errors.PositiveIntRequired");

                minFontSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum <see cref="Font"/> size, in pixels, to be generated at random.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The specified value is not a positive integer.</exception>
        /// <value>The maximum <see cref="Font"/> size, in pixels, to be generated at random.</value>
        public int MaximumFontSize
        {
            get
            {
                return maxFontSize;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, "Errors.PositiveIntRequired");

                maxFontSize = value;
            }
        }

        /// <summary>
        /// Gets a list of <see cref="Color">Colors</see> from which an <see cref="AutoInputProtectionImage"/> implementation 
        /// may sample at random.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Types that derive from <see cref="AutoInputProtectionImage"/> can use this list in any way that makes sense for 
        /// their particular implementations, though typically a <see cref="Color"/> is sampled at random for every individual character 
        /// in the string being generated.  Instead, it might make sense for an implementation to sample only one color for the 
        /// entire string or to use multiple colors per character.
        /// </para>
        /// <para>
        /// At least one color must be specified in the provider's configuration or <see cref="Initialize"/> throws a <see cref="ProviderException"/>.
        /// </para>
        /// <para>
        /// <see cref="PartitionedAutoInputProtectionImage"/>, a commonly used implemenation of <see cref="AutoInputProtectionImage"/>, 
        /// also requires at least one color or else an <see cref="InvalidOperationException"/> is thrown when the composite image is about 
        /// to be created.
        /// </para>
        /// </remarks>
        /// <value>List of <see cref="Color">Colors</see> that may be sampled at random.</value>
        public IList<Color> Colors
        {
            get
            {
                return colors;
            }
        }

        /// <summary>
        /// Gets a list of <see cref="Font">Fonts</see> from which an <see cref="AutoInputProtectionImage"/> implementation 
        /// may sample at random.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Types that derive from <see cref="AutoInputProtectionImage"/> can use this list in any way that makes sense for 
        /// their particular implementations, though typically a <see cref="Font"/> is sampled at random for every individual character 
        /// in the string being generated.  Instead, it might make sense for an implementation to sample only one font for the 
        /// entire string.
        /// </para>
        /// <para>
        /// At least one font must be specified in the provider's configuration or <see cref="Initialize"/> throws a <see cref="ProviderException"/>.
        /// </para>
        /// <para>
        /// <see cref="PartitionedAutoInputProtectionImage"/>, a commonly used implemenation of <see cref="AutoInputProtectionImage"/>, 
        /// also requires at least one font or else an <see cref="InvalidOperationException"/> is thrown when the composite image is about 
        /// to be created.
        /// </para>
        /// </remarks>
        /// <value>List of <see cref="Font">Fonts</see> that may be sampled at random.</value>
        public IList<Font> Fonts
        {
            get
            {
                return fonts;
            }
        }
        #endregion

        #region Private / Protected
        private readonly List<Color> colors;
        private readonly List<Font> fonts;
        private bool caseSensitive;
        private int minimumCharacters, maximumCharacters, minFontSize, maxFontSize;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new instance of the <see cref="AutoInputProtectionTextProvider" /> class for derived classes.
        /// </summary>
        protected VerificationCodeTextProvider()
        {
            colors = new List<Color>(8);
            fonts = new List<Font>(8);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BasicEnglishAutoInputProtectionTextProvider" /> class for derived classes.
        /// </summary>
        /// <param name="colors">An <see cref="IEnumerable{T}"/> of <see cref="Color">Colors</see> from which an 
        /// <see cref="AutoInputProtectionImage"/> implementation may sample at random.</param>
        /// <param name="fonts">An <see cref="IEnumerable{T}"/> of <see cref="Font">Fonts</see> from which an 
        /// <see cref="AutoInputProtectionImage"/> implementation may sample at random.</param>
        protected VerificationCodeTextProvider(IEnumerable<Color> colors, IEnumerable<Font> fonts)
        {
            this.colors = new List<Color>(colors);
            this.fonts = new List<Font>(fonts);
            minFontSize = 20;
            maxFontSize = 30;
            minimumCharacters = 4;
            maximumCharacters = 6;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            base.Initialize(name, config);

            VerificationCodeProviderHelper helper = new VerificationCodeProviderHelper(config);

            colors.AddRange(helper.ParseCollection<Color>("colors", false, true, true, ',') ?? new List<Color>(0));

            if (colors.Count == 0)
                throw VerificationCodeProviderHelper.CreateException(null, "Errors.ProviderHelper_Required", "colors");

            IList<string> fontNames = helper.ParseList<string>("fonts", false, true, true, ',');

            if (fontNames == null || fontNames.Count == 0)
                throw VerificationCodeProviderHelper.CreateException(null, "Errors.ProviderHelper_Required", "fonts");

            fonts.Clear();
            fonts.Capacity = fontNames.Count;

            foreach (string fontName in fontNames)
            {
                if (!Array.Exists(FontFamily.Families, delegate(FontFamily family)
                {
                    return fontName.Equals(family.Name, StringComparison.OrdinalIgnoreCase);
                }))
                    throw VerificationCodeProviderHelper.CreateException(null, "Errors.AIPImageProvider_InvalidFontName", fontName, "fonts");

                fonts.Add(new Font(fontName, 1));
            }

            minFontSize = helper.ParseInt32("minimumFontSize", false, 20);
            maxFontSize = helper.ParseInt32("maximumFontSize", false, 30);
            minimumCharacters = helper.ParseInt32("minimumCharacters", false, 4);
            maximumCharacters = helper.ParseInt32("maximumCharacters", false, 6);

            if (maximumCharacters < 1 || maximumCharacters < minimumCharacters)
                throw VerificationCodeProviderHelper.CreateException(null, "Errors.AIPTextProvider_MaxCharactersOutOfRangeConfig");

            if (minimumCharacters < 1)
                throw VerificationCodeProviderHelper.CreateException(null, "Errors.AIPTextProvider_MinCharactersOutOfRangeConfig");

            caseSensitive = helper.ParseBoolean("caseSensitive", false, false);
        }

        /// <summary>
        /// 验证输入正确性
        /// </summary>
        /// <param name="text"></param>
        /// <param name="imageText"></param>
        /// <returns></returns>
        public virtual bool ValidateUserInput(string text, string imageText)
        {
            return string.Equals(text, imageText, (CaseSensitiveComparison) ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
        }

        public abstract string GenerateRandomAutoInputProtectionText(CaptchaCharacterSet characterSet);
        #endregion
    }

    public class AutoInputProtectionTextProviderCollection : ProviderCollection, ICollection<VerificationCodeTextProvider>
    {
        public new VerificationCodeTextProvider this[string name]
        {
            get
            {
                return (VerificationCodeTextProvider)base[name];
            }
        }

        public VerificationCodeTextProvider this[int index]
        {
            get
            {
                int i = 0;
                foreach (VerificationCodeTextProvider provider in this)
                {
                    if (i++ == index)
                        return provider;
                }

                throw new ArgumentOutOfRangeException("index");
            }
        }

        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            VerificationCodeTextProvider item = provider as VerificationCodeTextProvider;

            if (item == null)
                throw new ArgumentException("Errors.AIPTextProviderCollection_InvalidType", "provider");

            Add(item);
        }

        #region ICollection<AutoInputProtectionTextProvider> Members
        /// <summary>
        /// Adds the specified <paramref name="item"/> to the collection.
        /// </summary>
        /// <exception cref="ArgumentNullException">The specified <paramref name="item"/> is <see langword="null"/>.</exception>
        /// <param name="item">The <see cref="AutoInputProtectionTextProvider"/> instance to be added to the collection.</param>
        public void Add(VerificationCodeTextProvider item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            base.Add(item);
        }

        /// <summary>
        /// Returns whether the specified <paramref name="item"/> is contained by the collection.
        /// </summary>
        /// <param name="item">The <see cref="AutoInputProtectionTextProvider"/> instance to check.</param>
        /// <returns><see langword="true">True</see> if the specified <paramref name="item"/> is contained by the collection; otherwise, 
        /// <see langword="false"/>.</returns>
        public bool Contains(VerificationCodeTextProvider item)
        {
            if (item == null)
                return false;

            foreach (VerificationCodeTextProvider provider in this)
                if (provider == item)
                    return true;

            return false;
        }

        /// <summary>
        /// Copies the entire collection to the specified <paramref name="array"/> starting at the specified <paramref name="arrayIndex"/>.
        /// </summary>
        /// <param name="array">An array of <see cref="AutoInputProtectionTextProvider"/> to which the collection will be copied.</param>
        /// <param name="arrayIndex">The index at which copying will begin.</param>
        public void CopyTo(VerificationCodeTextProvider[] array, int arrayIndex)
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
        /// <param name="item">The <see cref="AutoInputProtectionTextProvider"/> implementation to be removed.</param>
        /// <returns><see langword="true">True</see> if the specified <paramref name="item"/> is removed from the collection; otherwise, <see langword="false"/>.</returns>
        public bool Remove(VerificationCodeTextProvider item)
        {
            if (item == null)
                return false;

            bool contained = Contains(item);

            if (contained)
                base.Remove(item.Name);

            return contained;
        }
        #endregion

        #region IEnumerable<AutoInputProtectionTextProvider> Members
        /// <summary>
        /// Gets an object that can iterate over the entire collection of <see cref="AutoInputProtectionTextProvider"/> instances.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="AutoInputProtectionTextProvider"/> that can iterate over the entire collection.</returns>
        public new IEnumerator<VerificationCodeTextProvider> GetEnumerator()
        {
            foreach (VerificationCodeTextProvider provider in (System.Collections.IEnumerable)this)
                yield return provider;
        }
        #endregion
    }
}
