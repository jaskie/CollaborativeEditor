using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVP.CollaborativeEditor.Models
{
    public class AppDataContext
    {
        private AppDataContext()
        { 
        }

        public Document Document { get; } = new Document();
        
        public static AppDataContext Instance { get; } = new AppDataContext();


    }
}
