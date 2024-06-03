using MrG.AI.Client.Database.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MrG.AI.Client.VM
{
    public class RequestListVM : ObservableObject
    {
        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public string _sortOrder = "Desc";

        public string SortOrder
        {
            get { return _sortOrder; }
            set
            {
                _sortOrder = value;
                OnPropertyChanged();
            }
        }
        
        public List<string> SortOrderOptions { get; } = new List<string> { "Asc", "Desc" };


        
        private ObservableCollection<Request> requests = new ObservableCollection<Request>();
        public ObservableCollection<Request> Requests {
            get { return requests; }
            set
            {                
                requests = value;
                OnPropertyChanged();
            }

        }
        private bool _loadMoreVisible = false;
        public bool LoadMoreVisible
        {
            get { return _loadMoreVisible; }
            set
            {
                _loadMoreVisible = value;
                OnPropertyChanged();
            }
        }

    }
}
