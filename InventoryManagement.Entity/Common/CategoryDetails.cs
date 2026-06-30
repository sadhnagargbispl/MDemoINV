using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class CategoryDetails
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string IsAdd { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedById { get; set; }
        public User UserDetails { get; set; }
        public string ImgPath { get; set; }
    }
    public class ColorDetails
    {
        public int Id { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string IsAdd { get; set; }
    }

    public class SizeDetails
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string IsAdd { get; set; }
    }
    public class ProductInfo
    {
        public int prodInfoid { get; set; }
        public string prodid { get; set; }
        public string prodname { get; set; }
        public ColorDetails ColorDetails { get; set; }
        public SizeDetails SizeDetails { get; set; }
        public HttpPostedFileBase[] upload { get; set; }
        public string ProductImagePath { get; set; }
        public string ProductImagePath1 { get; set; }
        public string ProductImagePath2 { get; set; }
        public string ProductImagePath3 { get; set; }
        public string ProductImagePath4 { get; set; }
        public string ProductImagePath5 { get; set; }
    }
    //public class CategoryDetails
    //{
    //    public int CategoryId { get; set; }
    //    public string CategoryName { get; set; }
    //    public string Description { get; set; }
    //    public bool IsActive { get; set; }
    //    public string IsAdd { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //    public int CreatedById { get; set; }
    //    public User UserDetails { get; set; }
    //}
}