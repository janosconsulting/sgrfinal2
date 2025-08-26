using System;

namespace ERP.Web.Reportes
{
    public partial class KeepSessionAlive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //MetaRefresh.Attributes["content"] = Convert.ToString((Session.Timeout * 60) - 60) + ";url=KeepSessionAlive.aspx?q=" + DateTime.Now.Ticks;

            MetaRefresh.Attributes["content"] = Convert.ToString((5 * 60) - 60) + ";url=KeepSessionAlive.aspx?q=" + DateTime.Now.Ticks;
        }
    }
}