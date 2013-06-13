using System;
using System.Drawing;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 实现操作提供者集合的基类
    /// </summary>
    public abstract class PartitionedVerificationCodeImageProvider : VerificationCodeImageProvider
    {
        private int margin, minRotation, maxRotation;

        #region Public Properties
        /// <summary>
        /// Gets or sets the left, right, bottom and top margins from the edges of the image in which the text will be rendered, in pixels.
        /// </summary>
        /// <value>The margin in which the text will be rendered, in pixels.</value>
        public int Margin
        {
            get
            {
                return margin;
            }
            set
            {
                margin = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum amount, in degrees, of random rotation that is applied to each character.
        /// </summary>
        /// <value>The minimum amount, in degrees, of random rotation that is applied to each character.</value>
        public int MinimumCharacterRotation
        {
            get
            {
                return minRotation;
            }
            set
            {
                // negatives are allowed
                minRotation = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum amount, in degrees, of random rotation that is applied to each character.
        /// </summary>
        /// <value>The maximum amount, in degrees, of random rotation that is applied to each character</value>
        public int MaximumCharacterRotation
        {
            get
            {
                return maxRotation;
            }
            set
            {
                // negatives are allowed
                maxRotation = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new instance of the <see cref="PartitionedAutoInputProtectionImageProvider" /> class for derived classes.
        /// </summary>
        protected PartitionedVerificationCodeImageProvider()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the provider's settings from the specified <paramref name="config"/>.
        /// </summary>
        /// <exception cref="System.Configuration.Provider.ProviderException">An attribute value cannot be converted into the required type.</exception>
        /// <param name="name">Name of the provider.</param>
        /// <param name="config">A collection of name and value pairs that will be used to initialize the provider.</param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            base.Initialize(name, config);

            VerificationCodeProviderHelper helper = new VerificationCodeProviderHelper(config);

            margin = helper.ParseInt32("margin", false, -1);
            minRotation = helper.ParseInt32("minimumCharacterRotation", false, -1);
            maxRotation = helper.ParseInt32("maximumCharacterRotation", false, -1);
        }

        /// <summary>
        /// Creates an instance of <see cref="PartitionedAutoInputProtectionImage"/> for the specified <paramref name="background"/>
        /// image and <paramref name="textProvider"/>.
        /// </summary>
        /// <remarks>
        /// Derived types can override this method to return a type that either composites or derives from 
        /// <see cref="PartitionedAutoInputProtectionImage"/>.
        /// </remarks>
        /// <seealso cref="GenerateAutoInputProtectionImage(string,Size,AutoInputProtectionTextProvider)"/>
        /// <param name="background">The <see cref="Image"/> that is used as the surface on which the text is rendered.</param>
        /// <param name="textProvider">The <see cref="AutoInputProtectionTextProvider"/> implementation that generates the text to be rendered.</param>
        /// <returns>An instance of <see cref="PartitionedAutoInputProtectionImage"/> for the specified <paramref name="background"/>
        /// image and <paramref name="textProvider"/>.</returns>
        protected virtual PartitionedVerificationCodeImage GenerateImage(Image background, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
        {
            PartitionedVerificationCodeImage image = new PartitionedVerificationCodeImage(background, textProvider, characterSet, maximumCharacters, minimumCharacters);

            if (margin != -1)
                image.Margin = margin;

            if (minRotation != -1)
                image.MinimumCharacterRotation = minRotation;

            if (maxRotation != -1)
                image.MaximumCharacterRotation = maxRotation;

            return image;
        }

        /// <summary>
        /// Creates an instance of <see cref="PartitionedAutoInputProtectionImage"/> for the specified <paramref name="size"/>
        /// and <paramref name="textProvider"/>.
        /// </summary>
        /// <remarks>
        /// Derived types can override this method to return a type that either composites or derives from 
        /// <see cref="PartitionedAutoInputProtectionImage"/>.
        /// </remarks>
        /// <seealso cref="GenerateRandomAutoInputProtectionImage(Size,AutoInputProtectionTextProvider)"/>
        /// <param name="size">The dimensions of the image.</param>
        /// <param name="textProvider">The <see cref="AutoInputProtectionTextProvider"/> implementation that generates the text to be rendered.</param>
        /// <returns>An instance of <see cref="PartitionedAutoInputProtectionImage"/> for the specified <paramref name="size"/>
        /// and <paramref name="textProvider"/>.</returns>
        protected virtual PartitionedVerificationCodeImage GenerateImage(Size size, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
        {
            PartitionedVerificationCodeImage image = new PartitionedVerificationCodeImage(size, textProvider, characterSet, maximumCharacters, minimumCharacters);

            if (margin != -1)
                image.Margin = margin;

            if (minRotation != -1)
                image.MinimumCharacterRotation = minRotation;

            if (maxRotation != -1)
                image.MaximumCharacterRotation = maxRotation;

            return image;
        }

        /// <summary>
        /// When implemented by a derived type, creates an instance of a type that derives from <see cref="AutoInputProtectionImage"/>
        /// that generates an image of the specified <paramref name="size"/>, using the specified <paramref name="name"/> and 
        /// <paramref name="textProvider"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Derived types are not required to use the <paramref name="name"/> argument.  This overload is not used by the 
        /// <see cref="AutoInputProtectionControl"/> control.
        /// </para>
        /// <para>
        /// A derived type can use the specified <paramref name="name"/> to load or generate a background <see cref="Image"/> 
        /// that is passed to <see cref="GenerateImage(Image,AutoInputProtectionTextProvider)"/>, which creates an instance of 
        /// <see cref="PartitionedAutoInputProtectionImage"/> that is then returned to the caller immediately or wrapped by a specialized class that
        /// is returned instead.  Alternatively, a derived type may override <see cref="GenerateImage(Image,AutoInputProtectionTextProvider)"/> to 
        /// create a custom implementation that is then returned by this method as is.
        /// </para>
        /// <para>
        /// See <see cref="ResourceAutoInputProtectionImageProvider"/> for information about one particular usage of the <paramref name="name"/> argument.
        /// </para>
        /// </remarks>
        /// <seealso cref="AutoInputProtection.GenerateAutoInputProtectionImage"/>
        /// <seealso cref="GenerateRandomAutoInputProtectionImage"/>
        /// <param name="name"><see cref="String"/> that identifies a particular named resource, style or image.</param>
        /// <param name="size">The dimensions of the image.</param>
        /// <param name="textProvider"><see cref="AutoInputProtectionTextProvider"/> implementation to be used by the <see cref="AutoInputProtectionImage"/> implementation that is created.</param>
        /// <returns>An <see cref="AutoInputProtectionImage"/> implementation that can generate an image of the specified <paramref name="size"/> using the specified
        /// <paramref name="textProvider"/>.</returns>
        public abstract override VerificationCodeImage GenerateAutoInputProtectionImage(string name, Size size, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters);

        /// <summary>
        /// When implemented by a derived type, creates an instance of a type that derives from <see cref="AutoInputProtectionImage"/>
        /// that generates an image of the specified <paramref name="size"/> using the specified <paramref name="textProvider"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This overload is used by the <see cref="AutoInputProtectionControl"/> control.
        /// </para>
        /// <para>
        /// A derived type can call <see cref="GenerateImage(Size,AutoInputProtectionTextProvider)"/> to create an instance of 
        /// <see cref="PartitionedAutoInputProtectionImage"/>, which can then be returned to the caller immediately or it can be wrapped by a specialied class that 
        /// is returned instead.  Alternatively, a derived type may override <see cref="GenerateImage(Size,AutoInputProtectionTextProvider)"/> to create a custom 
        /// implementation that is then returned by this method as is.
        /// </para>
        /// </remarks>
        /// <seealso cref="AutoInputProtection.GenerateAutoInputProtectionImage"/>
        /// <seealso cref="GenerateAutoInputProtectionImage"/>
        /// <param name="size">The dimensions of the image.</param>
        /// <param name="textProvider"><see cref="AutoInputProtectionTextProvider"/> implementation to be used by the <see cref="AutoInputProtectionImage"/> implementation that is created.</param>
        /// <returns>An <see cref="AutoInputProtectionImage"/> implementation that can generate an image of the specified <paramref name="size"/> using the specified
        /// <paramref name="textProvider"/>.</returns>
        public abstract override VerificationCodeImage GenerateRandomAutoInputProtectionImage(Size size, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters);
        #endregion
    }
}
