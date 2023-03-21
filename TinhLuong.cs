using AjaxPro;
using ADMIN.Core.Call.Utility;
using ADMIN.Core.Model;
using ADMIN.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ADMIN.WebParts
{
    public class TinhLuong : WebPartTemplate
    {
        public override string WebPartId
        {
            get
            {
                return "TinhLuong";
            }
        }

        public override string WebPartTitle
        {
            get
            {
                return "Côn cụ tính lương";
            }
        }

        public override string Description
        {
            get
            {
                return "Công cụ tính lương";
            }
        }

        public override void RegAjaxPro(SiteParam OSiteParam, Page Page)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(TinhLuong), Page);
        }

        public override AjaxOut CheckPermission(RenderInfoCls ORenderInfo)
        {
            SiteParam ositeParam = WebEnvironments.CreateSiteParam(ORenderInfo);
            string ownerUserId = WebSessionUtility.GetCurrentLoginUser(ositeParam).OwnerUserId;
            bool retBoolean = true;
            return new AjaxOut
            {
                RetBoolean = retBoolean
            };
        }

        public override AjaxOut Draw(SiteParam OSiteParam, RenderInfoCls ORenderInfo)
        {
            AjaxOut ajaxOut = new AjaxOut();
            try
            {
                List<BangChuyenDoiCls> OChuyenDois = new List<BangChuyenDoiCls>();
                WebSessionUtility.SetSession(OSiteParam, "BangChuyenDois", OChuyenDois);
                ajaxOut.HtmlContent = WebEnvironments.ProcessHtml(string.Concat(new string[]
                {
                    "<div id=\"divListForm\">\r\n   " +
                    "   <div class=\"ibox float-e-margins\"> \r\n       " +
                    "       <div class=\"ibox-title\"> \r\n           " +
                    "           <h5>", WebLanguage.GetLanguage(ORenderInfo, "Công cụ tính lương"),"</h5> \r\n       " +
                    "       </div> \r\n       " +
                    "       <div class=\"ibox-content\"> \r\n          " +
                    "           <div class=\"row\">\r\n                    " +
                    "              <div class =\"col-sm-8\">" +
                    "                  <label style=\"font-weight:bold;margin-bottom:5px\">Thu Nhập </label>\r\n                 " +
                    "                  <input   id=\"txtThuNhap\" type=\"text\" placeholder=\"VD: 10,000,000\" class=\"input-sm form-control\" onkeypress='CheckCurrency(event);'>\r\n "+
                    "               </div>\r\n                 " +
                    "              <div class =\"col-sm-12\">" +
                    "                  <label style=\"font-weight:bold;margin-bottom:5px\">Mức lương đóng bảo hiểm </label><br>\r\n                 " +
                    "                  <div class =\"col-sm-2\">" +
                    "                      <label class=\"radio-inline\"><input id = \"rdbsChinhThuc\" type = \"radio\" name=\"optradioHl1\" value= \"1\" checked>Trên lương chính thức</label> " +
                    "                  </div>\r\n    "+
                    "                  <div class =\"col-sm-6\">" +
                    "                      <div class =\"col-sm-2\">" +
                    "                           <label class=\"radio-inline\"><input id = \"rdbsKhac\" type = \"radio\" name=\"optradioHl1\" value=\"2\">Khác</label> " +
                    "                      </div>\r\n    "+
                    "                      <div class =\"col-sm-4\">" +
                    "                           <input type=\"text\" id=\"txtThuKhac\" name=\"luong\" value=\"\" placeholder=\"VNĐ\" class=\"border-hover\" disabled > "+
                    "                      </div>\r\n    "+
                    "                  </div>\r\n    "+
                    "              </div>\r\n    "+
                    "              <div class =\"col-sm-8\">" +
                    "                  <label style=\"font-weight:bold;margin-bottom:5px\">Khu vực </label>\r\n                 " +
                    "                  <select id=\"cboKhuVuc\" placeholder=\"Khu vực\" class=\"form-control\">" +
                    "                       <option value = '4680000'>Vùng 1: 4.680.000 đồng/tháng</option>" +
                    "                       <option value = '4160000'>Vùng 2: 4.160.000 đồng/tháng</option>" +
                    "                       <option value = '3640000'>Vùng 3: 3.640.000 đồng/tháng</option>" +
                    "                       <option value = '3250000'>Vùng 4: 3.250.000 đồng/tháng</option>" +
                    "                   </select>\r\n" +
                    "              </div>\r\n                 " +
                    "              <div class =\"col-sm-8\">" +
                    "                  <label style=\"font-weight:bold;margin-bottom:5px\">Số người phụ thuộc </label>\r\n                 " +
                    "                  <input   id=\"txtSoNguoiPhuThuoc\" type=\"number\" placeholder=\"Số người phụ thuộc\" class=\"input-sm form-control\">\r\n "+
                    "               </div>\r\n  "+  
                    "              <div class=\"col-sm-12\">\r\n" +
                    "                   <button onclick=\"javascript:GrossToNet();\" type=\"button\" class=\"btn btn-sm btn-primary mr-5px\">Gross -> Net</button>\r\n  " +
                    "                   <button onclick=\"javascript:NetToGross();\" type=\"button\" class=\"btn btn-sm btn-primary mr-5px\">Net -> Gross</button>\r\n "+
                    "              </div>\r\n                 " +
                    "              <div id=\"divProcessing\" class=\"processing\"></div>\r\n          " +
                    "              <div class=\"col-sm-12\" id='TabCHuyenDoi'>\r\n     " +
                    "                  <div id='divDataContent'> ", TinhLuong.ServerSideDrawSearchResult(ORenderInfo).HtmlContent,"</div>\r\n       " +
                    "               </div > \r\n"+                  
                    "           </div > \r\n"+
                    "       </div> \r\n   " +
                    "   </div> \r\n" +
                    "</div>\r\n" +
                    "<div id=\"divActionForm\" style=\"display:none\"></div>\r\n" +
                    "   <div id=\"divmyModal\" class=\"modal fade\" style=\"overflow: hidden\" role=\"dialog\" aria-labelledby=\"largeModal\" aria-hidden=\"true\" data-backdrop=\"static\">\r\n" +
                    "    <div class=\"modal-dialog\">\r\n        " +
                    "       <div class=\"modal-content\">\r\n            " +
                    "           <div class=\"panel-heading\">\r\n                " +
                    "               <button type=\"button\" class=\"close\"  data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>\r\n                " +
                    "               <h2 class=\"modal-title\" id=\"TitleModal\"><span class=\"\">Thông tin</span></h2>\r\n            " +
                    "           </div> \r\n            " +
                    "           <div class=\"modal-body\" id=\"myModal\">\r\n            " +
                    "           </div>\r\n        " +
                    "       </div>\r\n    " +
                    "   </div>\r\n" +
                    "</div>\r\n"
                })) +
                
                WebEnvironments.ProcessJavascript(
                    "<script language=\"javascript\">\r\n   " +
                    "   RenderInfo = CreateRenderInfo();\r\n   " +
                    "   function RealCallReading()\r\n   " +
                    "   {\r\n       " +
                    "      AjaxOut = ADMIN.WebParts.TinhLuong.ServerSideDrawSearchResult(RenderInfo).value;\r\n       " +
                    "      if(AjaxOut.Error)\r\n       " +
                    "      {\r\n           " +
                    "      callGallAlert(AjaxOut.InfoMessage);\r\n           " +
                    "      return;\r\n       " +
                    "      }\r\n       " +
                    "      document.getElementById('divDataContent').innerHTML=AjaxOut.HtmlContent;\r\n   " +
                    "   }\r\n" +
                    "   function GrossToNet()\r\n   " +
                    "   {\r\n" +
                    "      ThuNhap = document.getElementById('txtThuNhap').value;\r\n" +
                    "      KhuVuc = document.getElementById('cboKhuVuc').value;\r\n" +
                    "      SoNguoiPhuThuoc = document.getElementById('txtSoNguoiPhuThuoc').value;\r\n" +
                    "       if(txtThuNhap.value == '' || txtThuNhap.value == null)\r\n" +
                    "       {\r\n" +
                    "           callGallAlert('Bạn chưa nhập thu nhập!');\r\n" +
                    "           return false;\r\n" +
                    "       }\r\n" +
                    "      AjaxOut = ADMIN.WebParts.TinhLuong.ServerSideAddGrossToNet(RenderInfo, ThuNhap, KhuVuc, SoNguoiPhuThuoc).value;\r\n       " +
                    "      if(AjaxOut.Error)\r\n       " +
                    "      {\r\n           " +
                    "          callGallAlert(AjaxOut.InfoMessage);\r\n           " +
                    "          return;\r\n       " +
                    "      }\r\n       " +
                    "      $('#TabCHuyenDoi').html(AjaxOut.HtmlContent);\r\n" +
                    "   }\r\n" +
                    "   function NetToGross()\r\n   " +
                    "   {\r\n       " +
                    "      ThuNhap = document.getElementById('txtThuNhap').value;\r\n" +
                    "      KhuVuc = document.getElementById('cboKhuVuc').value;\r\n" +
                    "      SoNguoiPhuThuoc = document.getElementById('txtSoNguoiPhuThuoc').value;\r\n" +
                    "      AjaxOut = ADMIN.WebParts.TinhLuong.ServerSideAddNetToGross(RenderInfo, ThuNhap, KhuVuc, SoNguoiPhuThuoc).value;\r\n       " +
                    "      if(AjaxOut.Error)\r\n       " +
                    "      {\r\n           " +
                    "      callGallAlert(AjaxOut.InfoMessage);\r\n           " +
                    "      return;\r\n       " +
                    "      }\r\n       " +
                    "      $('#TabCHuyenDoi').html(AjaxOut.HtmlContent);\r\n" +
                    "   }\r\n" +
                    "</script>\r\n");
            }
            catch (Exception ex)
            {
                ajaxOut.Error = true;
                ajaxOut.InfoMessage = (string.IsNullOrEmpty(ex.Message) ? ex.ToString() : ex.Message);
            }
            return ajaxOut;
        }

        [AjaxMethod(HttpSessionStateRequirement.ReadWrite)]
        public static AjaxOut ServerSideDrawSearchResult(RenderInfoCls ORenderInfo)
        {
            AjaxOut RetAjaxOut = new AjaxOut();
            try
            {
                SiteParam OSiteParam = WebEnvironments.CreateSiteParam(ORenderInfo);

                List<BangChuyenDoiCls> OChuyenDois = WebSessionUtility.GetSession(OSiteParam, "BangChuyenDois") as List<BangChuyenDoiCls>;
                string text =
                    "   <div class=\"search-result-info\"></div>\r\n" +
                    "   <div class=\"table-responsive\"> \r\n" +
                    "       <table class=\"table table-bordered table-hover dataTables-autosort footable-loaded footable\"> \r\n" +
                    "           <thead> \r\n" +
                    "               <tr> \r\n" +
                    "                   <th style=\"width:50px\">STT</th> \r\n " +
                    "                   <th style=\"min-width:150px\">Lương Gross</th> \r\n" +
                    "                   <th style=\"min-width:150px\">Bảo hiểm</th> \r\n" +
                    "                   <th style=\"min-width:150px\">Thuế TNCN</th> \r\n" +
                    "                   <th style=\"min-width:150px\">Lương Net</th> \r\n" +
                    "               </tr> \r\n" +
                    "           </thead> \r\n" +
                    "           <tbody> \r\n";
                for (int i = 0; i < OChuyenDois.Count; i++)
                {
                    text +=
                    "               <tr> \r\n" +
                    "                 <td style=\"text-align: center;\">" + (i + 1) + "</td> \r\n" +
                    "                 <td>" + OChuyenDois[i].LuongGross.ToString("N0") + "</td> \r\n" +
                    "                 <td>" + OChuyenDois[i].BaoHiem.ToString("N0") + "</td> \r\n" +
                    "                 <td>" + OChuyenDois[i].ThueTNCN.ToString("N0") + "</td> \r\n " +
                    "                 <td>" + OChuyenDois[i].LuongNet.ToString("N0") + "</td> \r\n "+
                    "               </tr> \r\n";
                }
                text += 
                    "           </tbody> \r\n     " +
                    "       </table></br>\r\n " +
                    "   </div>\r\n";
                text += 
                    "   <div style=\"font-weight:bold;margin-bottom:5px\">Diễn giải chi tiết </div>\r\n                 ";
                text += 
                    "   <div class=\"search-result-info\"></div>\r\n" +
                    "   <div class=\"table-responsive\"> \r\n" +
                    "       <table class=\"table table-bordered table-hover dataTables-autosort footable-loaded footable\"> \r\n" +
                    "           <tbody> \r\n";
                
                for (int i = 0; i < OChuyenDois.Count; i++)
                {
                    text +=
                    "               <tr> \r\n" +
                    "                 <th>Lương Gross</th>\r\n" +
                    "                 <td>"+ OChuyenDois[i].LuongGross.ToString("N0") +"</td> \r\n" +
                    "               </tr> \r\n"+
                    "               <tr> \r\n" +
                    "                 <th>Bảo hiểm xã hội(8%)</th>\r\n" +
                    "                 <td>" + OChuyenDois[i].BHXH.ToString("N0") + "</td> \r\n" +
                    "               </tr> \r\n" +
                    "               <tr> \r\n" +
                    "                 <th>Bảo hiểm y tế (1.5%)\t</th>\r\n" +
                    "                 <td>" + OChuyenDois[i].BHYT.ToString("N0") + "</td> \r\n" +
                    "               </tr> \r\n" +
                    "               <tr> \r\n" +
                    "                 <th>Bảo hiểm thất nghiệp (1%)</th>\r\n" +
                    "                 <td>" + OChuyenDois[i].BHTN.ToString("N0") + "</td> \r\n" +
                    "               </tr> \r\n" +
                    "               <tr> \r\n" +
                    "                 <th>Thu nhập trước thuế</th>\r\n" +
                    "                 <td>" + OChuyenDois[i].ThuNhapTT.ToString("N0") + "</td> \r\n" +
                    "               </tr> \r\n" +
                    "               <tr> \r\n" +
                    "                 <th>Giảm trừ gia cảnh bản thân</th>\r\n" +
                    "                 <td>-11,000,000</td> \r\n" +
                    "               </tr> \r\n" +
                    "               <tr> \r\n" +
                    "                 <th>Giảm trừ gia cảnh người phụ thuộc</th>\r\n" +
                    "                 <td>" + OChuyenDois[i].SNPT.ToString("N0") + "</td> \r\n" +
                    "               </tr> \r\n" +
                    "               <tr> \r\n" +
                    "                 <th>Thu nhập chịu thuế</th>\r\n" +
                    "                 <td>" + OChuyenDois[i].ThuNhapCT.ToString("N0") + "</td> \r\n" +
                    "               </tr> \r\n" +
                    "               <tr> \r\n" +
                    "                 <th>Thuế thu nhập cá nhân(*)</th>\r\n" +
                    "                 <td>" + OChuyenDois[i].ThueTNCN.ToString("N0") + "</td> \r\n" +
                    "               </tr> \r\n" +
                    "               <tr> \r\n" +
                    "                 <th>Lương NET</th>\r\n" +
                    "                 <td>" + OChuyenDois[i].LuongNet.ToString("N0") + "</td> \r\n" +
                    "               </tr> \r\n";
                }
                text +=
                    "           </tbody> \r\n     " +
                    "       </table></br>\r\n " +
                    "   </div>\r\n";
                    
                RetAjaxOut.HtmlContent = WebEnvironments.ProcessHtml(text);
            }
            catch (Exception ex)
            {
                RetAjaxOut.Error = true;
                RetAjaxOut.InfoMessage = (string.IsNullOrEmpty(ex.Message) ? ex.ToString() : ex.Message);
            }
            return RetAjaxOut;
        }
       
        [AjaxMethod(HttpSessionStateRequirement.ReadWrite)]
        public static AjaxOut ServerSideAddGrossToNet(RenderInfoCls ORenderInfo, string ThuNhap, string KhuVuc, string SNPT)
        {
            AjaxOut RetAjaxOut = new AjaxOut();
            try
            {
                SiteParam OSiteParam = WebEnvironments.CreateSiteParam(ORenderInfo);                         
                WebSession.CheckSessionTimeOut(ORenderInfo);
                double thunhap = double.Parse(ThuNhap);
                double khuvuc = double.Parse(KhuVuc);
                double snpt = 0;
                if (!string.IsNullOrEmpty(SNPT))
                    snpt = double.Parse(SNPT);
                double BaoHiem = 0;
                double BHXH = 0;
                double BHYT = 0;
                double BHTN = 0;         
                double ThuNhapCT = 0;
                double ThueTNCN = 0;
                double LuongNet = 0;

                if (thunhap/1490000 < 20)
                {
                    BHXH = thunhap * 8 / 100;
                }    
                else
                {
                    BHXH = 20 * 1490000 * 8 / 100;
                }


                if (thunhap / 1490000 < 20)
                {
                    BHYT = thunhap * 1.5 / 100;
                }
                else
                {
                    BHYT = 20 * 1490000 * 1.5 / 100;
                }

                if (thunhap / khuvuc < 20)
                {
                    BHTN = thunhap * 1 / 100;
                }
                else
                {
                    BHTN = 20 * khuvuc * 1 / 100;
                }

                BaoHiem = BHYT + BHXH + BHTN;
                
                ThuNhapCT = thunhap - BaoHiem - 11000000 - 4400000 * snpt;
                if(ThuNhapCT < 0)
                {
                    ThuNhapCT = 0;
                    ThueTNCN = 0;
                }
                else
                {
                    if (thunhap < 5000000)
                    {
                        ThueTNCN = ThuNhapCT * 5 / 100;
                    }
                    else if (ThuNhapCT > 5000000 && ThuNhapCT < 10000000)
                    {
                        ThueTNCN = ThuNhapCT * 10 / 100 - 250000;
                    }
                    else if (ThuNhapCT > 10000000 && ThuNhapCT < 18000000)
                    {
                        ThueTNCN = ThuNhapCT * 15 / 100 - 750000;
                    }
                    else if (ThuNhapCT > 18000000 && ThuNhapCT < 32000000)
                    {
                        ThueTNCN = ThuNhapCT * 20 / 100 - 1650000;
                    }
                    else if (ThuNhapCT > 32000000 && ThuNhapCT < 52000000)
                    {
                        ThueTNCN = ThuNhapCT * 25 / 100 - 3250000;
                    }
                    else if (ThuNhapCT > 52000000 && ThuNhapCT < 800000000)
                    {
                        ThueTNCN = ThuNhapCT * 30 / 100 - 5850000;
                    }
                    else
                    {
                        ThueTNCN = ThuNhapCT * 35 / 100 - 9850000;
                    }
                }
             
                LuongNet = thunhap - BaoHiem - ThueTNCN;
                List<BangChuyenDoiCls> OChuyenDois = WebSessionUtility.GetSession(OSiteParam, "BangChuyenDois") as List<BangChuyenDoiCls>;
                OChuyenDois.Add(new BangChuyenDoiCls()
                {       
                    ID = Guid.NewGuid().ToString(),
                    BHXH = - BHXH,
                    BHYT = - BHYT,
                    BHTN = - BHTN,
                    ThuNhapCT = ThuNhapCT,
                    ThuNhapTT = thunhap - BaoHiem,
                    LuongGross = thunhap,
                    BaoHiem = - BaoHiem,
                    ThueTNCN = ThueTNCN,
                    LuongNet = LuongNet,
                    SNPT = - snpt * 4400000,
                });           
                RetAjaxOut.HtmlContent = ServerSideDrawSearchResult(ORenderInfo).HtmlContent;
            }
            catch (Exception ex)
            {
                RetAjaxOut.Error = true;
                RetAjaxOut.InfoMessage = ex.Message.ToString();
                RetAjaxOut.HtmlContent = ex.Message.ToString();
            }
            return RetAjaxOut;
        }
        [AjaxMethod(HttpSessionStateRequirement.ReadWrite)]
        public static AjaxOut ServerSideAddNetToGross(RenderInfoCls ORenderInfo, string ThuNhap, string KhuVuc, string SNPT)
        {
            AjaxOut RetAjaxOut = new AjaxOut();
            try
            {
                SiteParam OSiteParam = WebEnvironments.CreateSiteParam(ORenderInfo);
                WebSession.CheckSessionTimeOut(ORenderInfo);
                double thunhap = double.Parse(ThuNhap);
                double khuvuc = double.Parse(KhuVuc);
                double snpt = 0;
                if (!string.IsNullOrEmpty(SNPT))
                    snpt = double.Parse(SNPT);
                double BaoHiem = 0;
                double BHXH = 0;
                double BHYT = 0;
                double BHTN = 0;
                double ThuNhapCT = 0;
                double ThueTNCN = 0;
                double GRGD = 0;
                double TNTT = 0;
                double TNQD = 0;
                TNQD = thunhap - (11000000 + snpt * 4400000);
                if(thunhap < 11000000)
                {
                    TNQD = 0; TNTT = 0 ; ThueTNCN = 0;
                }   
                else
                {
                    if (TNQD < 4750000)
                    {
                        TNTT = TNQD / 0.95;
                    }
                    else if (TNQD > 4750000 && TNQD < 9250000)
                    {
                        TNTT = (TNQD - 250000) / 0.9;
                    }
                    else if (TNQD > 9250000 && TNQD < 16050000)
                    {
                        TNTT = (TNQD - 750000) / 0.85;
                    }
                    else if (TNQD > 16050000 && TNQD > 27250000)
                    {
                        TNTT = (TNQD - 1650000) / 0.8;
                    }
                    else if (TNQD > 27250000 && TNQD < 42250000)
                    {
                        TNTT = (TNQD - 3250000) / 0.75;
                    }
                    else if (TNQD > 42250000 && TNQD < 61850000)
                    {
                        TNTT = (TNQD - 5850000) / 0.7;
                    }
                    else
                    {
                        TNTT = (TNQD - 9850000) / 0.65;
                    }
                    //ThueTNCN
                    if (TNTT < 5000000)
                    {
                        ThueTNCN = TNTT * 5 / 100;
                    }
                    else if (TNTT > 5000000 && TNTT < 10000000)
                    {
                        ThueTNCN = TNTT * 10 / 100 - 250000;
                    }
                    else if (TNTT > 10000000 && TNTT < 18000000)
                    {
                        ThueTNCN = TNTT * 15 / 100 - 750000;
                    }
                    else if (TNTT > 18000000 && TNTT < 32000000)
                    {
                        ThueTNCN = TNTT * 20 / 100 - 1650000;
                    }
                    else if (TNTT > 32000000 && TNTT < 52000000)
                    {
                        ThueTNCN = TNTT * 25 / 100 - 3250000;
                    }
                    else if (TNTT > 52000000 && TNTT < 80000000)
                    {
                        ThueTNCN = TNTT * 30 / 100 - 5850000;
                    }
                    else
                    {
                        ThueTNCN = TNTT * 35 / 100 - 9850000;
                    }
                }

                GRGD = (TNTT + 11000000 + snpt* 4400000) / 0.895;
                //BaoHiem

                if (GRGD / 1490000 < 20)
                {
                    BHXH = GRGD * 8 / 100;
                }
                else
                {
                    BHXH = 20 * 1490000 * 8 / 100;
                }

                if (GRGD / 1490000 < 20)
                {
                    BHYT = GRGD * 1.5 / 100;
                }
                else
                {
                    BHYT = 20 * 1490000 * 1.5 / 100;
                }

                if (GRGD / khuvuc < 20)
                {
                    BHTN = GRGD * 1 / 100;
                }
                else
                {
                    BHTN = 20 * khuvuc * 1 / 100;
                }
                List<BangChuyenDoiCls> OChuyenDois = WebSessionUtility.GetSession(OSiteParam, "BangChuyenDois") as List<BangChuyenDoiCls>;
                OChuyenDois.Add(new BangChuyenDoiCls()
                {
                    ID = Guid.NewGuid().ToString(),
                    BHXH = BHXH,
                    BHYT = BHYT,
                    BHTN = BHTN,
                    ThuNhapCT = ThuNhapCT,
                    ThuNhapTT = thunhap - BaoHiem,
                    LuongGross = GRGD,
                    BaoHiem = BaoHiem,
                    ThueTNCN = ThueTNCN,
                    LuongNet = thunhap,
                    SNPT = snpt*4400000,
                });
                RetAjaxOut.HtmlContent = ServerSideDrawSearchResult(ORenderInfo).HtmlContent;
            }
            catch (Exception ex)
            {
                RetAjaxOut.Error = true;
                RetAjaxOut.InfoMessage = ex.Message.ToString();
                RetAjaxOut.HtmlContent = ex.Message.ToString();
            }
            return RetAjaxOut;
        }
        public class BangChuyenDoiCls
        {
            public string ID { get; set; }
            public double BHXH { get; set; }
            public double BHYT { get; set; }
            public double BHTN { get; set; }
            public double BaoHiem { get; set; }
            public double LuongGross { get; set; }
            public double ThuNhapCT { get; set; }
            public double ThuNhapTT { get; set; }
            public double ThueTNCN { get; set; }
            public double LuongNet { get; set; }
            public double SNPT { get; set; }
        }
    }
}
