using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POS.Alipay
{
    /// <summary>
    /// 基础配置类
    /// </summary>
    public class AlipayConfig
    {
        //支付宝公钥
        public static string alipay_public_key = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwphVHpmunx43gbe6LovSxsAEynE2J76Wn706FTguRDVfdcp2XfY8+ClBJNjYGbttxXCGYb6gkpXV60IzUNiLyeieN71LuHsAdFOWdxUmkba9UooBZUyiE+DYlvhXDfUMNFtWZzlbkEZI7EtXZ9elqS25RamVIS9AtJjfwuhS20Loz8WG+nUXLWDCPsAvm8JKlYnBePWzhTQOw4Y4pFIx0PzkeuNAw0ypznFHKcloenKXWvs2UCCCn5hLzXSoirwJK42IhrG1d8sfusr9CXJfconkZUTirTVq/gBMSRjokzKxsPSo+f0C3ELCJbOIF0oUtoqAcm060utUy5HZuupXDQIDAQAB";


        //这里要配置没有经过的原始私钥

        //开发者私钥（应用私钥）
        public static string merchant_private_key = @"MIIEowIBAAKCAQEAvqWHoY+kz3hrlIoZx17H7HS0z7oLfc+vFJBQ1OkZp4V5MEzd6wAAA1fjS3Yn4l/1ju2J9NkrDVavIxlEjqsoTeJCDJynwh57GF/VrqYbdDtr44OHmxBloP4Jm5J8dGZLnRpNvK7hB6PkJ3CQGxX3kwFtvcorXnfr+9IyqF05cC0177hHi479IqFZ/FLFdIRQLXUJUIoFjcA/XsmCiTDr9yO5ItalAfVn6ULVRCpCD8JpY0IjpERreStNaWBVWBmgsm+/q8G+Oips9uN9saWPbo4xDUSrfvGCBF9ppVpsPLSe7O3E0OSSoVLotNupFzjnHGTAaAR3IYXh5HGiae80gwIDAQABAoIBAAL7Uci8F2bLSltzYX3VxKi1FpLdJQrdsa5Pp4P89VLCaqfPu/TN9jIXMM3gVjVbLNsEahDzSJIX1ftljMXydVnqfP/3f0Qw99nkvdHSdUF3IHPZfch2FDDVaE8PlnS3mN0nKSoXThuxe4MGZqqVyF57Oj0qqUz78SvqGLK7aoePlom8O63c9naDrNJlIwJTtRJSFooPhwtnPZzP/vwnBfOIMS0sHxcnCCLgKkdmlITFbO2s7vRl2jkvcqAH7wiQs+vlgsSjqXLl84MSAWUTaWQWB6ITCP2W+teUJCC2YiGdWW/w03tXwANpZ9n/0JD1YGAn2C+wXIHADMArKxvflRkCgYEA6RogEK2+wqJadzHbJ2bHRtJ1Krd2I9icV2HYN+h9SH+zInrvOOfjQuZEeLNpU1a+KNZW7qUsy/wfGxWxVNJx/BCaHZSUB0gJN+fmDmegSmCuhXg5jMvV3xnLxY8nkf+FXkA3rWatlssPtMERPSG4n/kac8LWGoS2vMvi5Qid76cCgYEA0V/EUVEKVxqXeSBkyOVZSMlaEQL1IqlUmszWXlJGuA7BlRD6Fp3pXLr9Ev7uaIQVD5KymtBvoboR/QJfkEHJfQVhFLDrwcuRPsstRY9AWGkt6f7F69fUWRlJCYY2I2sFB+hWVFAupyEVrns96uRIP8MUREUjUtgcsadwM0EtD8UCgYEAtAcIUrlNRdbL1Zi8sD6Rk2IBOZ99b4vgmyAnNG6rK63sdKnLTgDRpR5gLXQq2UF6FeIMO713zocGow1q6p6ph8purH/On50D+ZhxNhjUU/09bDmBAPuATxEGLqs0HW7h2vvvxE0zre1gUPJ3VOZRR8PEqTkF0G9FDqTMtKSmDasCgYAl3tqVksD6DFdFk3RnWgSSQ8lEtnI8eMBS16YWnW8pL5IYMl/p41ooabII04+v2+QrUu93TShD6nICf4FliGC3elea9H1QKk/1Oa7QnB6ZXzzszCZkiD9idk5dqKb2NQ9N5NpombA2jqdrTeNfLzdaQSDTUGYZP/nAsHZ87tcFuQKBgAqYUwXcnk+A7r8SxUTwfY+FPs4OdZRMPh0sbPJzFqNKpTYbLO9t6Ruz79+PGKKKw6H30Th2Z7RNm0VYDW0VoJ0m34fRYHe1QMy8YjWhrBRN2D3yR7TZQFLnq9yuwy70tdJaS3VFpqlV581Ycg3dtVJhi4SX+oSMrSXvFEvRoM+L";

        //开发者公钥（应用公钥）
        public static string merchant_public_key = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvqWHoY+kz3hrlIoZx17H7HS0z7oLfc+vFJBQ1OkZp4V5MEzd6wAAA1fjS3Yn4l/1ju2J9NkrDVavIxlEjqsoTeJCDJynwh57GF/VrqYbdDtr44OHmxBloP4Jm5J8dGZLnRpNvK7hB6PkJ3CQGxX3kwFtvcorXnfr+9IyqF05cC0177hHi479IqFZ/FLFdIRQLXUJUIoFjcA/XsmCiTDr9yO5ItalAfVn6ULVRCpCD8JpY0IjpERreStNaWBVWBmgsm+/q8G+Oips9uN9saWPbo4xDUSrfvGCBF9ppVpsPLSe7O3E0OSSoVLotNupFzjnHGTAaAR3IYXh5HGiae80gwIDAQAB";
        //应用ID
        public static string appId = "2018030802335882";

        //合作伙伴ID：partnerID
        public static string pid = "2088411310675202";
        //支付宝网关
        public static string serverUrl = "https://openapi.alipay.com/gateway.do";
        public static string mapiUrl = "https://mapi.alipay.com/gateway.do";
        public static string monitorUrl = "http://mcloudmonitor.com/gateway.do";

        //编码，无需修改
        public static string charset = "utf-8";
        //签名类型，支持RSA2（推荐！）、RSA
        //public static string sign_type = "RSA2";
        public static string sign_type = "RSA2";
        //版本号，无需修改
        public static string version = "1.0";


        /// <summary>
        /// 公钥文件类型转换成纯文本类型
        /// </summary>
        /// <returns>过滤后的字符串类型公钥</returns>
        public static string getMerchantPublicKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_public_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }

        /// <summary>
        /// 私钥文件类型转换成纯文本类型
        /// </summary>
        /// <returns>过滤后的字符串类型私钥</returns>
        public static string getMerchantPriveteKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_private_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }
    }
}
