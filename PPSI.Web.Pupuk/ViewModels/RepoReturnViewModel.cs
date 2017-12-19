using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPSI.Web.Pupuk.ViewModels
{
    public class RepoReturnViewModel<T>
    {
        public string Messages { get; set; }
        public T Payload { get; set; }
    }

    public class RepoReturnViewModel
    {
        public string Messages { get; set; }
        public object Payload { get; set; }
    }
}
