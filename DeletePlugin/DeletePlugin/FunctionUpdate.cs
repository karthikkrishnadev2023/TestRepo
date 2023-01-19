using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;

namespace DeletePlugin
{
    public class DelPlugin : IPlugin
    {
 
        public void yaswanth(IServiceProvider serviceProvider){
                    String name = "Yaswanth";
        }
        public void Lalith(int a,int b){
            int c = a*b;
        }
        
        public void Kiran(int a, int b, int c)
        {
            int d = a+b+c;
        }
        
        
        String name :"sairam"
        
           public void sairam(int a,int b)
           {
            int c = a+b;
           }


        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingservice = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            EntityReference EntRec = null;
            if (context.InputParameters.Contains("Target"))
            {
                EntRec = (EntityReference)context.InputParameters["Target"];
            }

            Entity LeadRec = service.Retrieve(EntRec.LogicalName, EntRec.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));


            if(LeadRec.Contains("description") && LeadRec["description"] != null && LeadRec.Contains("new_f7") && LeadRec["new_f7"] != null && LeadRec.Contains("new_s7") && LeadRec["new_s7"] != null)
            {
                string decs = (string)LeadRec["description"];

                DateTime F7 = (DateTime)LeadRec["new_f7"];
                DateTime S7 = (DateTime)LeadRec["new_s7"];

                if (decs.Contains("confirmed for deletion") && (DateTime.Compare(F7, S7) < 0))
                {
                    Entity a = new Entity("contact");
                    a.Id = new Guid("9FD4A450-CB0C-EA11-A813-000D3A1B1223");
                    a["description"] = "It will work" + "\n" + "ID : " + EntRec.Id +"\n" + " Logical Name : " + EntRec.LogicalName + "RECORD DELETED!";
                    service.Update(a);
                }
                else
                {
                    return;
                }
            }

            
            

            
        }
    }
}
