using System;
using System.IO;
using SkiaSharp;

namespace YiSha.Util
{
    public class CaptchaHelperSkiaSharp
    {
        /// <summary>
        /// 生成验证码图片（跨平台，SkiaSharp实现）
        /// </summary>
        /// <param name="randomCode"></param>
        /// <returns></returns>
        public static byte[] CreateCaptchaImage(string randomCode)
        {
            int width = (int)(randomCode.Length * 16);
            int height = 28;
            using (var bitmap = new SKBitmap(width, height))
            using (var canvas = new SKCanvas(bitmap))
            {
                var random = new Random();
                // 填充背景
                canvas.Clear(SKColors.AliceBlue);

                // 绘制干扰曲线
                for (int i = 0; i < 2; i++)
                {
                    using (var paint = new SKPaint { Color = SKColors.Gray, StrokeWidth = 1, IsAntialias = true, Style = SKPaintStyle.Stroke })
                    {
                        var path = new SKPath();
                        path.MoveTo(0, random.Next(height));
                        path.CubicTo(
                            random.Next(width), random.Next(height),
                            random.Next(width), random.Next(height),
                            width, random.Next(height)
                        );
                        canvas.DrawPath(path, paint);
                    }
                }

                // 绘制验证码文字
                var colors = new[] { SKColors.Black, SKColors.Red, SKColors.DarkBlue, SKColors.Green, SKColors.Orange, SKColors.Brown, SKColors.DarkCyan, SKColors.Purple };
                var fonts = new[] { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
                int cindex = random.Next(colors.Length);
                float x = 14;
                float y = height / 2f + 2;
                foreach (char t in randomCode)
                {
                    int findex = random.Next(fonts.Length);
                    using (var paint = new SKPaint { Color = colors[cindex], TextSize = 18, IsAntialias = true, Typeface = SKTypeface.FromFamilyName(fonts[findex], SKFontStyle.Bold) })
                    {
                        float angle = random.Next(-45, 45);
                        canvas.Save();
                        canvas.Translate(x, y);
                        if (t != '+' && t != '-' && t != '*')
                        {
                            canvas.RotateDegrees(angle);
                        }
                        canvas.DrawText(t.ToString(), 0, 0, paint);
                        canvas.Restore();
                        x += 16;
                    }
                }

                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    return data.ToArray();
                }
            }
        }
    }
}

