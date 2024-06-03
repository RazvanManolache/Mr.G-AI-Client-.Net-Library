using MrG.AI.Client.Data.Action;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrG.AI.Client.VM
{
    public class ActionListVM : ObservableObject
    {
        private List<ActionApi> actions;
        public List<ActionApi> Actions
        {
            get => actions;
            set
            {
                actions = value;
                SetProperty(ref actions, value);
            }
        }

    }
}
