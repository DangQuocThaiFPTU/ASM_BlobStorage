using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ImagesStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("Upload");
        }
        public ActionResult Upload()
        {
            CloudBlobContainer blobContainer = BlobStorageService.GetCloudBlobContainer();
            List<string> blods = new List<string>();
            blobContainer.ListBlobs().ToList().ForEach(s => blods.Add(s.Uri.ToString()));
            return View(blods);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase imageUpload)
        {
            if (imageUpload != null)
            {
                if (imageUpload.ContentLength > 0)
                {
                    CloudBlobContainer blobContainer = BlobStorageService.GetCloudBlobContainer();
                    CloudBlockBlob blod = blobContainer.GetBlockBlobReference(imageUpload.FileName);
                    blod.UploadFromStream(imageUpload.InputStream);
                }
            }
            else
            {
                TempData["Msg"] = "No file is selected.";
            }
            return RedirectToAction("Upload");
        }

        public ActionResult DeleteImage(string imageName)
        {
            Uri uri = new Uri(imageName);
            string fileName = Path.GetFileName(uri.LocalPath);
            CloudBlobContainer blobContainer = BlobStorageService.GetCloudBlobContainer();
            CloudBlockBlob blod = blobContainer.GetBlockBlobReference(fileName);
            blod.Delete();
            TempData["Msg"] = "File : " + fileName + "deleted.";
            return RedirectToAction("Upload");
        }
    }
}