using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentUpload
{
    public class clsDocuments
    {
        public class ResumeDocument
        {
            public Int64 RID { get; set; }
            public Int64 ResID { get; set; }
            [MaxLength(20)]
            public string FileType { get; set; }
            [MaxLength(100)]
            public string FileName { get; set; }
            public byte[] FileData { get; set; }
            public string Description { get; set; }
            public Int64 ReqResID { get; set; }
        }


        public class GetResumeDoc
        {
            public string FileData { get; set; }
            public string FileName { get; set; }
            public Int16 FileType { get; set; }
        }

        public class GetHtmlText
        {
            public byte[] FileData { get; set; }
            public string FileName { get; set; }
            public string ResponseType { get; set; }
        }

        public class CreateResumeDocument
        {
            public Int64 RID { get; set; }
            public Int64 ResID { get; set; }
            [MaxLength(20)]
            public string FileType { get; set; }
            [MaxLength(100)]
            public string FileName { get; set; }
            public string FileData { get; set; }
            public string Description { get; set; }
            public string ResHTMLText { get; set; }
            public string ResConvertedText { get; set; }
        }


    }

}
