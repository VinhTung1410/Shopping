using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Shopping.Model1
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
                // Create a new stream and copy the file content to it
                using (var memoryStream = new MemoryStream())
                {
                    file.InputStream.CopyTo(memoryStream);
                    memoryStream.Position = 0;

                    using (var img = Image.FromStream(memoryStream))
                    {
                        // Verify it's a valid image
                        if (!IsValidImage(img))
                        {
                            result.IsValid = false;
                            result.ErrorMessage = "Invalid image format.";
                            return result;
                        }

                        // Check image dimensions
                        if (img.Width > ProductImage.MaxImageWidth || img.Height > ProductImage.MaxImageHeight)
                        {
                            result.IsValid = false;
                            result.ErrorMessage = $"Image dimensions cannot exceed {ProductImage.MaxImageWidth}x{ProductImage.MaxImageHeight} pixels.";
                            return result;
                        }

                        result.IsValid = true;
                        result.ImageWidth = img.Width;
                        result.ImageHeight = img.Height;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = "Error processing image: " + ex.Message;
            }

            return result;
        }

        private static bool IsValidImage(Image img)
        {
            try
            {
                // Try to get the image format - will throw if not a valid image
                ImageFormat format = img.RawFormat;
                return true;
            }
            catch
            {
                return false;
            }
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

            // Save the image using proper stream handling
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                using (var img = Image.FromStream(memoryStream))
                {
                    // Save with JPG format explicitly
                    img.Save(imagePath, ImageFormat.Jpeg);
                }
            }

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