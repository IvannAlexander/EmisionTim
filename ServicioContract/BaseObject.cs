using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace ServicioModelo
{
    public class BaseObject
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(BaseObject));

        public BaseObject()
        {
            XmlConfigurator.Configure();
        }

    }
}
