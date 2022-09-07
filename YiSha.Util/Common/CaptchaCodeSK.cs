using SkiaSharp;

namespace YiSha.Util.Common
{
    /// <summary>
    /// 验证码 SkiaSharp 库（不用考虑太多跨平台问题）
    ///
    /// <para>添加包</para>
    /// <para>dotnet add package SkiaSharp</para>
    /// <para>dotnet add package SkiaSharp.NativeAssets.Linux</para>
    /// </summary>
    public class CaptchaCodeSK
    {
        /// <summary>
        /// Tuple 的第一个值是表达式
        /// Tuple 的第二个值是表达式结果
        /// </summary>
        /// <returns></returns>
        public static Tuple<string, int> GetCaptchaCode()
        {
            int value = 0;
            char[] operators = { '+', '-', '*' };
            string randomCode = string.Empty;
            Random random = new Random();

            int first = random.Next() % 10;
            int second = random.Next() % 10;
            char operatorChar = operators[random.Next(0, operators.Length)];
            switch (operatorChar)
            {
                case '+': value = first + second; break;
                case '-':
                    // 第1个数要大于第二个数
                    if (first < second)
                    {
                        int temp = first;
                        first = second;
                        second = temp;
                    }
                    value = first - second;
                    break;

                case '*': value = first * second; break;
            }

            char code = (char)('0' + (char)first);
            randomCode += code;
            randomCode += operatorChar;
            code = (char)('0' + (char)second);
            randomCode += code;
            randomCode += "=?";
            return new Tuple<string, int>(randomCode, value);
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="randomCode"></param>
        /// <returns></returns>
        public static byte[] CreateCaptchaImage(string randomCode)
        {
            Random random = new();
            //验证码颜色集合
            var colors = new[] { SKColors.Black, SKColors.Red, SKColors.DarkBlue, SKColors.Green, SKColors.Orange, SKColors.Brown, SKColors.DarkCyan, SKColors.Purple };
            //验证码字体集合
            var fonts = new[] { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            //相当于js的 canvas.getContext('2d')
            using var image2d = new SKBitmap(100, 30, SKColorType.Bgra8888, SKAlphaType.Premul);
            //相当于前端的canvas
            using var canvas = new SKCanvas(image2d);
            //填充白色背景
            canvas.DrawColor(SKColors.AntiqueWhite);
            //样式 跟xaml差不多
            using var drawStyle = new SKPaint();
            //填充验证码到图片
            for (int i = 0; i < randomCode.Length; i++)
            {
                drawStyle.IsAntialias = true;
                drawStyle.TextSize = 30;
                var font = SKTypeface.FromFamilyName(fonts[random.Next(0, fonts.Length - 1)], SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);
                drawStyle.Typeface = font;
                drawStyle.Color = colors[random.Next(0, colors.Length - 1)];
                //写字
                canvas.DrawText(randomCode[i].ToString(), (i + 1) * 16, 28, drawStyle);
            }
            //生成三条干扰线
            for (int i = 0; i < 3; i++)
            {
                drawStyle.Color = colors[random.Next(colors.Length)];
                drawStyle.StrokeWidth = 1;
                canvas.DrawLine(random.Next(0, randomCode.Length * 15), random.Next(0, 60), random.Next(0, randomCode.Length * 16), random.Next(0, 30), drawStyle);
            }
            //创建对象信息
            using var img = SKImage.FromBitmap(image2d);
            using var p = img.Encode(SKEncodedImageFormat.Png, 100);
            using var ms = new MemoryStream();
            //保存到流
            p.SaveTo(ms);
            var bytes = ms.GetBuffer();
            return bytes;
        }

        /// <summary>
        /// Tuple 的第一个值是表达式结果
        /// Tuple 的第二个值是表达式图片
        /// </summary>
        /// <returns></returns>
        public static Tuple<string, byte[]> GetImgData()
        {
            Tuple<string, int> tuple = GetCaptchaCode();
            var code = tuple.Item2.ToString();
            var img = CreateCaptchaImage(tuple.Item1);
            return new Tuple<string, byte[]>(code, img);
        }
    }
}
