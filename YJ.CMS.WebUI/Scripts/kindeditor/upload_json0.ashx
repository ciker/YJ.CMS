<%@ webhandler Language="C#" class="Upload" %>

/**
 * KindEditor ASP.NET
 *
 * 本ASP.NET程序是演示程序，建议不要直接在实际项目中使用。
 * 如果您确定直接使用本程序，使用之前请仔细确认相关安全设置。
 *
 */

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using LitJson;

public class Upload : IHttpHandler
{
	private HttpContext context;

	public void ProcessRequest(HttpContext context)
	{
		String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);
		
		//文件保存目录路径
        String savePath = "/Upload/BusinessLicenseImg/";

		//文件保存目录URL
        String saveUrl = "/Upload/BusinessLicenseImg/";

		//定义允许上传的文件扩展名
		Hashtable extTable = new Hashtable();
		extTable.Add("image", "gif,jpg,jpeg,png,bmp");
		extTable.Add("flash", "swf,flv");
		extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
		extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

		//最大文件大小
		int maxSize = 1000000;
		this.context = context;

		HttpPostedFile imgFile = context.Request.Files["imgFile"];
        
		if (imgFile == null)
		{
			showError("请选择文件。");
		}

		String dirPath = context.Server.MapPath(savePath);
		if (!Directory.Exists(dirPath))
		{
			showError("上传目录不存在。");
		}

		String dirName = context.Request.QueryString["dir"];
		if (String.IsNullOrEmpty(dirName)) {
			dirName = "image";
		}
		if (!extTable.ContainsKey(dirName)) {
			showError("目录名不正确。");
		}

		String fileName = imgFile.FileName;
		String fileExt = Path.GetExtension(fileName).ToLower();

		if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
		{
			showError("上传文件大小超过限制。");
		}

		if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
		{
			showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
		}
        
        
		//创建文件夹
		//dirPath += dirName + "/";
		//saveUrl += dirName + "/";
		//if (!Directory.Exists(dirPath)) {
		//	Directory.CreateDirectory(dirPath);
		//}
		//String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
		//dirPath += ymd + "/";
		//saveUrl += ymd + "/";
		if (!Directory.Exists(dirPath)) {
			Directory.CreateDirectory(dirPath);
		}

		String newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
		String filePath = dirPath + newFileName;

		imgFile.SaveAs(filePath);
        //检查真实类型
        if (!IsAllowedExtension(filePath))
        {
            File.Delete(filePath);
            showError("上传文件类型错误");
        }
        newFileName = newFileName.Replace("/", "");
		Hashtable hash = new Hashtable();
		hash["error"] = 0;
        hash["url"] = newFileName;
		context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
		context.Response.Write(JsonMapper.ToJson(hash));
		context.Response.End();
	}

	private void showError(string message)
	{
		Hashtable hash = new Hashtable();
		hash["error"] = 1;
		hash["message"] = message;
		context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
		context.Response.Write(JsonMapper.ToJson(hash));
		context.Response.End();
	}

    //检查文件真实类型
    private bool IsAllowedExtension(string filePath)
    {
        bool ret = false;

        System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
        string fileclass = "";
        byte buffer;
        try
        {
            buffer = r.ReadByte();
            fileclass = buffer.ToString();
            buffer = r.ReadByte();
            fileclass += buffer.ToString();
        }
        catch
        {
            return false;
        }
        r.Close();
        fs.Close();
        String[] fileType = { "255216", "7173", "6677", "13780" };

        for (int i = 0; i < fileType.Length; i++)
        {
            if (fileclass == fileType[i])
            {
                ret = true;
                break;
            }
        }
        return ret;
    } 

	public bool IsReusable
	{
		get
		{
			return true;
		}
	}
}
