//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YJ.CMS.BLL
{
    public partial class ProductService : BaseService<Model.Product>, IBLL.IProductService
    {
    	public ProductService(IDAL.IBaseDAL<Model.Product> dal)
    	{
    		base.CurrentDal = dal;
    	}
    }
    
}
