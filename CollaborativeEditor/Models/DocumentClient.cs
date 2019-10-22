using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVP.CollaborativeEditor.Models
{
    public class DocumentClient : DtoBase
    {
        private string _name;

        public DocumentClient()
        {
        }

        public string Name { get => _name; set => Set(ref _name, value); }


    }
}
