using Microsoft.AspNetCore.Http;
using Services.Result;

namespace Services.FileSystemServices
{
    public class FileServices : IFileServices
    {
        public async Task<FileSystemResult> AddImageAsync(string path, IFormFile file, bool isMain = false)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return new FileSystemResult { Succesd = false, Msg = "No file provided." };

                // تأكد من أن المسار موجود
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                var fileName = string.Empty;
                if (isMain)
                {
                    fileName = "main_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                }
                else
                {
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                }
                var filePath = Path.Combine(fullPath, fileName);
                // حفظ الملف
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new FileSystemResult { Succesd = true, Msg = fileName };
            }
            catch (Exception ex)
            {
                return new FileSystemResult { Succesd = false, Msg = $"Error: {ex.Message}" };
            }
        }

        public async Task<FileSystemResult> AddRangeImageAsync(string path, List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    return new FileSystemResult { Succesd = false, Msg = "No files provided." };

                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);

                var savedFiles = new List<string>();

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        // تحديد اسم الملف الجديد
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(fullPath, fileName);

                        // حفظ الملف
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        savedFiles.Add(fileName);
                    }
                }

                return new FileSystemResult { Succesd = true, Data = savedFiles };
            }
            catch (Exception ex)
            {
                return new FileSystemResult { Succesd = false, Msg = $"Error: {ex.Message}" };
            }
        }

        public async Task<FileSystemResult> AddRangeImageProductAsync(string path, IFormFile Main, List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    return new FileSystemResult { Succesd = false, Msg = "No files provided." };

                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);

                var savedFiles = new List<string>();
                string mainImageName = null;

                // حفظ الصورة الأساسية
                if (Main != null && Main.Length > 0)
                {
                    mainImageName = "main_" + Guid.NewGuid().ToString() + Path.GetExtension(Main.FileName);
                    var mainImagePath = Path.Combine(fullPath, mainImageName);

                    using (var stream = new FileStream(mainImagePath, FileMode.Create))
                    {
                        await Main.CopyToAsync(stream);
                    }
                }

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        // تحديد اسم الملف الجديد
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(fullPath, fileName);

                        // حفظ الملف
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        savedFiles.Add(fileName);
                    }
                }

                savedFiles.Insert(0, mainImageName);

                return new FileSystemResult { Succesd = true, Data = savedFiles };
            }
            catch (Exception ex)
            {
                return new FileSystemResult { Succesd = false, Msg = $"Error: {ex.Message}" };
            }
        }

        public async Task<FileSystemResult> DeleteImageAsync(string path, string? Folder = null)
        {
            try
            {
                // لو الـ paths فارغة، إرجع رسالة فشل
                if (path == null)
                {
                    return new FileSystemResult { Succesd = false, Msg = "No images provided to delete." };
                }

                // تحديد المسار الكامل للصورة
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Folder, path);

                // التحقق إذا كانت الصورة موجودة
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);  // حذف الصورة
                }
                else
                {
                    return new FileSystemResult { Succesd = false, Msg = $"Image at path {fullPath} not found." };
                }

                return new FileSystemResult { Succesd = true, Msg = "Images deleted successfully." };
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء، رجع رسالة الخطأ
                return new FileSystemResult { Succesd = false, Msg = $"Error: {ex.Message}" };
            }
        }

        public async Task<FileSystemResult> DeleteRangeImageAsync(List<string> paths)
        {
            try
            {
                // لو الـ paths فارغة، إرجع رسالة فشل
                if (paths == null || !paths.Any())
                {
                    return new FileSystemResult { Succesd = false, Msg = "No images provided to delete." };
                }

                foreach (var path in paths)
                {
                    // تحديد المسار الكامل للصورة
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", path);

                    // التحقق إذا كانت الصورة موجودة
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);  // حذف الصورة
                    }
                    else
                    {
                        return new FileSystemResult { Succesd = false, Msg = $"Image at path {path} not found." };
                    }
                }

                return new FileSystemResult { Succesd = true, Msg = "Images deleted successfully." };
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء، رجع رسالة الخطأ
                return new FileSystemResult { Succesd = false, Msg = $"Error: {ex.Message}" };
            }
        }

        public async Task<FileSystemResult> UpdateImage(string OldImage, string path, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return new FileSystemResult { Succesd = false, Msg = "No file provided." };

                // تأكد من وجود المسار
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
                if (!Directory.Exists(fullPath))
                    return new FileSystemResult { Succesd = false, Msg = "Directory does not exist." };

                // حذف الصورة القديمة
                var oldFilePath = Path.Combine(fullPath, OldImage);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                // حفظ الصورة الجديدة
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var newFilePath = Path.Combine(fullPath, fileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new FileSystemResult { Succesd = true, Msg = fileName };
            }
            catch (Exception ex)
            {
                return new FileSystemResult { Succesd = false, Msg = $"Error: {ex.Message}" };
            }
        }
    }
}