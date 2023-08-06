using AbsolCase.Configurations;
using AbsolCase.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class StampPDFController : Controller
    {
        private readonly IWebHostEnvironment _he;

        public StampPDFController(IWebHostEnvironment he)
        {
            this._he = he;
        }


        private void AddTextToPdf(string inputPdfFile, string outputPdfFile, string textToAdd)
        {
            using (var existingFileStream = new FileStream(inputPdfFile, FileMode.Open, FileAccess.Read))
            using (var newFileStream = new FileStream(outputPdfFile, FileMode.Create, FileAccess.Write))
            {
                var reader = new PdfReader(existingFileStream);
                var stamper = new PdfStamper(reader, newFileStream);

                //string fontPath = "~/fonts/Damion/Damion-Regular.ttf"; // Adjust the path as per your file location.
                //BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var baseFont = BaseFont.CreateFont(BaseFont.TIMES_BOLDITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                var pageCount = reader.NumberOfPages;

                for (int i = 1; i <= pageCount; i++)
                {
                    var contentByte = stamper.GetOverContent(i);

                    var ct = new ColumnText(contentByte);
                    var phrase = new Phrase(textToAdd, new Font(baseFont, 14)); // Set the font size (e.g., 12)

                    var pageSize = reader.GetPageSizeWithRotation(i);

                    // Define the position, width, and height for the text box
                    var rect = new Rectangle((int)(pageSize.Width - 120), 20, (int)(pageSize.Width - 26), 50);

                    ct.SetSimpleColumn(rect);
                    ct.AddElement(phrase);
                    ct.Alignment = Element.ALIGN_RIGHT;
                    ct.Go();
                }

                stamper.Close();
                reader.Close();
            }
        }

        public void AddImageToPdf(string pdfFilePath, string imagePath)
        {
            // Create a new PDF reader based on the existing document
            PdfReader pdfReader = new PdfReader(pdfFilePath);

            // Create a new PDF stamper based on the reader and output stream
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream($"{_he.WebRootPath}\\files\\modified.pdf", FileMode.Create));

            // Get the first page of the PDF document
            PdfContentByte pdfContentByte = pdfStamper.GetOverContent(1);

            // Load the image
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);

            // Calculate the desired width and height for the image
            float desiredWidth = image.Width * 0.3f;
            float desiredHeight = image.Height * 0.3f;
            image.ScaleToFit(desiredWidth, desiredHeight);

            // Set the position of the image to be at the bottom right corner
            float x = pdfContentByte.PdfDocument.PageSize.Width - desiredWidth - 20; // You can adjust the horizontal position as needed
            float y = 20; // You can adjust the vertical position as needed
            image.SetAbsolutePosition(x, y);

            // Add the image to the PDF document
            pdfContentByte.AddImage(image);

            // Close the stamper and reader
            pdfStamper.Close();
            pdfReader.Close();

            // Delete the original file and rename the new file
            //File.Delete(pdfFilePath);
            //File.Move(pdfFilePath + ".new", pdfFilePath);
        }

        private void StampText(int Id, string Text)
        {
            SResponse resp = Fetch.GotoService("api", $"Documents/GetDocumentsById?Id={Id}", "Get");
            SResponse resp2 = Fetch.GotoService("api", $"Documents/GetRootPath", "Get");
            Decuments doc = JsonConvert.DeserializeObject<Decuments>(resp.Resp);
            string host = resp2.Resp;
            string path = doc.DecumentPath;
            path = path.Replace('/', '\\');
            string fileName = $"{host}/{path}";

            //using (FileStream fileStream = System.IO.File.Create(fileName))
            //{
            //    file.CopyTo(fileStream);
            //    fileStream.Flush();
            //}
            // Add text to the PDF file

            string outputFileName = $"{_he.WebRootPath}\\files\\_modified.pdf";
            AddTextToPdf(fileName, outputFileName, Text);
            SResponse resp3 = Fetch.GotoService("api", $"Documents/DeleteDocumentAtPath?Id={Id}", "Delete");
            byte[] fileBytes = System.IO.File.ReadAllBytes(outputFileName);
            doc.File = fileBytes;
            doc.extention = "pdf";
            var body = JsonConvert.SerializeObject(doc);
            SResponse resp4 = Fetch.GotoService("api", $"Documents/ReplaceDocument", "Put", body);

            // Optionally, you may want to delete the original file from the server
            // Uncomment the following line if you want to delete the original file
            System.IO.File.Delete(outputFileName);

            //return File(fileBytes, "application/pdf", "modified_pdf.pdf");
            // Return the modified PDF as a download
            //return View();
        }
        public IActionResult Index(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public IActionResult Index(int Id, string TextToAdd)
        {
            StampText(Id, TextToAdd);
            return View();
        }

        public IActionResult Image(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public IActionResult Image(int Id, IFormFile  Image)
        {
            //string pdfFilePath = $"{he.WebRootPath}\\files\\{file.FileName}";
            //using (FileStream fileStream = System.IO.File.Create(pdfFilePath))
            //{
            //    file.CopyTo(fileStream);
            //    fileStream.Flush();
            //}

            SResponse resp = Fetch.GotoService("api", $"Documents/GetDocumentsById?Id={Id}", "Get");
            SResponse resp2 = Fetch.GotoService("api", $"Documents/GetRootPath", "Get");
            Decuments doc = JsonConvert.DeserializeObject<Decuments>(resp.Resp);
            string host = resp2.Resp;
            string path = doc.DecumentPath;
            path = path.Replace('/', '\\');
            string fileName = $"{host}/{path}";

            string pngFilePath = $"{_he.WebRootPath}\\images\\{Image.FileName}";
            using (FileStream fileStream = System.IO.File.Create(pngFilePath))
            {
                Image.CopyTo(fileStream);
                fileStream.Flush();
            }

            // Add the image to the PDF file
            AddImageToPdf(fileName, pngFilePath);
            //System.IO.File.Delete(pdfFilePath);

            // After processing, you can redirect to another action or return a view with a success message
            //TempData["response"] = "Form submitted successfully.";

            // Return the updated PDF as a download
            SResponse resp3 = Fetch.GotoService("api", $"Documents/DeleteDocumentAtPath?Id={Id}", "Delete");
            string newfilepath =  $"{_he.WebRootPath}/modified.pdf";
            byte[] fileBytes = System.IO.File.ReadAllBytes(newfilepath);
            doc.File = fileBytes;
            doc.extention = "pdf";
            var body = JsonConvert.SerializeObject(doc);
            SResponse resp4 = Fetch.GotoService("api", $"Documents/ReplaceDocument", "Put", body);

            // Optionally, you may want to delete the original file from the server
            // Uncomment the following line if you want to delete the original file
            System.IO.File.Delete(newfilepath);
            System.IO.File.Delete(pngFilePath);
            return View();
        }

        public IActionResult Draw(int Id) 
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public IActionResult Draw(int Id, string DrawingData)
        {
            //string pdfFilePath = $"{he.WebRootPath}\\files\\{file.FileName}";
            //using (FileStream fileStream = System.IO.File.Create(pdfFilePath))
            //{
            //    file.CopyTo(fileStream);
            //    fileStream.Flush();
            //}
            SResponse resp = Fetch.GotoService("api", $"Documents/GetDocumentsById?Id={Id}", "Get");
            SResponse resp2 = Fetch.GotoService("api", $"Documents/GetRootPath", "Get");
            Decuments doc = JsonConvert.DeserializeObject<Decuments>(resp.Resp);
            string host = resp2.Resp;
            string path = doc.DecumentPath;
            path = path.Replace('/', '\\');
            string fileName = $"{host}/{path}";

            // Process the drawing data and add it to the PDF
            if (!string.IsNullOrEmpty(DrawingData))
            {
                // Remove the "data:image/png;base64," prefix from the drawing data
                var base64Data = DrawingData.Split(',')[1];

                // Convert the Base64-encoded data to a byte array
                byte[] imageBytes = Convert.FromBase64String(base64Data);

                // Create a unique filename for the image (you may use GUID or a timestamp)
                var imageFileName = "drawing_" + Guid.NewGuid().ToString() + ".png";

                // Save the image file to the server's file system (you can customize the path)
                var imagePath = Path.Combine(_he.WebRootPath, "images", imageFileName);
                System.IO.File.WriteAllBytes(imagePath, imageBytes);

                // Add the image to the PDF file
                AddImageToPdf(fileName, imagePath);

                SResponse resp3 = Fetch.GotoService("api", $"Documents/DeleteDocumentAtPath?Id={Id}", "Delete");
                string newfilepath = $"{_he.WebRootPath}/modified.pdf";
                byte[] fileBytes = System.IO.File.ReadAllBytes(newfilepath);
                doc.File = fileBytes;
                doc.extention = "pdf";
                var body = JsonConvert.SerializeObject(doc);
                SResponse resp4 = Fetch.GotoService("api", $"Documents/ReplaceDocument", "Put", body);
                System.IO.File.Delete(newfilepath);
                System.IO.File.Delete(imagePath);
            }

            // Process the rest of the form data and perform any required actions

            // After processing, you can redirect to another action or return a view with a success message
            //TempData["response"] = "Form submitted successfully.";

            // Return the updated PDF as a download
            return View();
        }

        public IActionResult Initials(int Id) {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public IActionResult Initials(int Id, string TextToAdd)
        {
            string[] words = TextToAdd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string newinitials = string.Join("", words.Select(w => w[0]));
            StampText(Id, newinitials);
            return View();
        }

    }
}
