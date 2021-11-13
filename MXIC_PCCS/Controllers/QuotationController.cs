using MXIC_PCCS.DataUnity.BusinessUnity;
using MXIC_PCCS.DataUnity.Interface;
using MXIC_PCCS.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MXIC_PCCS.Controllers
{
    [Authorize]
    public class QuotationController : Controller
    {
        IQuotation _IQuotation = new DataUnity.BusinessUnity.Quotation();
        StringBuilder SB = new StringBuilder();

        // GET: Quotation
        public ActionResult Index()
        {
            var id = HttpContext.User.Identity.Name;
            ViewBag.ID = id;
            return View();
        }

        [HttpPost]
        public ActionResult UploadQuotation(HttpPostedFileBase file)
        {
            try
            {
                //EPPLUS 授權 (不可註解刪除)
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                //檢查匯入的EXCEL檔
                if (file != null && file.ContentLength > 0)
                {
                    //取得Sheet
                    using (var ExcelPkg = new ExcelPackage(file.InputStream))
                    {
                        ExcelWorksheet PoSheet = ExcelPkg.Workbook.Worksheets["PO報價"];

                        int POendRowIndex = PoSheet.Dimension.End.Row;
                        int POendColumn = PoSheet.Dimension.End.Column;

                        string PoNo = "", VendorName = "";

                        //PO
                        if (PoSheet.Cells[7, 7].Text.Contains("PO NO.") && !string.IsNullOrWhiteSpace(PoSheet.Cells[7, 9].Text))
                        {
                            PoNo = PoSheet.Cells[7, 9].Text;
                        }

                        //VedorName
                        if (PoSheet.Cells[5, 7].Text.Contains("供應商Vendor") && !string.IsNullOrWhiteSpace(PoSheet.Cells[5, 9].Text))
                        {
                            VendorName = PoSheet.Cells[5, 9].Text;
                        }

                        //清除現有PO資料
                        var MessageStr = _IQuotation.ClearTable(PoNo);
                        if (!MessageStr.Contains("判讀結束!"))
                        {
                            SB.Clear();
                            SB.AppendFormat("<script>alert('判讀資料發生錯誤!');window.location.href='../Quotation/Index';</script>");
                            return Content(SB.ToString());
                        }

                        //PO報價上傳儲存資料庫
                        List<QuotationProperty> Property_ListModel = new List<QuotationProperty>();

                        for (int i = 1; i <= POendRowIndex; i++)
                        {
                            for (int j = 1; j <= POendColumn; j++)
                            {
                                if (PoSheet.Cells[i, j].Text.Contains("10-") || PoSheet.Cells[i, j].Text.Contains("20-"))
                                {
                                    QuotationProperty Property_Model = new QuotationProperty();
                                    Property_Model.PoClassID = PoSheet.Cells[i, j].Text;
                                    Property_Model.PoClassName = PoSheet.Cells[i, j + 1].Text;
                                    Property_Model.Unit = PoSheet.Cells[i, j + 2].Text;
                                    Property_Model.Amount = PoSheet.Cells[i, j + 3].Text;
                                    Property_Model.ExcelPosition = i.ToString() + "," + (j + 3).ToString();
                                    Property_ListModel.Add(Property_Model);
                                }
                            }
                        }

                        _IQuotation.ImportQuotation(VendorName, PoNo, Property_ListModel);

                        file.SaveAs(ConfigurationManager.AppSettings["ExampleDirectory"] + PoNo + ".xlsx");
                    }
                }
                else
                {
                    SB.Clear();
                    SB.AppendFormat("<script>alert('請先選擇匯入檔案!');window.location.href='../Quotation/Index';</script>");
                    return Content(SB.ToString());
                }
            }
            catch (Exception ex)
            {
                SB.Clear();
                SB.AppendFormat("<script>alert('匯入檔案時發生錯誤!');window.location.href='../Quotation/Index';</script>");
                return Content(SB.ToString());
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public string CheckQuotationPo(HttpPostedFileBase file)
        {
            string Result = _IQuotation.ClearTableCheck(file);
            //1表示Po已存在
            return Result;
        }
        public string SearchQuotation(string VendorName, string PoNo, string PoClassID)
        {
            //呼叫IQuotation介面中的QuotationList方法
            string jsonStr = _IQuotation.SearchQuotation(VendorName, PoNo, PoClassID);

            return jsonStr;
        }

        public ActionResult DownloadQuotationExample()
        {
            try
            {
                string filepath = Server.MapPath("~/Content/報價單範本.xlsx");
                string filename = Path.GetFileName(filepath);
                Stream iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return File(iStream, "application/xlsx", filename);
            }
            catch (Exception ex)
            {
                SB.Clear();
                SB.AppendFormat("<script>alert('下載失敗!');window.location.href='../Quotation/Index';</script>");
                return Content(SB.ToString());
            }
        }

        public string DelQuotation(string PoNo)
        {
            //呼叫IQuotation介面中的QuotationList方法
            string jsonStr = _IQuotation.DelQuotation(PoNo);

            return jsonStr;

        }
    }
}