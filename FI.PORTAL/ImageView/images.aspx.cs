using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FI.PORTAL.dbconnect;
using FI.PORTAL.logics;

namespace FI.PORTAL
{
    public partial class images : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long commentID = Convert.ToInt64(Request.QueryString["comment"].ToString());
                    ImageLoad(commentID);
                }
            }
            catch(Exception ex)
            {

            }
        }
        private void ImageLoad(long commID)
        {
            try
            {
                requestinit_logic request = new requestinit_logic();
                List<MemoryStream> myimages = new List<MemoryStream>();
                myimages = request.Getimages(commID);
                if(myimages != null && myimages.Count > 0)
                {                   
                    if (myimages[0] != null)
                    {
                        image1.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(myimages[0].ToArray(), 0);
                    }
                    else
                    {
                        image2.ImageUrl = "~/Content/dist/img/404.png";
                    }
                    if (myimages[1].Capacity > 100 )
                    {
                        image2.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(myimages[1].ToArray(), 0);
                    }
                    else
                    {
                        image2.ImageUrl = "~/Content/dist/img/404.png";
                    }
                    if (myimages[2].Capacity > 100)
                    {
                        image3.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(myimages[2].ToArray(), 0);
                    }
                    else
                    {
                        image2.ImageUrl = "~/Content/dist/img/404.png";
                    }
                }

            }catch(Exception ex)
            {
                
            }
        }

        protected void btndownload1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = image1.ImageUrl;
                byte[] bytes = Convert.FromBase64String(url.Replace("data:image/jpeg;base64,", ""));
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=img.jpeg");
                Response.ContentType = "image/jpeg";
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.End();
            }
            catch { }
        }

        protected void btndownload2_Click(object sender, EventArgs e)
        {
            try
            {
                string url = image1.ImageUrl;
                byte[] bytes = Convert.FromBase64String(url.Replace("data:image/jpeg;base64,", ""));
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=img.jpeg");
                Response.ContentType = "image/jpeg";
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.End();
            }
            catch { }
        }

        protected void btndownload3_Click(object sender, EventArgs e)
        {
            try
            {
                string url = image1.ImageUrl;
                byte[] bytes = Convert.FromBase64String(url.Replace("data:image/jpeg;base64,", ""));
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=img.jpeg");
                Response.ContentType = "image/jpeg";
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.End();
            }
            catch { }
        }
    }
}