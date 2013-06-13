
namespace Spacebuilder.Common
{
    /// <summary>
    /// 数据存放位置枚举
    /// </summary>
    public enum VerificationCodePersistenceMode
    {
        /// <summary>
        /// 缓存
        /// </summary>
        Cache,
        /// <summary>
        /// session
        /// </summary>
        Session
    }

    /// <summary>
    /// 验证码难易度
    /// </summary>
    public enum CaptchaDifficultyLevel
    {
        /// <summary>
        /// 低
        /// </summary>
        Low = 0,
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 难
        /// </summary>
        Hard = 2
    }

    /// <summary>
    /// 验证码使用场景
    /// </summary>
    public enum VerifyScenarios
    {
        /// <summary>
        /// 用户登录时
        /// </summary>
        Login = 1,

        /// <summary>
        /// 发布内容时
        /// </summary>
        Post = 2,

        /// <summary>
        /// 用户注册时
        /// </summary>
        Register = 3
    }
}
