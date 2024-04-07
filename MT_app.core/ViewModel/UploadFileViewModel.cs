using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MT_app.core.ViewModel
{
    public class UploadFileViewModel
    {
        public IFormFile FormFile { get; set; }
    }
}
