#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Library.Helpers
{
    public static class UIHelper
    {
        public static Image.GetThumbnailImageAbort thumbnailCallback = ThumbnailCallback;

        public static bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="size"></param>
        /// <remarks>
        ///     This method will throw a AccessViolationException when the machine OS running the server code is windows 7.
        /// </remarks>
        /// <returns></returns>
        public static byte[] CreateThumbnail(byte[] imageData, Size size)
        {
            using (var inStream = new MemoryStream())
            {
                inStream.Write(imageData, 0, imageData.Length);

                using (var image = Image.FromStream(inStream))
                {
                    //do not make image bigger
                    if (size.Equals(image.Size) || (image.Width < size.Width || image.Height < size.Height))
                    {
                        //if no shrinking is ocurring, return the original bytes
                        return imageData;
                    }

                    using (var thumb = image.GetThumbnailImage(size.Width, size.Height, thumbnailCallback, IntPtr.Zero))
                    {
                        using (var outStream = new MemoryStream())
                        {
                            thumb.Save(outStream, ImageFormat.Jpeg);

                            return outStream.ToArray();
                        }
                    }
                }
            }
        }

        public static byte[] ResizeTo(byte[] imageData, int width, int height)
        {
            var imgToResize = Image.FromStream(new MemoryStream(imageData));
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0F;
            int destWidth = 0;
            int destHeight = 0;

            if (sourceWidth > sourceHeight)
            {
                //nPercent = ((float)width / (float)sourceWidth);
                //destWidth = (int)(sourceWidth * nPercent);
                //destHeight = (int)(sourceHeight * nPercent);

                nPercent = ((float)width / (float)sourceWidth);
                destWidth = width + (int)(sourceWidth * nPercent);
                destHeight = height;

            }
            else if (sourceWidth < sourceHeight)
            {
                //nPercent = ((float)height / (float)sourceHeight);
                //destWidth = (int)(sourceWidth * nPercent);
                //destHeight = (int)(sourceHeight * nPercent);

                nPercent = ((float)height / (float)sourceHeight);
                destWidth = width;
                destHeight = height + (int)(sourceWidth * nPercent);
            }
            else if (sourceWidth == sourceHeight)
            {
                nPercent = ((float)height / (float)sourceHeight);
                destWidth = (int)(sourceWidth * nPercent);
                destHeight = (int)(sourceHeight * nPercent);
            }

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            byte[] buffer;
            using (var outStream = new MemoryStream())
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Png);
                EncoderParameters ep = new EncoderParameters();
                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)50);

                b.Save(outStream, jpgEncoder, ep);
                buffer = outStream.ToArray();
            }
            return buffer;
        }

        public static Image Crop(Image image, int width, int height)
        {
            int cropx = image.Width > width ? image.Width / 2 - width / 2 : 0;
            int cropy = image.Height > height ? image.Height / 2 - height / 2 : 0;
            width = image.Width > width ? width : image.Width;
            height = image.Height > height ? height : image.Height;

            Rectangle cropRect = new Rectangle(cropx, cropy, width, height);

            var target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(image, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
            }

            return target;
        }

        public static Image Resize(this Image img, int srcX, int srcY, int srcWidth, int srcHeight, int dstWidth, int dstHeight)
        {
            var bmp = new Bitmap(dstWidth, dstHeight);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    var destRect = new Rectangle(0, 0, dstWidth, dstHeight);
                    graphics.DrawImage(img, destRect, srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return bmp;
        }

        public static Image ResizeProportional(this Image img, int width, int height, bool enlarge = false)
        {
            double ratio = Math.Max(img.Width / (double)width, img.Height / (double)height);
            if (ratio < 1 && !enlarge) return img;
            return img.Resize(0, 0, img.Width, img.Height, (int)Math.Round(img.Width / ratio), (int)Math.Round(img.Height / ratio));
        }

        public static Image ResizeCropExcess(this Image img, int dstWidth, int dstHeight)
        {
            double srcRatio = img.Width / (double)img.Height;
            double dstRatio = dstWidth / (double)dstHeight;
            int srcX, srcY, cropWidth, cropHeight;

            if (srcRatio < dstRatio) // trim top and bottom
            {
                cropHeight = dstHeight * img.Width / dstWidth;
                srcY = (img.Height - cropHeight) / 2;
                cropWidth = img.Width;
                srcX = 0;
            }
            else // trim left and right
            {
                cropWidth = dstWidth * img.Height / dstHeight;
                srcX = (img.Width - cropWidth) / 2;
                cropHeight = img.Height;
                srcY = 0;
            }

            return Resize(img, srcX, srcY, cropWidth, cropHeight, dstWidth, dstHeight);
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            int destWidth = 0;
            int destHeight = 0;

            if (nPercentW > nPercentH)
            {
                nPercent = nPercentW;
            }
            else
            {
                nPercent = nPercentH;
            }
            destWidth = (int)(sourceWidth * nPercent);
            destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);

            //g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        public static byte[] ResizeImage(byte[] imageData, int width, int height)
        {
            Image image;
            byte[] buffer;
            using (var inStream = new MemoryStream())
            {
                inStream.Write(imageData, 0, imageData.Length);
                image = Image.FromStream(inStream);
                

                using (var outStream = new MemoryStream())
                {
                    EncoderParameters ep = new EncoderParameters();
                    if (image.RawFormat.Guid == ImageFormat.Jpeg.Guid)
                    {
                        image = resizeImage(image, new Size(width, height));
                        if (image.Width > 758 || image.Height > 472)
                        {
                            ImageCodecInfo iEncoder = GetEncoder(ImageFormat.Jpeg);
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)80);
                            image.Save(outStream, iEncoder, ep);
                        }
                        else
                        {
                            ImageCodecInfo iEncoder = GetEncoder(ImageFormat.Jpeg);
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)90);
                            image.Save(outStream, iEncoder, ep);
                        }
                    }
                    else if (image.RawFormat.Guid == ImageFormat.Png.Guid)
                    {
                        image = resizeImage(image, new Size(width, height));
                        if (image.Width > 758 || image.Height > 472)
                        {
                            ImageCodecInfo iEncoder = GetEncoder(ImageFormat.Jpeg);
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)80);
                            image.Save(outStream, iEncoder, ep);
                        }
                        else
                        {
                            ImageCodecInfo iEncoder = GetEncoder(ImageFormat.Png);
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)80);
                            image.Save(outStream, iEncoder, ep);
                        }

                    }else if (image.RawFormat.Guid == ImageFormat.Gif.Guid)
                    {
                        image = resizeImage(image, new Size(width, height));
                        if (image.Width > 758 || image.Height > 472)
                        {
                            ImageCodecInfo iEncoder = GetEncoder(ImageFormat.Gif);
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)80);
                            image.Save(outStream, iEncoder, ep);
                        }
                        else
                        {
                            ImageCodecInfo iEncoder = GetEncoder(ImageFormat.Gif);
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)90);
                            image.Save(outStream, iEncoder, ep);
                        }
                    }
                    buffer = outStream.ToArray();
                }
            }
            return buffer;
        }

        public static byte[] ScaleImage(byte[] imageData, int width, int height)
        {
            Image image;
            byte[] buffer = null;
            if (imageData != null)
            {
                using (var inStream = new MemoryStream())
                {
                    inStream.Write(imageData, 0, imageData.Length);
                    image = Image.FromStream(inStream);
                    //image = ResizeImage(image, new Size(width, height));

                    using (var outStream = new MemoryStream())
                    {
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        EncoderParameters ep = new EncoderParameters();
                        ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)50);

                        image.Save(outStream, jpgEncoder, ep);
                        buffer = outStream.ToArray();
                    }
                }
            }
            return buffer;
        }

        //public static string ScrambleWord(string word)
        //{
        //    var chars = new char[word.Length];
        //    var rand = new Random(10000);
        //    var index = 0;
        //    while (word.Length > 0)
        //    {
        //        // Get a random number between 0 and the length of the word. 
        //        var next = rand.Next(0, word.Length - 1); // Take the character from the random position 
        //        //and add to our char array. 
        //        chars[index] = word[next]; // Remove the character from the word. 
        //        word = word.Substring(0, next) + word.Substring(next + 1);
        //        ++index;
        //    }
        //    return new string(chars);
        //}

        public static string ScrambleWord(string input)
        {
            string[] words = input.Split(' ');
            string scrumble = String.Empty;
            foreach (var word in words)
            {
                int len = word.Length;
                int diffLen = 0;
                if (len > 2)
                {
                    diffLen = word.Length - 2;
                    scrumble += (word.Substring(0, 2)).PadRight(diffLen, '*') + " ";
                }

                scrumble += "";
            }
            return scrumble;
        }

        /// <summary>
        /// Gets the enum listed dropdown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SelectList DropDown<T>()
        {
            var list = new SortedList<int, string>();
            var Values = Enum.GetValues(typeof(T));
            foreach (var Value in Values)
            {
                int val = System.Convert.ToInt32(Value);

                string display = ((Enum)Value).GetDescription();
                list.Add(val, display);
            }
            return new SelectList(list, "Key", "Value");
        }

        /// <summary>
        /// Gets the enum listed dropdown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static SelectList DropDown<T>(List<int> includes)
        {
            var list = new SortedList<int, string>();
            var Values = Enum.GetValues(typeof(T));
            foreach (var Value in Values)
            {
                int val = System.Convert.ToInt32(Value);
                if (includes.Contains(val))
                {
                    string display = ((Enum)Value).GetDescription();
                    list.Add(val, display);
                }
            }
            return new SelectList(list, "Key", "Value");
        }

        public static SelectList DropDown<T>(List<int> includes, int selected)
        {
            var list = new SortedList<int, string>();
            var Values = Enum.GetValues(typeof(T));
            foreach (var Value in Values)
            {
                int val = System.Convert.ToInt32(Value);
                if (includes.Contains(val))
                {
                    string display = ((Enum)Value).GetDescription();
                    list.Add(val, display);
                }
            }
            return new SelectList(list, "Key", "Value", selected);
        }

        /// <summary>
        /// Gets the enum listed dropdown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultText"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static SelectList DropDown<T>(string defaultText, List<int> includes)
        {
            var list = new SortedList<int, string>();
            var Values = Enum.GetValues(typeof(T));
            list.Add(-1, defaultText);
            foreach (var Value in Values)
            {
                int val = System.Convert.ToInt32(Value);
                if (includes.Contains(val))
                {
                    string display = ((Enum)Value).GetDescription();
                    list.Add(val, display);
                }
            }
            return new SelectList(list, "Key", "Value");
        }

        /// <summary>
        /// Gets the enum listed dropdown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultText"></param>
        /// <param name="includes"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static SelectList DropDown<T>(string defaultText, List<int> includes, int selected)
        {
            var list = new SortedList<int, string>();
            var Values = Enum.GetValues(typeof(T));
            list.Add(-1, defaultText);
            foreach (var Value in Values)
            {
                int val = System.Convert.ToInt32(Value);
                if (includes.Contains(val))
                {
                    string display = ((Enum)Value).GetDescription();
                    list.Add(val, display);
                }
            }
            return new SelectList(list, "Key", "Value", selected);
        }

        public static string TitleCase(this string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        public static string Convert(string html)
        {
            var oHtmlToText = new HtmlToText();
            return oHtmlToText.Convert(html);
        }

        public static string GetUrl(HttpRequestBase request)
        {
            var url = string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority);
            return url;
        }

        /// <summary>
        ///     Gets the <see cref="DescriptionAttribute" /> of an <see cref="Enum" />
        ///     type value.
        /// </summary>
        /// <param name="value">The <see cref="Enum" /> type value.</param>
        /// <returns>
        ///     A string containing the text of the
        ///     <see cref="DescriptionAttribute" />.
        /// </returns>
        public static string GetDescription(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var description = value.ToString();
            var fieldInfo = value.GetType().GetField(description);
            var attributes =
                (DescriptionAttribute[])
                    fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }

        //
        public static string RemoveEmails(this string scrape)
        {
            Regex reg = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);
            Match match;
            string filterString = scrape;
            List<string> results = new List<string>();
            for (match = reg.Match(scrape); match.Success; match = match.NextMatch())
            {
                if (!(results.Contains(match.Value)))
                    filterString = filterString.Replace(match.Value, "");
            }

            return filterString;
        }

        public static string RemoveNumbers(this string scrape)
        {
            Regex reg = new Regex(@"\d{5,}", RegexOptions.IgnoreCase);
            Match match;
            string filterString = scrape;
            List<string> results = new List<string>();
            for (match = reg.Match(scrape); match.Success; match = match.NextMatch())
            {
                if (!(results.Contains(match.Value)))
                    filterString = filterString.Replace(match.Value, "");
            }

            return filterString;
        }

        public static string RemoveWebsites(this string scrape)
        {
            Regex reg = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase);
            Match match;
            string filterString = scrape;
            List<string> results = new List<string>();
            for (match = reg.Match(scrape); match.Success; match = match.NextMatch())
            {
                if (!(results.Contains(match.Value)))
                    filterString = filterString.Replace(match.Value, "");
            }

            return filterString;
        }

        public static string RemoveSpecialCharacters(this string scrape)
        {
            Regex reg = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);
            Match match;
            string filterString = scrape;
            List<string> results = new List<string>();
            for (match = reg.Match(scrape); match.Success; match = match.NextMatch())
            {
                if (!(results.Contains(match.Value)))
                    filterString = filterString.Replace(match.Value, "");
            }

            return filterString;
        }
       
        public static string Get6DigitCode()
        {
            string _numbers = "0123456789";
            Random random = new Random();
            StringBuilder builder = new StringBuilder(6);
            string numberAsString = string.Empty;

            for (var i = 0; i < 6; i++)
            {
                builder.Append(_numbers[random.Next(0, _numbers.Length)]);
            }

            numberAsString = builder.ToString();            
            return numberAsString;
        }

        public static string GetToken()
        {
            string _numbers = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();
            int len = 20;
            StringBuilder builder = new StringBuilder(len);
            string numberAsString = string.Empty;

            for (var i = 0; i < len; i++)
            {
                builder.Append(_numbers[random.Next(0, _numbers.Length)]);
            }

            numberAsString = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(builder.ToString()));
            return numberAsString;
        }
    }
}