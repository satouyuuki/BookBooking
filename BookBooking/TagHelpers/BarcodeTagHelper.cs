using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using BookBooking.Models;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using ZXing.Common;

namespace BookBooking.TagHelpers
{
    public class BarcodeTagHelper : TagHelper
    {
        public string BarcodeId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Alt { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            try
            {
                var margin = 0;
                var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<SixLabors.ImageSharp.PixelFormats.La32>
                {
                    Format = ZXing.BarcodeFormat.PDF_417,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Height = this.Height > 80 ? this.Height : 80,
                        Width = this.Width > 400 ? this.Width : 400,
                        Margin = margin
                    }
                };

                var image = barcodeWriter.Write(BarcodeId);
                output.TagName = "img";
                output.Attributes.Clear();
                output.Attributes.Add("width", Width);
                output.Attributes.Add("height", Height);
                output.Attributes.Add("alt", Alt);
                output.Attributes.Add("src", $"{image.ToBase64String(PngFormat.Instance)}");
            }
            catch(Exception ex)
            {

            }
        }
    }
}

