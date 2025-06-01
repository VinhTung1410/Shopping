using Shopping.Model1;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Shopping.Controller1
{
    public class ImageHelper
    {
        public static ValidationResult ValidateImage(HttpPostedFile file)
        {
            var result = new ValidationResult();

            // Check if file exists
            if (file == null || file.ContentLength == 0)
            {
                result.IsValid = false;
                result.ErrorMessage = "No file was uploaded.";
                return result;
            }

            // Check file size
            if (file.ContentLength > ProductImage.MaxFileSizeBytes)
            {
                result.IsValid = false;
                result.ErrorMessage = $"File size exceeds maximum limit of {ProductImage.MaxFileSizeBytes / (1024 * 1024)}MB.";
                return result;
            }

            // Check file extension
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ProductImage.AllowedExtension)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Only {ProductImage.AllowedExtension} files are allowed.";
                return result;
            }

            try
            {
                using (var image = Image.FromStream(file.InputStream))
                {
                    // Check image dimensions
                    if (image.Width > ProductImage.MaxImageWidth || image.Height > ProductImage.MaxImageHeight)
                    {
                        result.IsValid = false;
                        result.ErrorMessage = $"Image dimensions cannot exceed {ProductImage.MaxImageWidth}x{ProductImage.MaxImageHeight} pixels.";
                        return result;
                    }

                    result.IsValid = true;
                    result.ImageWidth = image.Width;
                    result.ImageHeight = image.Height;
                }
            }
            catch (Exception)
            {
                result.IsValid = false;
                result.ErrorMessage = "Invalid image file.";
            }

            return result;
        }

        public static string SaveImage(HttpPostedFile file, string productId)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"{productId}_{timestamp}{Path.GetExtension(file.FileName)}";
            var imagePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Image"), fileName);

            // Ensure directory exists
            var directory = Path.GetDirectoryName(imagePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            file.SaveAs(imagePath);
            return fileName;
        }
    }
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
    }
}