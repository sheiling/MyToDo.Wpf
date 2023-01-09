using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common
{
    public class MenuBar : BindableBase
    {
        /// <summary>
        /// 图标
        /// </summary>
        private string icon;
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        /// <summary>
        /// 名字
        /// </summary>
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 命名空间
        /// </summary>
        private string nameSpace;
        public string NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }



    }
}
