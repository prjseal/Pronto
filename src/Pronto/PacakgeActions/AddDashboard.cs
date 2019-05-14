using System;
using System.Xml;
using umbraco.interfaces;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using Umbraco.Core.Logging;
using umbraco.IO;

namespace Pronto.PacakgeActions
{
    public class AddDashboard : IPackageAction
    {
        public string Alias()
        {
            return "AddDashboard";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            LogHelper.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, String.Format("Start Action {0}: Execute() for package {1}.", this.Alias(), packageName));
            try
            {
                this.AddSectionDashboard("prontoDashboardSection", "content", "Welcome", "/App_Plugins/Pronto/dashboard.html");
                LogHelper.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, String.Format("Complete Action {0}: Execute() for package {1}.", this.Alias(), packageName));
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, String.Format("Error in {0}: Execute() for package {1}.", this.Alias(), packageName), ex);
                return false;
            }
        }

        public void AddSectionDashboard(string sectionAlias, string area, string tabCaption, string src)
        {
            bool saveFile = false;
            var dashboardFilePath = IOHelper.MapPath(SystemFiles.DashboardConfig);

            XmlDocument dashboardXml = new XmlDocument();
            dashboardXml.Load(dashboardFilePath);

            XmlNode findSection = dashboardXml.SelectSingleNode("//section [@alias='" + sectionAlias + "']");

            if (findSection == null)
            {
                var xmlToAdd = "<section alias='" + sectionAlias + "'>" +
                                    "<areas>" +
                                        "<area>" + area + "</area>" +
                                    "</areas>" +
                                    "<tab caption='" + tabCaption + "'>" +
                                        "<control addPanel='true' panelCaption=''>" + src + "</control>" +
                                    "</tab>" +
                               "</section>";

                //Get the main root <dashboard> node
                XmlNode dashboardNode = dashboardXml.SelectSingleNode("//dashBoard");

                if (dashboardNode != null)
                {
                    //Load in the XML string above
                    XmlDocument xmlNodeToAdd = new XmlDocument();
                    xmlNodeToAdd.LoadXml(xmlToAdd);

                    var toAdd = xmlNodeToAdd.SelectSingleNode("*");

                    //Prepend the xml above to the dashboard node - so that it will be the first dashboards to show in the backoffice.
                    dashboardNode.PrependChild(dashboardNode.OwnerDocument.ImportNode(toAdd, true));

                    //Save the file flag to true
                    saveFile = true;
                }
            }

            if (saveFile)
            {
                dashboardXml.Save(dashboardFilePath);
            }
        }


        public XmlNode SampleXml()
        {
            string sample = "<Action runat=\"install\" undo=\"true/false\" alias=\"AddDasboard\"/>";

            return helper.parseStringToXmlNode(sample);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            this.RemoveDashboardTab("prontoDashboardSection");

            return true;
        }

        public void RemoveDashboardTab(string sectionAlias)
        {

            string dbConfig = IOHelper.MapPath(SystemFiles.DashboardConfig);
            XmlDocument dashboardFile = new XmlDocument();
            dashboardFile.Load(dbConfig);

            XmlNode section = dashboardFile.SelectSingleNode("//section [@alias = '" + sectionAlias + "']");

            if (section != null)
            {
                dashboardFile.SelectSingleNode("//dashBoard").RemoveChild(section);
                dashboardFile.Save(dbConfig);
            }

        }
    }
}
