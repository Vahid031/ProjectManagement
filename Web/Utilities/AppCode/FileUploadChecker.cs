using DatabaseContext.Context;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Services.EfServices.General;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.Other;

namespace Web.Utilities.AppCode
{

    public static class CheckUploadFile
    {
        
        #region prop

        public static string[] ValidPicExt { get { return new string[] { "image/jpg", "image/jpeg", "image/bmp", "image/pjpeg" }; } }

        public static string[] ValidAnimatePicExt { get { return new string[] { "image/gif", "image/jpeg", "image/bmp", "image/png", "image/pjpeg", "image/x-icon" }; } }

        public static string[] ValidFlashExt { get { return new string[] { "application/x-shockwave-flash" }; } }

        public static string[] ValidDocsExt { get { return new string[] { "application/pdf" }; } }

        public static string[] ValidZipExt { get { return new string[] { "application/x-zip-compressed", "application/x-rar-compressed", "application/zip", "application/rar" }; } } // need to add in host

        public static string[] ValidVideoExt { get { return new string[] { "video/x-flv" }; } } /// need to add in host

        #endregion

        #region methods

        public static bool IsValidPic(string fileType)
        {
            try
            {
                return ValidPicExt.Contains(fileType);
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public static bool IsValidAnimatePic(string fileType)
        {
            try
            {
                return ValidAnimatePicExt.Contains(fileType);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsValidFlash(string fileType)
        {
            try
            {
                return ValidFlashExt.Contains(fileType);
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public static bool IsValidDoc(string fileType)
        {
            try
            {
                return ValidDocsExt.Contains(fileType);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsValidZip(string fileType)
        {
            try
            {
                return ValidZipExt.Contains(fileType);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsValidVideo(string fileType)
        {
            try
            {
                return ValidVideoExt.Contains(fileType);
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool IsValidLen(int value)
        {
            int maxLen = 2097151;

            if (value <= maxLen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Upload

        public static KeyValuePair<Int64, string> Upload(HttpPostedFileBase myFile, List<UploadFilesResultViewModel> statuses, Int64 memberId, string pathForSaving, string folder, string deleteUrl, Int64 fileTypeId)
        {

            //if (myFile != null && !IsValidLen(myFile.ContentLength))
            //{
            //    return new KeyValuePair<long, string>(1, "حجم عکس بیشتر از حد مورد نظر است");
            //}
            //else if (myFile != null && (!IsValidPic(myFile.ContentType)))
            //{
            //    return new KeyValuePair<long, string>(2, "پسوند فایل اشتباه است");
            //}
            //else
            {
                if (myFile != null && myFile.ContentLength != 0)
                {
                    IUnitOfWork _uow = DependencyResolver.StructureMapObjectFactory.Container.GetInstance<IUnitOfWork>();
                    IFileService _fileService = DependencyResolver.StructureMapObjectFactory.Container.GetInstance<IFileService>();

                    
                    string relativePath = "/Uploads/" + folder;
                    string fileTitle = myFile.FileName;
                    string saveFileName = string.Format("{0}-{1:d4}{2:d2}{3:d2}{4:d2}{5:d2}{6:d2}{7:d4}{8}", memberId, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond, fileTitle.Substring(fileTitle.LastIndexOf('.')));

                    if (CreateFolderIfNeeded(pathForSaving))
                    {
                        try
                        {
                            DomainModels.Entities.General.File file = new DomainModels.Entities.General.File();
                            file.Id = -1;
                            file.Address = relativePath + "/" + saveFileName;
                            file.DateTime = ConvertDate.GetShamsiTime(DateTime.Now);
                            file.FileTypeId = fileTypeId;
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Temp);
                            file.Type = myFile.ContentType;
                            file.Size = myFile.ContentLength.ToString();
                            file.MemberId = memberId;
                            file.Title = myFile.FileName;

                            myFile.SaveAs(pathForSaving + "/" + saveFileName);

                            _fileService.Insert(file);

                            if (_uow.SaveChanges(memberId) > 0)
                            {
                                string thumbnail_url = "";
                                if (myFile.ContentType.Contains("image"))
                                {
                                    thumbnail_url = file.Address;
                                }
                                else
                                {
                                    thumbnail_url = "/Uploads/noimage.png";
                                }

                                statuses.Add(new UploadFilesResultViewModel()
                                {
                                    name = file.Title,
                                    size = myFile.ContentLength,
                                    type = myFile.ContentType,
                                    url = file.Address,
                                    delete_url = deleteUrl + "/" + file.Id,
                                    thumbnail_url = thumbnail_url,
                                    delete_type = "POST",
                                    imageId = file.Id
                                });

                                return new KeyValuePair<long, string>(3, "عملیات با موفقیت انجام شد");
                            }
                        }
                        catch (Exception ex)
                        {
                            return new KeyValuePair<long, string>(4, "خطا در آپلود تصویر");
                        }
                    }
                }
            }

            return new KeyValuePair<long, string>(4, "خطا در آپلود تصویر");
        }

        private static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }

        public static void DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        #endregion

    }

}