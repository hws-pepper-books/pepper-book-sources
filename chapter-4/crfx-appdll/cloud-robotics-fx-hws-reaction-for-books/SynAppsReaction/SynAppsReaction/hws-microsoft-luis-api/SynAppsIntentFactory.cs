using System;
using System.Collections.Generic;
using System.Linq;

namespace SynAppsLuis
{
    class SynAppsIntentFactory
    {
        private string SynappsDeviceId;
        
        public SynAppsIntentFactory(string synappsDeviceId)
        {
            this.SynappsDeviceId = synappsDeviceId;
        }
        public List<SynAppsIntentModel> CreateSynAppsIntentList(bool isAlwaysUseCache = false)
        {
            return CreateFromSQLDatabase();
        }

        private List<SynAppsIntentModel> CreateFromSQLDatabase()
        {
            return SynAppsIntentModel.FindAllByDeviceId(this.SynappsDeviceId);
        }

        private void RefreshSQLDatabase(List<SynAppsIntentModel> list)
        {
            SynAppsIntentModel.Refresh(this.SynappsDeviceId, list);
        }

        private bool IsRefresh()
        {
            return SynAppsIntentModel.IsRefresh(this.SynappsDeviceId);
        }
    }
}
