using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace MyToDo.Events
{
    public class UpdateModel
    {
        public bool IsOpenModel { get; set; }
    }

    public class UpdateLoadingModel : PubSubEvent<UpdateModel>
    {

    }
}
